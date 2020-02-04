using Core.ModuleRegistrator;

using Looper.Core;

using System.Collections.Generic;

namespace Module.Host.TPM
{
    public class HostTPMModuleRegistrator : IHostModuleRegistrator {
        public IEnumerable<IHandler> GetHandlers() {
            return new List<IHandler>()
            {
				new Handlers.Notifications.PromoDemandChangeNotificationHandler(),
				new Handlers.Notifications.PromoProductChangeNotificationHandler(),
				new Handlers.Notifications.PromoUpliftFailNotificationHandler(),
				new Handlers.Notifications.CancelledPromoNotificationHandler(),
				new Handlers.Notifications.WeekBeforeDispatchPromoNotificationHandler(),
				new Handlers.Notifications.PromoOnApprovalNotificationHandler(),
				new Handlers.Notifications.PromoOnRejectNotificationHandler(),
				new Handlers.Notifications.PromoApprovedNotificationHandler(),
				new Handlers.Notifications.PromoROIReportNotificationHandler(),
				new Handlers.FullXLSXUpdateImportPromoSalesHandler(),
                new Handlers.FullXLSXUpdateImportDemandHandler(),
                new Handlers.FullXLSXUpdateImportClientShareHandler(),
                new Handlers.FullXLSXCOGSUpdateImporHandler(),
                new Handlers.FullXLSXTradeInvestmentUpdateImporHandler(),
                new Handlers.FullXLSXImportBaseLineHandler(),
				new Handlers.FullXLSXUpdateImportIncrementalPromoHandler(),
                new Handlers.UpdateUpliftHandler(),
                new Handlers.CalculatePromoParametersHandler(),
                new Handlers.CalculateBudgetsHandler(),
                new Handlers.CalculateActualParamatersHandler(),
                new Handlers.PromoWorkflowHandler(),
                new Handlers.FullXLSXImportPromoProductHandler(),
                new Handlers.FullXLSXImportPromoProductFromTLCHandler(),
                new Handlers.BaseLineUpgradeHandler(),
                new Handlers.SchedulerExportHandler(),
                new Handlers.FullXLSXUpdateBrandTechHandler(),
                new Handlers.FullXLSXUpdateAllHandler(),
                new Handlers.FullXLSXUpdateBudgetSubItemHandler(),
                new Handlers.FullXLSXNoNegoUpdateImporHandler(),
                new Handlers.FullXLSXAssortmentMatrixImportHandler(),
                new Handlers.AutoResetPromoHandler(),
				new Handlers.XLSXImportActualLsvHandler(),
                new Handlers.ActualLSVChangeHandler(),
                new Handlers.DataFlow.RecalculateAllPromoesHandler(),
                new Handlers.DataFlow.DataFlowFilteringHandler(),
                new Handlers.DataFlow.DataFlowRecalculatingHandler(),
                new Handlers.PromoPartialWorkflowHandler(),
                new Handlers.RemoveDeletedDataHandler(),
                new Handlers.FillInOutProductIdsHandler(),
                new Handlers.DataLakeIntegrationHandlers.MarsCustomersCheckHandler(),
                new Handlers.Notifications.ClientTreeNeedUpdateNotificationHandler(),
                new Handlers.UnblockPromoesHandler(),
                new Handlers.FullXLSXUpdateImportPromoProductsCorrectionHandler(),
                new Handlers.FullXLSXUpdateImportPromoProductsUpliftHandler(),
                new Handlers.PromoListPlanRecalculationHandler(),
                new Handlers.PromoListActualRecalculationHandler(),
				new Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler(),
				new Handlers.DataLakeIntegrationHandlers.MarsProductsCheckHandler()
			};
        }
    }
}
