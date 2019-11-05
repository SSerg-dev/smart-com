using Interfaces.Core.Common;
using Looper.Core;
using Module.Host.TPM.Actions.Notifications;
using ProcessingHost.Handlers;
using System;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.Notifications
{
	/// <summary>
	/// Класс для запуска экшена по формированияю и отправке уведомлений по действию Reject промо
	/// </summary>
	public class PromoOnRejectNotificationHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("The formation of the message began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                IAction action = new PromoOnRejectNotificationAction();
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
                    handlerLogger.Write(true, String.Format("Newsletter notifications ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                }
            }
        }
    }
}
