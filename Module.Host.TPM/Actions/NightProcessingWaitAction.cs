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

namespace Module.Host.TPM.Actions
{
    class NightProcessingWaitAction : BaseAction
    {

        public override void Execute()
        {
            try
            {
                bool isNightProccesing = true;
                bool isError = false;
                DateTimeOffset startTime = DateTimeOffset.Now;
                var handlerTimeout = AppSettingsManager.GetSetting<int>("NIGHT_PROCESSING_WAIT_HANDLER_TIMEOUT", 28800000); 

                using (DatabaseContext context = new DatabaseContext())
                {
                    do
                    {
                        string error = "";
                        bool isProcessError = false;

                        isNightProccesing = IsNightProcessingInProgress(context, out error, out isProcessError);

                        if (isProcessError)
                        {
                            Errors.Add(error);
                            isError = true;
                        }

                        if (!isProcessError && error != string.Empty)
                        {
                            Errors.Add("Unable to find property of Night Processing status");
                            isError = true;
                        }

                        TimeSpan ts = DateTimeOffset.Now - startTime;
                        // Если хендлер выполняется дольше заданного таймаута, пишем ошибку
                        if (ts.TotalMilliseconds > handlerTimeout)
                        {
                            Errors.Add("Night processing timeout over");
                            isError = true;
                        }

                        if(!isError && isNightProccesing)
                        {
                            Thread.Sleep(30000);
                        }
                    }
                    while (isNightProccesing && !isError);
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while waiting night processing", e.ToString());
                Errors.Add(msg);
            }
        }        

        private bool IsNightProcessingInProgress(DatabaseContext context, out string error, out bool isProcessError)
        {
            const int NIGHT_PROCESSING_IN_PROGRESS = 1;
            const int NIGHT_PROCESSING_HAS_ERROR = 2;
            isProcessError = false;
            error = string.Empty;

            try
            {
                string nightProcessingPrefix = AppSettingsManager.GetSetting<string>("NIGHT_PROCESSING_PROGRESS_PREFIX", "NightProcessingProgress");
                var checkJobStatusScript = String.Format(Consts.Templates.checkProcessingFlagTemplate, nightProcessingPrefix);
                var status = context.SqlQuery<Byte>(checkJobStatusScript).FirstOrDefault();
                var statusCode = int.Parse(status.ToString());
                bool isInProgress;

                switch (statusCode)
                {
                    case NIGHT_PROCESSING_IN_PROGRESS:
                        {
                            isInProgress = true;
                            break;
                        }
                    case NIGHT_PROCESSING_HAS_ERROR:
                        {
                            isProcessError = true;
                            isInProgress = false;
                            error = "Error was occured while night processing";
                            break;
                        }
                    default:
                        {
                            isInProgress = false;
                            break;
                        }
                }

                return isInProgress;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }
    }
}
