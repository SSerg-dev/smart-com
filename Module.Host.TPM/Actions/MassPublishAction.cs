using Interfaces.Implementation.Action;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    class MassPublishAction : BaseAction
    {
        private LogWriter HandlerLogger { get; }
        private Guid UserId { get; }
        private Guid RoleId { get; }
        public string HandlerStatus { get; private set; }
        public List<Guid> PromoIds { get; set; }
        public Guid HandlerId { get; set; }
        readonly Stopwatch Stopwatch1 = new Stopwatch();

        public MassPublishAction(LogWriter handlerLogger, Guid userId, Guid roleId, List<Guid> promoIds, Guid handlerId)
        {
            HandlerLogger = handlerLogger;
            UserId = userId;
            RoleId = roleId;
            PromoIds = promoIds;
            HandlerId = handlerId;
        }
        public override void Execute()
        {
            using (DatabaseContext Context = new DatabaseContext())
            {
                Stopwatch1.Restart();

                using (var transaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
                        PromoStatus draftPublished = promoStatuses.FirstOrDefault(g => g.SystemName == "DraftPublished");
                        PromoStatus draft = promoStatuses.FirstOrDefault(g => g.SystemName == "Draft");
                        DateTimeOffset today = TimeHelper.TodayStartDay();
                        List<Promo> promoes = Context.Set<Promo>()
                            .Include(d => d.PromoBlockedStatus.BlockedPromoes)
                            .Include(d => d.PromoProductTrees)
                            .Where(g => PromoIds.Contains(g.Id) && g.StartDate > today && g.PromoStatusId == draft.Id)
                            .ToList();
                        List<Promo> notCopyPromoes = new List<Promo>();
                        OneLoadModel oneLoad = new OneLoadModel
                        {
                            ClientTrees = Context.Set<ClientTree>().Where(g => g.EndDate == null).ToList(),
                            BrandTeches = Context.Set<BrandTech>().Where(g => !g.Disabled).ToList(),
                            COGSs = Context.Set<COGS>().Where(x => !x.Disabled).ToList(),
                            PlanCOGSTns = Context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToList(),
                            ProductTrees = Context.Set<ProductTree>().Where(g => g.EndDate == null).ToList(),
                            TradeInvestments = Context.Set<TradeInvestment>().Where(x => !x.Disabled).ToList(),
                            Products = Context.Set<Product>().Where(g => !g.Disabled).ToList()
                        };
                        foreach (Promo promo in promoes)
                        {
                            List<Product> filteredProducts = new List<Product>();

                            try
                            {
                                filteredProducts = PromoHelper.CheckSupportInfo(promo, promo.PromoProductTrees.ToList(), oneLoad, Context);
                            }
                            catch (Exception ex)
                            {
                                HandlerLogger.Write(true, "Promo:" + promo.Number.ToString() + " - " + ex.Message, "Warning");
                                notCopyPromoes.Add(promo);
                                continue;
                            }
                            promo.Name = PromoHelper.GetPromoName(promo, Context);
                            promo.PromoStatusId = draftPublished.Id;
                        }
                        foreach (var item in notCopyPromoes)
                        {
                            promoes.Remove(item);
                        }
                        if (promoes.Count == 0)
                        {
                            HandlerLogger.Write(true, "No promoes to copy", "Error");
                            return;
                        }
                        Context.SaveChanges();
                        foreach (Promo promo in promoes)
                        {
                            Context.Set<ChangesIncident>().Add(new ChangesIncident
                            {
                                Id = Guid.NewGuid(),
                                DirectoryName = "PromoScenario",
                                ItemId = promo.Id.ToString(),
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                Disabled = false
                            });
                            promo.PromoBlockedStatus.DraftToPublished = true;
                            //promo.PromoBlockedStatus.BlockedPromoes.Add(new BlockedPromo
                            //{
                            //    Id = Guid.NewGuid(),
                            //    PromoBlockedStatusId = promo.Id,
                            //    HandlerId = HandlerId,
                            //    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            //    Disabled = false,
                            //});
                        }
                        Context.SaveChanges();
                        string promoesadd = string.Join(", ", promoes.Select(g => g.Number));
                        HandlerLogger.Write(true, "Promo add: " + promoesadd, "Message");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        HandlerLogger.Write(true, GetExceptionMessage.GetInnerException(ex).Message, "Error");
                        HandlerStatus = "HasErrors";
                        return;
                    }
                }




                //if ((DateTimeOffset)promo.DispatchesStart < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)promo.EndDate)
                //{
                //    HandlerLogger.Write(true, string.Format("ML Promo: {0} is not in the RS period, startdate: {1:yyyy-MM-dd HH:mm:ss}", inputMlId, promo.StartDate), "Warning");
                //    errorcount++;
                //}
            }
        }
    }
}
