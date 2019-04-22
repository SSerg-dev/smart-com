﻿using ProcessingHost.Handlers;
using System;
using System.Linq;
using Looper.Core;
using Utility.LogWriter;
using System.Diagnostics;
using Interfaces.Core.Common;
using Module.Host.TPM.Actions.Notifications;

namespace Module.Host.TPM.Handlers {
    /// <summary>
    /// Класс для запуска экшена по формированияю и рассылке списка промо для которых подходит новый продукт
    /// </summary>
    public class PromoUpliftFailNotificationHandler : BaseHandler {
        public override void Action(HandlerInfo info, ExecuteData data) {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("The formation of the message began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));
                //string param = HandlerDataHelper.GetIncomingArgument<paramType>("paramName", info.Data).Value;

                IAction action = new PromoUpliftFailNotificationAction();
                action.Execute();
                // Если в прцессе выполнения возникли ошибки, статус задачи устанавливаем ERROR
                if (action.Errors.Any()) {
                    data.SetValue<bool>("HasErrors", true);
                    if (handlerLogger != null) {
                        handlerLogger.Write(true, String.Join(Environment.NewLine, action.Errors));
                    }
                }
            } catch (Exception e) {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);
                if (handlerLogger != null) {
                    handlerLogger.Write(true, e.ToString());
                }
            } finally {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();
                if (handlerLogger != null) {
                    handlerLogger.Write(true, String.Format("Newsletter notifications ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds));
                }
            }
        }
    }
}