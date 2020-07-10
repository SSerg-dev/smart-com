using Interfaces.Core.Common;
using Looper.Core;
using Module.Host.TPM.Actions;
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
    class RestartMainNightProcessingHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Restart Main Night Processing Test start at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                IAction action = new RestartMainNightProcessingActions();
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
                else if (action.Warnings.Any())
                {
                    data.SetValue<bool>("HasWarnings", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Warnings, "Warning");
                    }
                }
                else
                {
                    handlerLogger.Write(true, "Main Night Process is correct", "Message");

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
                    handlerLogger.Write(true, String.Format("Restart Main Night Processing Test ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                }

            
            }
        }
    }
}