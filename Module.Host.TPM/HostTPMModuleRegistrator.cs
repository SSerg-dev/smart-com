using Core.ModuleRegistrator;
using Looper.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Host.TPM {
    public class HostTPMModuleRegistrator : IHostModuleRegistrator {
        public IEnumerable<IHandler> GetHandlers() {
            return new List<IHandler>()
            {
                new Handlers.FullXLSXUpdateImportPromoSalesHandler(),
                new Handlers.FullXLSXUpdateImportDemandHandler(),
                new Handlers.FullXLSXUpdateImportClientShareHandler(),
                new Handlers.FullXLSXCOGSUpdateImporHandler(),
                new Handlers.FullXLSXTradeInvestmentUpdateImporHandler(),
                new Handlers.FullXLSXImportBaseLineHandler(),
                new Handlers.UpdateUpliftHandler(),
                new Handlers.CalculatePromoParametersHandler(),
                new Handlers.CalculateBudgetsHandler(),
                new Handlers.PromoDemandChangeNotificationHandler(),
                new Handlers.PromoProductChangeNotificationHandler(),
                new Handlers.CalculateActualParamatersHandler(),
                new Handlers.PromoWorkflowHandler(),
                new Handlers.FullXLSXImportPromoProductHandler(),
                new Handlers.PromoUpliftFailNotificationHandler(),
                new Handlers.BaseLineUpgradeHandler(),
                new Handlers.SchedulerExportHandler(),
                new Handlers.FullXLSXUpdateBrandTechHandler(),
                new Handlers.FullXLSXUpdateAllHandler(),
                new Handlers.FullXLSXUpdateBudgetSubItemHandler(),
                new Handlers.FullXLSXNoNegoUpdateImporHandler(),
                new Handlers.FullXLSXAssortmentMatrixImportHandler(),
                new Handlers.AutoResetPromoHandler(),
                new Handlers.RejectPromoNotificationHandler(),
                new Handlers.XLSXImportActualLsvHandler(),
                new Handlers.ActualLSVChangeHandler()
            };
        }
    }
}
