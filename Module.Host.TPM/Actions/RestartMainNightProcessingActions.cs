using Core.Dependency;
using Core.Settings;
using DocumentFormat.OpenXml.Drawing.Charts;
using Interfaces.Implementation.Action;
using Looper;
using Module.Persist.TPM.Utils;
using Nest;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    class RestartMainNightProcessingActions : BaseAction
    {
        public override void Execute()
        {
            try
            {
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                int timetoRestart = settingsManager.GetSetting<int>("TIME_TO_RESTART_MAIN_NIGHT_PROCESS", 172800000); 
                using (DatabaseContext context = new DatabaseContext())
                {
                    var nightHandler = context.LoopHandlers.Where(e => e.Name.Equals("Module.Host.TPM.Handlers.NightProcessingMainHandler") && e.ExecutionPeriod == 86400000).FirstOrDefault();
                    if(nightHandler == null)
                    {
                        Errors.Add("Handler Module.Host.TPM.Handlers.NightProcessingMainHandler not found");
                        return;
                    }
                    var currentDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    var processHandlerDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(nightHandler.NextExecutionDate);

                    var diffDate = (currentDate - processHandlerDate).TotalMilliseconds;
                    if(diffDate > timetoRestart)
                    {
                        RestartMainNightProcessing(nightHandler,currentDate);
                        Warnings.Add(String.Format("Main night handler does not work, the process will be assigned to {0}",nightHandler.NextExecutionDate.ToString()));
                        context.SaveChanges();
                    }
                }

                }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while inserting: {0}", e.ToString());
                Errors.Add(msg);
            }
        }

        private void RestartMainNightProcessing(global::Persist.Model.LoopHandler nightHandler, DateTimeOffset currentDate)
        {
            var newExecutionDate = currentDate.AddDays(1);
            newExecutionDate = new DateTime(newExecutionDate.Year, newExecutionDate.Month, newExecutionDate.Day, nightHandler.NextExecutionDate.Value.Hour, nightHandler.NextExecutionDate.Value.Minute, 0);
            nightHandler.Status = "WAITING";
            nightHandler.NextExecutionDate = newExecutionDate;
        }
    }
}
