﻿using Interfaces.Core.Common;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Utils;
using ProcessingHost.Handlers;
using System;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// Класс для запуска экшена по расчету значения SellOutBaselineQty после изменения записи в справочнике CoefficientSI2SO
    /// </summary>
    public class SellOutBaselineQtyCalculationHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Sell-out Baseline calculation began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                IAction action = GetAction(info);
                action.Execute();
                // Если в прцессе выполнения возникли ошибки, статус задачи устанавливаем ERROR
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Errors, "Error");
                    }
                }
                if (action.Warnings.Any())
                {
                    data.SetValue<bool>("HasWarnings", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Warnings, "Warning");
                    }
                }
                if (action.Results.Any())
                {
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Results.Keys, "Message");
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
                    handlerLogger.Write(true, String.Format("Sell-out Baseline calculation ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }

        protected IAction GetAction(HandlerInfo info)
        {
            string oldBrandTechCode = HandlerDataHelper.GetIncomingArgument<String>("oldBrandTechCode", info.Data, false);
            string oldDemandCode = HandlerDataHelper.GetIncomingArgument<String>("oldDemandCode", info.Data, false);
            string brandTechCode = HandlerDataHelper.GetIncomingArgument<String>("brandTechCode", info.Data, false);
            string demandCode = HandlerDataHelper.GetIncomingArgument<String>("demandCode", info.Data, false);
            double cValue = HandlerDataHelper.GetIncomingArgument<double>("cValue", info.Data, false);
            return new SellOutBaselineQtyCalculationAction(oldBrandTechCode, oldDemandCode, brandTechCode, demandCode, cValue);
        }
    }
}