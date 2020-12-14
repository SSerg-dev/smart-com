using Core.Settings;
using Interfaces.Core.Common;
using Looper.Core;
using Module.Host.TPM.Actions;
using Module.Host.TPM.Actions.Notifications;
using Module.Host.TPM.Handlers.MainNightProcessing;
using Module.Persist.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    public class RollingVolumeQTYRecalculationHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Rolling promo collecting started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                IAction action = new RollingVolumeQTYRecalculationActions();
                action.Execute();
                // Если в процессе выполнения возникли ошибки, статус задачи устанавливаем ERROR
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
                    handlerLogger.Write(true, String.Format("Rolling promo collecting ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                }
                handlerLogger.UploadToBlob();
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                using (DatabaseContext context = new DatabaseContext())
                {
                    MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                }
            }
        }
    }
}
