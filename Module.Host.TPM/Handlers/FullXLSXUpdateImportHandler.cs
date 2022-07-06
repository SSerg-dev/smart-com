using Interfaces.Core.Common;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Moule.Host.TPM.Actions;
using ProcessingHost.Handlers.Import;
using System;
using System.Collections.Generic;

namespace Module.Host.TPM.Handlers
{
    class FullXLSXUpdateImportPromoSalesHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoSalesAction(settings);
        }
    }

    class FullXLSXUpdateImportDemandHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportDemandAction(settings);
        }
    }

    class FullXLSXUpdateImportClientShareHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportClientShareAction(settings);
        }
    }

    class FullXLSXCOGSUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var year = Int32.Parse(HandlerDataHelper.GetIncomingArgument<string>("CrossParam.Year", handlerData));
            var importDestination = HandlerDataHelper.GetIncomingArgument<string>("ImportDestination", handlerData);
            data.SetValue("Year", year);
            data.SetValue("ImportDestination", importDestination);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            var year = data.GetValue<int>("Year");
            var importDestination = data.GetValue<string>("ImportDestination");
            return new FullXLSXCOGSUpdateImportAction(settings, year, importDestination);
        }
    }

    class FullXLSXRATIShopperUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var importDestination = HandlerDataHelper.GetIncomingArgument<string>("ImportDestination", handlerData);
            data.SetValue("ImportDestination", importDestination);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            var importDestination = data.GetValue<string>("ImportDestination");
            return new FullXLSXRATIShopperUpdateImportAction(settings, importDestination);
        }
    }

    class FullXLSXCompetitorBrandTechUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXCompetitorBrandTechUpdateImportAction(settings);
        }
    }

    class FullXLSXCompetitorUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXCompetitorUpdateImportAction(settings);
        }
    }

    class FullXLSXCompetitorPromoUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var importDestination = HandlerDataHelper.GetIncomingArgument<string>("ImportDestination", handlerData);
            data.SetValue("ImportDestination", importDestination);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXCompetitorPromoUpdateImportAction(settings);
        }
    }

    class FullXLSXCompetitorPromoUpdateNewImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var importDestination = HandlerDataHelper.GetIncomingArgument<string>("ImportDestination", handlerData);
            data.SetValue("ImportDestination", importDestination);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXCompetitorPromoUpdateNewImportAction(settings);
        }
    }

    class FullXLSXTradeInvestmentUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var year = Int32.Parse(HandlerDataHelper.GetIncomingArgument<string>("CrossParam.Year", handlerData));
            var importDestination = HandlerDataHelper.GetIncomingArgument<string>("ImportDestination", handlerData);
            data.SetValue("Year", year);
            data.SetValue("ImportDestination", importDestination);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            var year = data.GetValue<int>("Year");
            var importDestination = data.GetValue<string>("ImportDestination");
            return new FullXLSXTradeInvestmentUpdateImportAction(settings, year, importDestination);
        }
    }

    class FullXLSXUpdateBrandTechHandler : FullXLSXImportHandler {

        private Guid roleId;
        private Guid userId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            roleId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportBrandTechAction(settings, userId, roleId);
        }
    }

    class FullXLSXUpdateBudgetSubItemHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportBudgetSubItemAction(settings);
        }
    }

    class FullXLSXUpdateNonPromoEquipmentHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportNonPromoEquipmentAction(settings);
        }
    }

    class FullXLSXImportNonPromoDMPHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var planQuantity = Int32.Parse(HandlerDataHelper.GetIncomingArgument<string>("PlanQuantity", handlerData));
            var nonPromoSupportId = Guid.Parse(HandlerDataHelper.GetIncomingArgument<string>("NonPromoSupportId", handlerData));
            data.SetValue("PlanQuantity", planQuantity);
            data.SetValue("NonPromoSupportId", nonPromoSupportId);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {

            var planQuantity = data.GetValue<int>("PlanQuantity");
            var nonPromoSupportId = data.GetValue<Guid>("NonPromoSupportId");
            return new FullXLSXImportNonPromoDMPAction(settings, planQuantity, nonPromoSupportId);
        }
    }

    class FullXLSXImportPromoDMPHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
            var planQuantity = Int32.Parse(HandlerDataHelper.GetIncomingArgument<string>("PlanQuantity", handlerData));
            var promoSupportId = Guid.Parse(HandlerDataHelper.GetIncomingArgument<string>("PromoSupportId", handlerData));
            data.SetValue("PlanQuantity", planQuantity);
            data.SetValue("PromoSupportId", promoSupportId);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {

            var planQuantity = data.GetValue<int>("PlanQuantity");
            var promoSupportId = data.GetValue<Guid>("PromoSupportId");
            return new FullXLSXImportPromoDMPAction(settings, planQuantity, promoSupportId);
        }
    }

    class FullXLSXUpdateAllHandler : FullXLSXImportHandler {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data) {
            List<String> ufs = HandlerDataHelper.GetIncomingArgument<List<String>>("UniqueFields", handlerData);
            data.SetValue("uniqueFields", ufs);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateByPropertyImportAction(settings, settings.ModelType, data.GetValue<List<String>>("uniqueFields"));
        }
    }

    class FullXLSXUpdateCoefficientSI2SOHandler : FullXLSXImportHandler 
    {
        private Guid handlerId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            handlerId = info.HandlerId;
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportCoefficientSI2SOAction(settings, handlerId);
        }
    }

    class FullXLSXUpdateTechnologyHandler : FullXLSXImportHandler 
    {
        private Guid handlerId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            handlerId = info.HandlerId;
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportTechnologyAction(settings, handlerId);
        }
    }

    class FullXLSXUpdateProductHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportProductAction(settings);
        }
    }

    class FullXLSXNoNegoUpdateImporHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXNoNegoUpdateImportAction(settings);
        }
    }

    class FullXLSXMechanicTypeUpdateImporHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXMechanicTypeUpdateImportAction(settings);
        }
    }

    class FullXLSXAssortmentMatrixImportHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXAssortmentMatrixImportAction(settings);
        }
    }

    class FullXLSXPLUDictionaryImportHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXPLUDictionaryImportAction(settings);
        }
    }

    class FullXLSXImportPromoProductHandler : FullXLSXImportHandler
    {
        /// <summary>
        /// Id промо для которого загружается PromoProducts
        /// </summary>
        protected Guid promoId;
        protected Guid userId;
        protected Guid roleId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            promoId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            roleId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXImportPromoProductAction(settings, promoId, userId, roleId);
        }
    }

    class FullXLSXImportPromoProductPluHandler : FullXLSXImportPromoProductHandler
	{
		protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
		{
            return new FullXLSXImportPromoProductPluAction(settings, promoId, userId, roleId);

        }
	}

    class FullXLSXImportPromoProductFromTLCHandler : FullXLSXImportHandler
    {
        /// <summary>
        /// Id промо для которого загружается PromoProducts
        /// </summary>
        private Guid promoId;
        private Guid userId;
        private Guid roleId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            promoId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            roleId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXImportPromoProductFromTLCAction(settings, promoId, userId, roleId);
        }
    }

    class XLSXImportActualLsvHandler : FullXLSXImportHandler
    {
        private Guid handlerId;
        private Guid userId;
        private Guid roleId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            handlerId = info.HandlerId;
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            roleId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new XLSXImportActualLsvAction(settings, handlerId, userId, roleId);
        }
    }

    class FullXLSXUpdateImportPromoProductsCorrectionHandler : FullXLSXImportHandler
    {
        private Guid userId;
        private Guid handlerId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductsCorrectionAction(settings, userId, handlerId);
        }
    }

    class FullXLSXUpdateImportPromoProductsUpliftHandler : FullXLSXImportHandler
    {
        /// <summary>
        /// Id промо для которого загружаются ProductUplift-ы
        /// </summary>
        private Guid promoId;
        private Guid userId;
        private string TempId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            promoId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            TempId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<string>("TempId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductsUpliftAction(settings, promoId, userId, TempId);
        }
    }

    public class FullXLSXUpdateImportClientDashboardHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportClientDashboardAction(settings);
        }
    }
    public class FullXLSXUpdateImportRollingVolumesHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportRollingVolumesAction(settings);
        }
    }

    public class FullXLSXRPAPromoSupportImportHandler: FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPAPromoSupportImportAction(settings);
        }
    }

    public class FullXLSXRPANonPromoSupportImportHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPANonPromoSupportImportAction(settings);
        }
    }

    public class FullXLSXRPAActualEANPCImportHandler: FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPAActualEanPcImportAction(settings);
        }
    }

    public class FullXLSXRpaActualPluImportHandler: FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRpaActualPluImportAction(settings);
        }
    }
}
