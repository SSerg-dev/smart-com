using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Looper.Parameters;
using System.Threading;
using System.Diagnostics;
using Module.Persist.TPM.Utils.Filter;
using Core.Settings;
using Core.Dependency;
using System.Data.Entity;
using Utility.LogWriter;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers.DataFlow
{
    class RecalculateAllPromoesHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = new FileLogWriter(info.HandlerId.ToString(), new Dictionary<string, string>() { ["Timing"] = "TIMING" });
            var stopWatch = Stopwatch.StartNew();
            handlerLogger.Write(true, String.Format("The Data Flow initialization began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            handlerLogger.Write(true, "The task for filtering of promoes will be created in a few seconds.", "Message");

            var context = new DatabaseContext();
            try
            {
                var handlerData = new HandlerData();
                CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.DataFlowFiltering, handlerData, context);
                handlerLogger.Write(true, "The task for filtering of promoes was created.", "Message");
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The Data Flow initialization ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
            }
            finally
            {
                if (context != null)
                {
                    context.SaveChanges();
                    ((IDisposable)context).Dispose();
                }

                stopWatch.Stop();
                handlerLogger.Write(true, String.Format("The Data Flow initialization ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
        }
    }
}
