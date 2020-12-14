using Core.Settings;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Handlers.MainNightProcessing;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class DayIncrementalQTYRecalculationHandler : BaseHandler
    {
        public string logLine = "";
        string startJobTemplate = "msdb.dbo.sp_start_job @job_name = N'{0}', @step_name = N'{1}'";
        string setProcessingFlagTemplate = "UPDATE [DefaultSchemaSetting].[JobFlag] SET [Value] = 1 WHERE [Prefix] = N'{0}'";
        string checkJobStatusTemplate = "SELECT [Value] FROM [DefaultSchemaSetting].[JobFlag] WHERE [Prefix] = N'{0}'";

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = new LogWriter(info.HandlerId.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            logLine = String.Format("The calculation day incremental QTY began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    var message = string.Empty;
                    var isProcessing = IsProcessing(context, out message);

                    if (message != string.Empty)
                    {
                        handlerLogger.Write(true, String.Format("Start calculating job was blocked due to unknown job status. Error: {0}", message), "Error");
                    }
                    else
                    {
                        if (isProcessing)
                        {
                            handlerLogger.Write(true, String.Format("Start calculating job was blocked due to job has already started"), "Error");
                        }
                        else
                        {
                            string error = StartJob(context);
                            if (error != string.Empty)
                                handlerLogger.Write(true, String.Format(error), "Error");

                            DateTimeOffset startTime = DateTimeOffset.Now;
                            isProcessing = true;
                            while (isProcessing)
                            {
                                Thread.Sleep(30000);
                                isProcessing = IsProcessing(context, out message);
                                if (message != string.Empty)
                                {
                                    handlerLogger.Write(true, String.Format("Unable to find job status"), "Error");
                                    isProcessing = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The calculation day incremental QTY was ended with errors at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
            }
            finally
            {
                sw.Stop();
                handlerLogger.Write(true, "");
                logLine = String.Format("The calculation day incremental QTY was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
                handlerLogger.Write(true, logLine, "Message");
                handlerLogger.UploadToBlob();
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                using (DatabaseContext context = new DatabaseContext())
                {
                    MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                }
            }
        }

        /// <summary>
        /// Записать ошибки в лог
        /// </summary>
        /// <param name="handlerLogger">Лог</param>
        /// <param name="errorString">Список ошибок, записанных через ';'</param>
        private void WriteErrorsInLog(ILogWriter handlerLogger, string errorString)
        {
            string[] errors = errorString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string message = "";
            foreach (string e in errors)
                message += e + "\n";

            handlerLogger.Write(true, message);
        }

        private bool IsProcessing(DatabaseContext context, out string message)
        {
            message = string.Empty;
            try
            {
                string prefix = AppSettingsManager.GetSetting<string>("CALCULATE_DAY_INCREMENTAL_QTY_PREFIX", "DayIncrementalQTYRecalculation");
                var checkJobStatusScript = String.Format(checkJobStatusTemplate, prefix);
                var status = context.SqlQuery<Byte>(checkJobStatusScript).FirstOrDefault();
                return Int32.Parse(status.ToString()) == 1 ? true : false;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        private string StartJob(DatabaseContext context)
        {
            try
            {
                SetProcessingFlag(context);

                string connectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                string database = builder.InitialCatalog;

                string jobName = $"DayIncrementalQTYRecalculation_{database}";

                string startJobScript = string.Empty;
                string startStep = AppSettingsManager.GetSetting<string>("CALCULATE_DAY_INCREMENTAL_QTY_START_STEP", "");
                startJobScript = String.Format(startJobTemplate, jobName, startStep);

                context.ExecuteSqlCommand(startJobScript);

                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void SetProcessingFlag(DatabaseContext context)
        {
            string prefix = AppSettingsManager.GetSetting<string>("CALCULATE_DAY_INCREMENTAL_QTY_PREFIX", "DayIncrementalQTYRecalculation");
            string setProcessingFlagScript = String.Format(setProcessingFlagTemplate, prefix);
            context.ExecuteSqlCommand(setProcessingFlagScript);
        }
    }
}
