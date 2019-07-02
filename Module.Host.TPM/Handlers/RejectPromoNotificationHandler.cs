﻿using Interfaces.Core.Common;
using Looper.Core;
using Module.Host.TPM.Actions.Notifications;
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
    /// <summary>
    /// Класс для запуска экшена по формированияю и отправке уведомлений по Reject Promo
    /// </summary>
    public class RejectPromoNotificationHandler : BaseHandler
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

                IAction action = new RejectPromoNotificationAction();
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
