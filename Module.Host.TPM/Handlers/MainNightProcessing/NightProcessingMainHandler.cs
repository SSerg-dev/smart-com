using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using _Interface = Persist.Model.Interface;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.LogWriter;
using System.Data.SqlClient;
using Module.Host.TPM.Handlers.MainNightProcessing;

namespace Module.Host.TPM.Handlers
{
    public class NightProcessingMainHandler : BaseHandler
    {
        public string logLine = "";

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = new FileLogWriter(info.HandlerId.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            logLine = String.Format("Main night processing began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    string incomingInterfaceName = AppSettingsManager.GetSetting<string>("INCOMING_BASELINE_APOLLO_INTERFACE_NAME", "BASELINE_APOLLO");
                    string outcomingInterfaceName = AppSettingsManager.GetSetting<string>("OUTCOMING_INCREMENTAL_TO_APOLLO_INTERFACE_NAME", "INCREMENTAL_TO_APOLLO");
                    _Interface.Interface incomingInterface = context.Set<_Interface.Interface>().Where(x => x.Name == incomingInterfaceName).FirstOrDefault();
                    _Interface.Interface outcomingInterface = context.Set<_Interface.Interface>().Where(x => x.Name == outcomingInterfaceName).FirstOrDefault();

                    // значение по умолчанию 5 часов
                    int inputBaseLineProcessHandlerTimeout = AppSettingsManager.GetSetting<int>("INPUT_BASELINE_PROCESS_HANDLER_TIMEOUT", 18000000);
                    // значение по умолчанию 1.5 часа
                    int startSISOBaselineCalculationHandlerTimeout = AppSettingsManager.GetSetting<int>("START_SISO_BASELINE_CALCULATION_HANDLER_TIMEOUT", 5400000);
                    // значение по умолчанию 12 часов
                    int recalculateAllPromoesHandlerTimeout = AppSettingsManager.GetSetting<int>("RECALCULATE_ALL_PROMOES_HANDLER_TIMEOUT", 43200000);
                    // значение по умолчанию 4 часа
                    int dayIncrementalQTYRecalculationHandlerTimeout = AppSettingsManager.GetSetting<int>("DAY_INCREMENTAL_QTY_RECALCULATION_HANDLER_TIMEOUT", 14400000);
                    // значение по умолчанию 0.5 часа
                    int rollingVolumeQTYRecalculationHandlerTimeout = AppSettingsManager.GetSetting<int>("ROLLING_VOLUME_QTY_RECALCULATION_HANDLER_TIMEOUT", 1800000);
                    // значение по умолчанию 0.5 часa
                    int outputIncrementalProcessHandlerTimeout = AppSettingsManager.GetSetting<int>("OUTPUT_INCREMENTAL_PROCESS_HANDLER_TIMEOUT", 1800000);

                    string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");

                    string allMainNightProcessingStepPrefixs = AppSettingsManager.GetSetting<string>("ALL_MAIN_NIGHT_PROCESSING_STEP_PREFIXS", "'MainNightProcessingStep','SISOBaselineCalculating','DayIncrementalQTYRecalculation'");
                    string[] allPrefxis = allMainNightProcessingStepPrefixs.Split(',');
                    MainNightProcessingHelper.SetAllFlagsDown(context, allPrefxis);

                    // разбор входящего файла
                    LoopHandler handler = CreateSingleLoopHandler(
                        "Incoming baseline processing",
                        "Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler"
                        );
                   
                    handler.SetParameterData(GetProcessorHandlerData(incomingInterface));
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                   
                    MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                    HandlerWaiting(context, handler, inputBaseLineProcessHandlerTimeout, handlerLogger);
                   
                    // расчет sell-in и sell-out Baseline
                    HandlerData handlerData = handler.GetParameterData();
                    int fileCount = HandlerDataHelper.GetOutcomingArgument<int>("fileCount", handlerData, false);
                    handler = GetNextHandler(context, fileCount);
                    if (handler != null)
                    {
                        context.LoopHandlers.Add(handler);
                        context.SaveChanges();
                    
                        MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                        HandlerWaiting(context, handler, startSISOBaselineCalculationHandlerTimeout, handlerLogger);
                    }
                    
                    // пересчет промо
                    handler = CreateSingleLoopHandler(
                        "Promo recalculation in dataflow process",
                        "Module.Host.TPM.Handlers.DataFlow.RecalculateAllPromoesHandler"
                        );
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                    
                    MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                    HandlerWaiting(context, handler, recalculateAllPromoesHandlerTimeout, handlerLogger);
                    
                    // хендлер для формирования CurrentDayIncremental, Difference и PreviousDayIncremental
                    handler = CreateSingleLoopHandler(
                        "Day Incremental QTY recalculation",
                        "Module.Host.TPM.Handlers.DayIncrementalQTYRecalculationHandler"
                        );
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                    
                    MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                    HandlerWaiting(context, handler, dayIncrementalQTYRecalculationHandlerTimeout, handlerLogger);
                    
                    // хендлер для формирования Difference из rolling volume
                    var currentDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    if (currentDate.DayOfWeek == ((DayOfWeek)DayOfWeek()) || DayOfWeek() > 6)
                    {
                        handler = CreateSingleLoopHandler(
                        "Rolling promo collecting",
                        "Module.Host.TPM.Handlers.RollingVolumeQTYRecalculationHandler"
                        );
                        context.LoopHandlers.Add(handler);
                        context.SaveChanges();

                        MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                        HandlerWaiting(context, handler, rollingVolumeQTYRecalculationHandlerTimeout, handlerLogger);
                    }

                    // формирование исходящего файла
                    handler = CreateSingleLoopHandler(
                        "Outcoming incremental processing",
                        "Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler"
                        );

                    handler.SetParameterData(GetProcessorHandlerData(outcomingInterface));
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();

                    MainNightProcessingHelper.SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                    HandlerWaiting(context, handler, outputIncrementalProcessHandlerTimeout, handlerLogger);
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("Main night processing was ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
            }
            finally
            {
                sw.Stop();
                handlerLogger.Write(true, "");
                logLine = String.Format("Main night processing was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds);
                handlerLogger.Write(true, logLine, "Message");
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

        /// <summary>
        /// Создание следующей отложенной задачи (для расчета SI и(или) SO Baseline)
        /// </summary>
        /// <param name="promo"></param>
        private LoopHandler GetNextHandler(DatabaseContext context, int fileCount)
        {
            LoopHandler handler = null;
            var coefficients = context.Set<CoefficientSI2SO>().Where(x => !x.Disabled && x.NeedProcessing);

            // если были входящие файлы, надо пересчитать Baseline
            if (fileCount > 0)
            {
                handler = CreateSingleLoopHandler(
                    "Start job to calculate sell-in and sell-out baseline values",
                    "Module.Host.TPM.Handlers.StartSISOBaselineCalculationHandler"
                    );
            }
            // если менялись коэффициенты, то надо пересчитать только SO Baseline
            else if (coefficients.Any())
            {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("startSecondStep", true, data, visible: false, throwIfNotExists: false);

                handler = CreateSingleLoopHandler(
                    "Start job to calculate sell-in and sell-out baseline values",
                    "Module.Host.TPM.Handlers.StartSISOBaselineCalculationHandler"
                    );
                handler.SetParameterData(data);
            }

            return handler;
        }

        private LoopHandler CreateSingleLoopHandler(string description, string name)
        {
            return new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = description,
                Name = name,
                ExecutionPeriod = null,
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = null,
                RoleId = null
            };
        }

        private static HandlerData GetProcessorHandlerData(_Interface.Interface @interface)
        {
            HandlerData data = new HandlerData();
            InterfaceFileListModel viewFiles = new InterfaceFileListModel()
            {
                FilterType = "INTERFACE"
            };
            HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, data, true, false);
            HandlerDataHelper.SaveIncomingArgument<Guid?>("UserId", null, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid>("InterfaceId", @interface.Id, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<string>("InterfaceName", @interface.Name, data, true, false);
            return data;
        }

        private void HandlerWaiting(DatabaseContext context, LoopHandler handler, int handlerTimout, ILogWriter handlerLogger)
        {
            DateTimeOffset startTime = DateTimeOffset.Now;
            var isHandlerInProgress = true;
            while (isHandlerInProgress)
            {
                TimeSpan ts = DateTimeOffset.Now - startTime;
                // Если хендлер выполняется дольше заданного таймаута, пишем ошибку
                if (ts.TotalMilliseconds > handlerTimout)
                {
                    handlerLogger.Write(true, String.Format("{0} timeout", handler.Name), "Error");
                    isHandlerInProgress = false;
                }
                else
                {
                    Thread.Sleep(30000);
                    string message;
                    isHandlerInProgress = IsHandlerInProgress(context, out message);
                    if (message != string.Empty)
                    {
                        handlerLogger.Write(true, String.Format("Unable to find {0} status", handler.Name), "Error");
                        isHandlerInProgress = false;
                    }
                }
            }
        }

        private bool IsHandlerInProgress(DatabaseContext context, out string message)
        {
            message = string.Empty;
            try
            {
                string mainNightProcessingPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_PREFIX", "MainNightProcessingStep");
                var checkJobStatusScript = String.Format(Consts.Templates.checkProcessingFlagTemplate, mainNightProcessingPrefix);
                var status = context.Database.SqlQuery<Byte>(checkJobStatusScript).FirstOrDefault();
                return Int32.Parse(status.ToString()) == 1 ? true : false;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }
        private int DayOfWeek()
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            int dayOfWeek = settingsManager.GetSetting<int>("DAY_OF_WEEK_Rolling_Volume", 99);
            dayOfWeek = dayOfWeek < 0 ? -2 : dayOfWeek;
            dayOfWeek++;
            dayOfWeek = dayOfWeek == 7 ? 0 : dayOfWeek;
            return dayOfWeek;
        }
    }
}
