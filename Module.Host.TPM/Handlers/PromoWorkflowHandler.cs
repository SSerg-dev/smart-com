using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
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
            ILogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Promo processing started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));

                using (DatabaseContext context = new DatabaseContext())
                {
                    List<Promo> plannedPromoList = context.Set<Promo>().Where(x => x.PromoStatus.SystemName == "Planned" && !x.Disabled).ToList();
                    List<Promo> startedPromoList = context.Set<Promo>().Where(x => x.PromoStatus.SystemName == "Started" && !x.Disabled).ToList();

                    string massage;

                    foreach (Promo promo in plannedPromoList)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            bool status = promoStateContext.ChangeState(null, PromoStates.Started, "System", out massage);

                            context.SaveChanges();
                        }
                    }

                    foreach (Promo promo in startedPromoList)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            bool status = promoStateContext.ChangeState(null, PromoStates.Finished, "System", out massage);

                            context.SaveChanges();
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
                    handlerLogger.Write(true, e.ToString());
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Promo processing ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds));
                }
            }
        }
    }
}
