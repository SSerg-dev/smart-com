using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Utility.LogWriter;
using Module.Persist.TPM.Model.Interfaces;
using Module.Frontend.TPM.Util;

namespace Module.Host.TPM.Actions
{
    class ClientTreeUpdateAction : BaseAction
    {
        private LogWriter HandlerLogger { get; }
        private Guid UserId { get; }
        private Guid RoleId { get; }
        public string HandlerStatus { get; private set; }
        public int ClientTreeId { get; set; }
        public Guid HandlerId { get; set; }
        Stopwatch Stopwatch1 = new Stopwatch();
        public ClientTreeUpdateAction(Guid handlerId, LogWriter Logger, Guid userId, Guid roleId, int clentTreeId)
        {
            HandlerLogger = Logger;
            UserId = userId;
            RoleId = roleId;
            ClientTreeId = clentTreeId;
            HandlerId = handlerId;
        }

        public override void Execute()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Stopwatch1.Restart();
                ClientTree clientTree = context.Set<ClientTree>().FirstOrDefault(g => g.Id == ClientTreeId && g.EndDate == null);
                List<TradeInvestment> tradeInvestments = context.Set<TradeInvestment>().Where(x => !x.Disabled && x.ClientTreeId == clientTree.Id).ToList();
                List<COGS> COGSs = context.Set<COGS>().Where(x => !x.Disabled && x.ClientTreeId == clientTree.Id).ToList();
                List<PlanCOGSTn> PlanCOGSTns = context.Set<PlanCOGSTn>().Where(x => !x.Disabled && x.ClientTreeId == clientTree.Id).ToList();
                List<AssortmentMatrix> assortmentMatrices = context.Set<AssortmentMatrix>().Where(x => !x.Disabled && x.ClientTreeId == clientTree.Id).ToList();
                PromoHelper.ClientDispatchDays clientDispatchDays = PromoHelper.GetClientDispatchDays(clientTree);
                Guid onApproval = context.Set<PromoStatus>().FirstOrDefault(g => !g.Disabled && g.SystemName == "OnApproval").Id;
                List<string> Statuses = new List<string> { "DraftPublished", "OnApproval", "Approved", "Planned" };
                List<string> resetStatuses = new List<string> { "Approved", "Planned" };
                List<Promo> promos = context.Set<Promo>()
                    .Include(g => g.PromoProducts)
                    .Where(g => g.ClientTreeKeyId == clientTree.Id && Statuses.Contains(g.PromoStatus.SystemName) && !g.Disabled && !g.IsInExchange && g.TPMmode == TPMmode.Current)
                    .ToList();
                foreach (Promo promo in promos)
                {
                    DateTimeOffset oldDispStart = (DateTimeOffset)(promo.DispatchesStart);
                    DateTimeOffset oldDispEnd = (DateTimeOffset)(promo.DispatchesEnd);
                    DateTimeOffset startDate = (DateTimeOffset)(promo.StartDate);
                    DateTimeOffset endDate = (DateTimeOffset)(promo.EndDate);
                    if (clientDispatchDays.IsStartAdd)
                    {
                        promo.DispatchesStart = startDate.AddDays(clientDispatchDays.StartDays);
                    }
                    else
                    {
                        promo.DispatchesStart = startDate.AddDays(-clientDispatchDays.StartDays);
                    }
                    if (clientDispatchDays.IsEndAdd)
                    {
                        promo.DispatchesEnd = endDate.AddDays(clientDispatchDays.EndDays);
                    }
                    else
                    {
                        promo.DispatchesEnd = endDate.AddDays(-clientDispatchDays.EndDays);
                    }
                    string error = PromoHelper.CheckCogs(promo, clientTree, tradeInvestments, COGSs, PlanCOGSTns);
                    string errorMatrix = PromoHelper.CheckAssortmentMatrix(promo, assortmentMatrices);
                    if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorMatrix))
                    {
                        
                        if (resetStatuses.Contains(promo.PromoStatus.SystemName))
                        {
                            promo.PromoStatusId = onApproval;
                        }
                    }
                    else
                    {
                        promo.DispatchesStart = oldDispStart;
                        promo.DispatchesEnd = oldDispEnd;
                        if (!string.IsNullOrEmpty(error))
                        {
                            HandlerLogger.Write(true, error, "Warning");
                        }
                        if (!string.IsNullOrEmpty(errorMatrix))
                        {
                            HandlerLogger.Write(true, errorMatrix, "Warning");
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
