﻿using Interfaces.Core.Common;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Model.Interfaces;
using Moule.Host.TPM.Actions;
using ProcessingHost.Handlers.Import;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            try
            {
                logger.Debug("Begin '{0}'", info.HandlerId);
                handlerLogger = new LogWriter(info.HandlerId.ToString());

                sw.Start();
                handlerLogger.Write(true, String.Format("Import started in {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                InitializeParameters(info.Data, data);
                WriteParametersToLog(handlerLogger.CurrentLogWriter, data);

                string separator = HandlerDataHelper.GetIncomingArgument<string>("Separator", info.Data, false) ?? ";";
                string quote = HandlerDataHelper.GetIncomingArgument<string>("QuoteChar", info.Data, false) ?? "";
                bool hasHeader = HandlerDataHelper.GetIncomingArgument<bool?>("HasHeader", info.Data, false) ?? true;
                // Импортируемая модель
                string importSubtypeName = HandlerDataHelper.GetIncomingArgument<string>("ImportSubtypeName", info.Data, false);

                // Импортируемая модель
                Type importType = HandlerDataHelper.GetIncomingArgument<Type>("ImportType", info.Data);
                Type modelType = HandlerDataHelper.GetIncomingArgument<Type>("ModelType", info.Data);
                // Название файла импорта
                FileModel importFile = HandlerDataHelper.GetIncomingArgument<FileModel>("File", info.Data);
                // Параметры импорта
                // ИД пользователя и текущей роли
                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data);
                string telemetrickName = importType.Name;

                FullImportSettings settings = new FullImportSettings()
                {
                    UserId = userId,
                    RoleId = roleId,
                    HasHeader = hasHeader,
                    Quote = quote,
                    Separator = separator,
                    ImportFile = importFile,
                    ImportSubtypeName = importSubtypeName,
                    ImportType = importType,
                    ModelType = modelType
                };

                IAction action = GetAction(settings, data);
                action.Execute();

                if (action.Results.Any())
                {
                    if (handlerLogger != null)
                    {
                        foreach (var result in action.Results.Where(x => x.Key.Contains("Promo")))
                        {
                            handlerLogger.Write(true, result.Key, "Message");
                        }
                    }
                }

                // TODO Обработка ошибок
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);

                    if (handlerLogger != null)
                    {
                        // Записать в лог возникшие ошибки.
                        foreach (var error in action.Errors)
                        {
                            handlerLogger.Write(true, error, "Error");
                        }
                    }
                }

                action.SaveResultToData<int>(info.Data, "ImportSourceRecordCount");
                action.SaveResultToData<int>(info.Data, "ImportResultRecordCount");
                HandlerDataHelper.SaveOutcomingArgument<int>("ErrorCount", action.GetResult<int>("ErrorCount", 0) + action.Errors.Count, info.Data, true, false);
                action.SaveResultToData<int>(info.Data, "WarningCount");
                action.SaveResultToData<ImportResultFilesModel>(info.Data, "ImportResultFilesModel");

                string resultStatus = action.GetResult<string>("ImportResultStatus", null);
                data.SetValue<string>("ImportResultStatus", resultStatus);
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
                    handlerLogger.Write(true, String.Format("Import completed in {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }

    class FullXLSXCOGSTnUpdateImportHandler : FullXLSXImportHandler
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
            return new FullXLSXCOGSTnUpdateImportAction(settings, year, importDestination);
        }
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            try
            {
                logger.Debug("Begin '{0}'", info.HandlerId);
                handlerLogger = new LogWriter(info.HandlerId.ToString());

                sw.Start();
                handlerLogger.Write(true, String.Format("Import started in {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                InitializeParameters(info.Data, data);
                WriteParametersToLog(handlerLogger.CurrentLogWriter, data);

                string separator = HandlerDataHelper.GetIncomingArgument<string>("Separator", info.Data, false) ?? ";";
                string quote = HandlerDataHelper.GetIncomingArgument<string>("QuoteChar", info.Data, false) ?? "";
                bool hasHeader = HandlerDataHelper.GetIncomingArgument<bool?>("HasHeader", info.Data, false) ?? true;
                // Импортируемая модель
                string importSubtypeName = HandlerDataHelper.GetIncomingArgument<string>("ImportSubtypeName", info.Data, false);

                // Импортируемая модель
                Type importType = HandlerDataHelper.GetIncomingArgument<Type>("ImportType", info.Data);
                Type modelType = HandlerDataHelper.GetIncomingArgument<Type>("ModelType", info.Data);
                // Название файла импорта
                FileModel importFile = HandlerDataHelper.GetIncomingArgument<FileModel>("File", info.Data);
                // Параметры импорта
                // ИД пользователя и текущей роли
                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data);
                string telemetrickName = importType.Name;

                FullImportSettings settings = new FullImportSettings()
                {
                    UserId = userId,
                    RoleId = roleId,
                    HasHeader = hasHeader,
                    Quote = quote,
                    Separator = separator,
                    ImportFile = importFile,
                    ImportSubtypeName = importSubtypeName,
                    ImportType = importType,
                    ModelType = modelType
                };

                IAction action = GetAction(settings, data);
                action.Execute();

                if (action.Results.Any())
                {
                    if (handlerLogger != null)
                    {
                        foreach (var result in action.Results.Where(x => x.Key.Contains("Promo")))
                        {
                            handlerLogger.Write(true, result.Key, "Message");
                        }
                    }
                }

                // TODO Обработка ошибок
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);

                    if (handlerLogger != null)
                    {
                        // Записать в лог возникшие ошибки.
                        foreach (var error in action.Errors)
                        {
                            handlerLogger.Write(true, error, "Error");
                        }
                    }
                }

                action.SaveResultToData<int>(info.Data, "ImportSourceRecordCount");
                action.SaveResultToData<int>(info.Data, "ImportResultRecordCount");
                HandlerDataHelper.SaveOutcomingArgument<int>("ErrorCount", action.GetResult<int>("ErrorCount", 0) + action.Errors.Count, info.Data, true, false);
                action.SaveResultToData<int>(info.Data, "WarningCount");
                action.SaveResultToData<ImportResultFilesModel>(info.Data, "ImportResultFilesModel");

                string resultStatus = action.GetResult<string>("ImportResultStatus", null);
                data.SetValue<string>("ImportResultStatus", resultStatus);
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
                    handlerLogger.Write(true, String.Format("Import completed in {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }

    class FullXLSXPPEUpdateImportHandler : FullXLSXImportHandler
    {
        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data)
        {
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXPPEUpdateImportAction(settings);
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
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            try
            {
                logger.Debug("Begin '{0}'", info.HandlerId);
                handlerLogger = new LogWriter(info.HandlerId.ToString());

                sw.Start();
                handlerLogger.Write(true, String.Format("Import started in {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                InitializeParameters(info.Data, data);
                WriteParametersToLog(handlerLogger.CurrentLogWriter, data);

                string separator = HandlerDataHelper.GetIncomingArgument<string>("Separator", info.Data, false) ?? ";";
                string quote = HandlerDataHelper.GetIncomingArgument<string>("QuoteChar", info.Data, false) ?? "";
                bool hasHeader = HandlerDataHelper.GetIncomingArgument<bool?>("HasHeader", info.Data, false) ?? true;
                // Импортируемая модель
                string importSubtypeName = HandlerDataHelper.GetIncomingArgument<string>("ImportSubtypeName", info.Data, false);

                // Импортируемая модель
                Type importType = HandlerDataHelper.GetIncomingArgument<Type>("ImportType", info.Data);
                Type modelType = HandlerDataHelper.GetIncomingArgument<Type>("ModelType", info.Data);
                // Название файла импорта
                FileModel importFile = HandlerDataHelper.GetIncomingArgument<FileModel>("File", info.Data);
                // Параметры импорта
                // ИД пользователя и текущей роли
                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data);
                string telemetrickName = importType.Name;

                FullImportSettings settings = new FullImportSettings()
                {
                    UserId = userId,
                    RoleId = roleId,
                    HasHeader = hasHeader,
                    Quote = quote,
                    Separator = separator,
                    ImportFile = importFile,
                    ImportSubtypeName = importSubtypeName,
                    ImportType = importType,
                    ModelType = modelType
                };

                IAction action = GetAction(settings, data);
                action.Execute();

                if (action.Results.Any())
                {
                    if (handlerLogger != null)
                    {
                        foreach (var result in action.Results.Where(x => x.Key.Contains("Promo")))
                        {
                            handlerLogger.Write(true, result.Key, "Message");
                        }
                    }
                }

                // TODO Обработка ошибок
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);

                    if (handlerLogger != null)
                    {
                        // Записать в лог возникшие ошибки.
                        foreach (var error in action.Errors)
                        {
                            handlerLogger.Write(true, error, "Error");
                        }
                    }
                }

                action.SaveResultToData<int>(info.Data, "ImportSourceRecordCount");
                action.SaveResultToData<int>(info.Data, "ImportResultRecordCount");
                HandlerDataHelper.SaveOutcomingArgument<int>("ErrorCount", action.GetResult<int>("ErrorCount", 0) + action.Errors.Count, info.Data, true, false);
                action.SaveResultToData<int>(info.Data, "WarningCount");
                action.SaveResultToData<ImportResultFilesModel>(info.Data, "ImportResultFilesModel");

                string resultStatus = action.GetResult<string>("ImportResultStatus", null);
                data.SetValue<string>("ImportResultStatus", resultStatus);
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
                    handlerLogger.Write(true, String.Format("Import completed in {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
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
        private TPMmode tPMmode;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            tPMmode = HandlerDataHelper.GetIncomingArgument<TPMmode>("TPMmode", info.Data, throwIfNotExists: false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductsCorrectionAction(settings, userId, handlerId, tPMmode);
        }
    }

    class FullXLSXUpdateImportPromoProductCorrectionPriceIncreaseHandler : FullXLSXImportHandler
    {
        private Guid userId;
        private Guid handlerId;
        private TPMmode tPMmode;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            tPMmode = HandlerDataHelper.GetIncomingArgument<TPMmode>("TPMmode", info.Data, throwIfNotExists: false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductCorrectionPriceIncreaseAction(settings, userId, handlerId, tPMmode);
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
        private TPMmode TPMmode;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            promoId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            TPMmode = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<TPMmode>("TPMmode", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            TempId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<string>("TempId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductsUpliftAction(settings, promoId, userId, TempId, TPMmode);
        }
    }

    class FullXLSXUpdateImportPromoProductsPriceIncreaseUpliftHandler : FullXLSXImportHandler
    {
        /// <summary>
        /// Id промо для которого загружаются ProductUplift-ы
        /// </summary>
        private Guid promoId;
        private Guid userId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            promoId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportPromoProductPriceIncreasesUpliftAction(settings, promoId, userId);
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
    public class FullXLSXRPAEventImportHandler : FullXLSXImportHandler
    { 
        private Guid rpaId;
        //private Guid roleId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPAEventImportAction(settings, rpaId);
        }
    }

    public class FullXLSXRPAPromoSupportImportHandler: FullXLSXImportHandler
    {
        private Guid rpaId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPAPromoSupportImportAction(settings, rpaId);
        }
    }

    public class FullXLSXRPANonPromoSupportImportHandler : FullXLSXImportHandler
    {
        private Guid rpaId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPANonPromoSupportImportAction(settings, rpaId);
        }
    }

    public class FullXLSXRPAActualEANPCImportHandler: FullXLSXImportHandler
    {
        private Guid rpaId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRPAActualEanPcImportAction(settings, rpaId);
        }
    }

    public class FullXLSXRpaActualPluImportHandler: FullXLSXImportHandler
    {
        private Guid rpaId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRpaActualPluImportAction(settings, rpaId);
        }
    }
    public class FullXLSXRpaTCLclosedImportHandler : FullXLSXImportHandler
    {
        private Guid rpaId;
        private Guid handlerId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRpaTCLclosedImportAction(settings, rpaId, handlerId);
        }
    }
    public class FullXLSXRpaTCLdraftImportHandler : FullXLSXImportHandler
    {
        private Guid rpaId;
        private Guid handlerId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRpaTCLdraftImportAction(settings, rpaId, handlerId);
        }
    }
    public class FullXLSXRpaActualShelfPriceImportHandler : FullXLSXImportHandler
    {
        private Guid rpaId;
        private Guid handlerId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            rpaId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RPAId", info.Data, false);
            handlerId = info.HandlerId;
            base.Action(info, data);
        }
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXRpaActualShelfPriceImportAction(settings, rpaId, handlerId);
        }
    }
    class FullXLSXUpdateImportEventHandler : FullXLSXImportHandler
    {
        private Guid RoleId;
        private Guid userId;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            RoleId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            userId = Looper.Parameters.HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
            base.Action(info, data);
        }

        protected override IAction GetAction(FullImportSettings settings, ExecuteData data)
        {
            return new FullXLSXUpdateImportEventAction(settings, RoleId, userId);
        }
    }
}
