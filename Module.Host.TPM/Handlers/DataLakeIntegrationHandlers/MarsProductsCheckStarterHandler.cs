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
using Looper;
using Module.Persist.TPM.Model.Import;
using Persist.Model;
using LoopHandler = Persist.Model.LoopHandler;

namespace Module.Host.TPM.Handlers.DataLakeIntegrationHandlers
{
    class MarsProductsCheckStarterHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = new FileLogWriter(info.HandlerId.ToString(), new Dictionary<string, string>() { ["Timing"] = "TIMING" });
            var stopWatch = Stopwatch.StartNew();
            handlerLogger.Write(true, String.Format("The checking of the Mars products initialization began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            handlerLogger.Write(true, "The task for checking of the Mars products will be created in a few seconds.", "Message");

            var context = new DatabaseContext();
            try
            {
				Guid? userId = Guid.Empty;
				Guid? roleId = Guid.Empty;
				LoopHandler currentHandler = context.LoopHandlers.Find(info.HandlerId);
				if (currentHandler != null)
				{
					userId = currentHandler.UserId;
					roleId = currentHandler.RoleId;
				}
                
				var handlerData = new HandlerData();
				var handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Synchronization materials with products.",
                    Name = "Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckHandler",
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId == Guid.Empty ? null : userId,
                    RoleId = roleId == Guid.Empty ? null : roleId
                };

                handler.SetParameterData(handlerData);
                context.LoopHandlers.Add(handler);

                context.SaveChanges();
                handlerLogger.Write(true, "The task for checking of the Mars products was created.", "Message");
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The checking of the Mars products initialization ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
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
                handlerLogger.Write(true, String.Format("The checking of the Mars products initialization ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
        }
    }
}
