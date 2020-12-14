using Looper.Core;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class PromoWorkflowHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Promo processing started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    List<Promo> plannedPromoList = context.Set<Promo>().Where(x => x.PromoStatus.SystemName == PromoStates.Planned.ToString() && !x.Disabled).ToList(); 
                    List<Promo> startedPromoList = context.Set<Promo>().Where(x => x.PromoStatus.SystemName == PromoStates.Started.ToString() && !x.Disabled).ToList();
                    string message;

                    //переводить промо в OnApproval при наступлении даты, которая раньше отгрузки на два периода, надо только при изменении набора продуктов, механики и т.д.
                    //отдельно это условие не проверяется, поэтому код ниже закомментирован

                    /*
                    List<Promo> toOnApprovalPromoList = context.Set<Promo>().Where(x =>
                        (x.PromoStatus.SystemName == PromoStates.Approved.ToString() || 
                        x.PromoStatus.SystemName == PromoStates.Planned.ToString()) &&
                        !x.Disabled).ToList(); 

                    foreach (var promo in toOnApprovalPromoList)
                    {
                        if (PromoUtils.NeedBackToOnApproval(promo))
                        {
                            using (var promoStateContext = new PromoStateContext(context, promo))
                            {
                                bool status = promoStateContext.ChangeState(null, PromoStates.OnApproval, "System", out message);
                                if (status)
                                {
                                    //Сохранение изменения статуса
                                    var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                    promoStatusChange.PromoId = promo.Id;
                                    promoStatusChange.StatusId = promo.PromoStatusId;
                                    promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                    context.Set<PromoStatusChange>().Add(promoStatusChange);
                                    context.SaveChanges();
                                }
                            }   
                        }
                    }
                    */

                    foreach (Promo promo in plannedPromoList)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            
                            bool status = promoStateContext.ChangeState(promo, PromoStates.Started, "System", out message);

                            if (status)
                            {
                                //Сохранение изменения статуса
                                PromoStatusChange psc = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                psc.PromoId = promo.Id;
                                psc.StatusId = promo.PromoStatusId;
                                psc.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(psc);
                                context.SaveChanges();
                            }
                        }
                    }

                    foreach (Promo promo in startedPromoList)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            bool status = promoStateContext.ChangeState(null, PromoStates.Finished, "System", out message);

                            if (status)
                            {
                                //Сохранение изменения статуса
                                PromoStatusChange psc = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                psc.PromoId = promo.Id;
                                psc.StatusId = promo.PromoStatusId;
                                psc.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(psc);
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Promo processing ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
