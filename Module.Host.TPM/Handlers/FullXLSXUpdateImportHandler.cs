using Interfaces.Core.Common;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Moule.Host.TPM.Actions;
using ProcessingHost.Handlers;
using ProcessingHost.Handlers.Import;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

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
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportBrandTechAction(settings);
        }
    }

    class FullXLSXUpdateBudgetSubItemHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            return new FullXLSXUpdateImportBudgetSubItemAction(settings);
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

    class FullXLSXAssortmentMatrixImportHandler : FullXLSXImportHandler
    {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXAssortmentMatrixImportAction(settings);
        }
    }

    class FullXLSXImportPromoProductHandler : FullXLSXImportHandler
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
            return new FullXLSXImportPromoProductAction(settings, promoId, userId, roleId);
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
}
