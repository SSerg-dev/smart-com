using Looper.Core;

using Module.Host.TPM.Actions.Notifications;
using Module.Persist.TPM.Utils;
using ProcessingHost.Handlers;

using System;
using System.Diagnostics;
using System.Linq;

using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.Notifications
{
    public class PromoROIReportNotificationHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            var stopWatch = new Stopwatch();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("The formation of the message began at {0:yyyy-MM-dd HH:mm:ss}", 
                    ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                var action = new PromoROIReportNotificationAction();
                action.Execute();

                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);
                    handlerLogger.Write(true, action.Errors, "Error");
                }

				if (action.Warnings.Any())
				{
					data.SetValue<bool>("HasWarnings", true);
		            handlerLogger.Write(true, action.Warnings, "Warning");
				}

				if (action.Results.Any())
				{
		            handlerLogger.Write(true, action.Results.Keys, "Message");
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
                stopWatch.Stop();
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Newsletter notifications ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopWatch.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
