using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
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
using Module.Persist.TPM.Utils;
using Module.Host.TPM.Handlers.MainNightProcessing;

namespace Module.Host.TPM.Handlers
{
    public class StartSISOBaselineCalculationHandler : BaseHandler
    {
        public string logLine = "";

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = new LogWriter(info.HandlerId.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            logLine = String.Format("The calculation sell-in and sell-out baseline began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");

            DatabaseContext context = new DatabaseContext();

            try
            {
                bool? startSecondStep = HandlerDataHelper.GetIncomingArgument<bool>("startSecondStep", info.Data, false);
                
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
                        // ставим таймаут расчета baseline 
                        int timeout = AppSettingsManager.GetSetting<int>("BASELINE_СALCULATION_TIMEOUT", 1800000);

                        string error = StartJob(context, startSecondStep);
                        if (error != string.Empty)
                            handlerLogger.Write(true, String.Format(error), "Error");

                        DateTimeOffset startTime = DateTimeOffset.Now;
                        isProcessing = true;
                        while (isProcessing)
                        {
                            TimeSpan ts = DateTimeOffset.Now - startTime;
                            // Если процедура длится дольше заданного таймаута, пишем ошибку
                            if (ts.TotalMilliseconds > timeout)
                            {
                                handlerLogger.Write(true, String.Format("Calculation timeout"), "Error");
                                isProcessing = false;
                            }
                            else
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

                handlerLogger.Write(true, String.Format("The calculation sell-in and sell-out baseline was ended with errors at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
            }
            finally
            {
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);

                if (context != null)
                {
                    ((IDisposable)context).Dispose();
                }

                sw.Stop();
                handlerLogger.Write(true, "");
                logLine = String.Format("The calculation sell-in and sell-out baseline was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
                handlerLogger.Write(true, logLine, "Message");
                handlerLogger.UploadToBlob();
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
                string baselineCalculatingPrefix = AppSettingsManager.GetSetting<string>("CALCULATE_BASELINE_PREFIX", "SISOBaselineCalculating");
                var checkJobStatusScript = String.Format(Consts.Templates.checkProcessingFlagTemplate, baselineCalculatingPrefix);
                var status = context.SqlQuery<Byte>(checkJobStatusScript).FirstOrDefault();
                return Int32.Parse(status.ToString()) == 1 ? true : false;
            }
            catch(Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        private string StartJob(DatabaseContext context, bool? startSecondStep)
        {
            try
            {
                string baselineCalculatingPrefix = AppSettingsManager.GetSetting<string>("CALCULATE_BASELINE_PREFIX", "SISOBaselineCalculating");
                MainNightProcessingHelper.SetProcessingFlagUp(context, baselineCalculatingPrefix);

                string connectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                string database = builder.InitialCatalog;

                string calculateBaselineJobName = AppSettingsManager.GetSetting<string>("CALCULATE_BASELINE_JOB_NAME", "CalculateBaselineValues");
                calculateBaselineJobName += $"_{database}";
                string calculateBaselineSecondStepName = null;
                if (startSecondStep.HasValue && startSecondStep.Value)
                {
                    calculateBaselineSecondStepName = AppSettingsManager.GetSetting<string>("CALCULATE_BASELINE_SECOND_STEP_NAME", "CalculateSellOutBaselineValues");
                }

                string startJobScript = String.Format(Consts.Templates.startJobTemplate, calculateBaselineJobName, calculateBaselineSecondStepName);
                context.ExecuteSqlCommand(startJobScript);

                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
