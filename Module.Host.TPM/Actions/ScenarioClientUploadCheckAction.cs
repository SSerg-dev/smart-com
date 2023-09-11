using Core.Settings;
using Interfaces.Implementation.Action;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Module.Persist.TPM.Utils;
using Persist.Model;
using Looper.Parameters;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;

namespace Module.Host.TPM.Actions
{
    class ScenarioClientUploadCheckAction : BaseAction
    {

        public override void Execute()
        {
            try
            {
                var handlerTimeout = AppSettingsManager.GetSetting<int>("SCENARIO_CLIENT_UPLOADING_TIMEOUT", 3600000);
                var delta = AppSettingsManager.GetSetting<int>("SCENARIO_CLIENT_UPLOADING_TIMEOUT_DELTA", 900000);

                using (DatabaseContext context = new DatabaseContext())
                {
                    var handlerName = "Module.Host.TPM.Handlers.DLClientUploadWaitHandler";
                    var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
                    var timeoutDate = DateTime.Now.AddMilliseconds(-handlerTimeout);
                    var timeoutDateWithDelta = DateTime.Now.AddMilliseconds(-handlerTimeout - delta);
                    var tasksToCheck = context.Set<LoopHandler>().Where(x => x.Name == handlerName && x.CreateDate >= timeoutDateWithDelta && x.Status == "INPROGRESS");

                    const int SCENARIO_UPLOAD_COMPLETE = 0;
                    const int SCENARIO_UPLOAD_ERROR = 2;

                    if (tasksToCheck.Count() > 0)
                    {
                        foreach (var task in tasksToCheck)
                        {
                            if (task.CreateDate <= timeoutDate)
                            {
                                Warnings.Add($"{task.Name} finished with error. Timeout.");
                                task.Status = "ERROR";
                            }
                        }

                        IQueryable<JobFlagView> clientUploadingJobFlags = context.SqlQuery<JobFlagView>(
                            $@"SELECT Prefix, Value FROM {defaultSchema}.JobFlag WHERE Description LIKE 'Upload client%'").AsQueryable();

                        foreach (var job in clientUploadingJobFlags.Where(x => x.Value != 1))
                        {
                            var objectId = Int32.Parse(job.Prefix);
                            var clientName = context.Set<ClientTree>().First(x => x.ObjectId == objectId).Name;
                            var taskDescription = $"Wait while client {clientName} is in progress";
                            int jobFlag = job.Value;
                            var task = tasksToCheck.FirstOrDefault(x => x.Status != "ERROR" && x.Description == taskDescription);
                            if (task != null)
                            {
                                switch (jobFlag)
                                {
                                    case SCENARIO_UPLOAD_COMPLETE:
                                        task.Status = "COMPLETE";
                                        break;
                                    case SCENARIO_UPLOAD_ERROR:
                                        Warnings.Add($"Uploading {clientName} task finished with error. Return code: {SCENARIO_UPLOAD_ERROR}");
                                        task.Status = "ERROR";
                                        break;
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = $"An error occurred while scenario uploading check:{e}";
                Errors.Add(msg);
            }
        }
    }
}
