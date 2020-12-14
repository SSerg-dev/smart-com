using Core.Dependency;
using Core.Settings;
using Interfaces.Core.Common;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.LogWriter;
using _Interface = Persist.Model.Interface;

namespace Module.Host.TPM.Handlers
{
    class NightProcessingWaitHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Night processing wait began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                IAction action = new NightProcessingWaitAction();
                action.Execute();

                if (action.Results.Any())
                {
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Results.Keys, "Message");
                    }
                }
                if (action.Warnings.Any())
                {
                    data.SetValue<bool>("HasWarnings", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Warnings, "Warning");
                    }
                }
                // Если в прцессе выполнения возникли ошибки, статус задачи устанавливаем ERROR
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Errors, "Error");                          
                    }

                    //Вызов хранимой процедуры, на случай, если промо не разблокируются в пайплайне
                    UnblockPromo(handlerLogger, info.HandlerId);
                }
                else
                {
                    LoopHandler handler;

                    handlerLogger.Write(true, "Start promo reapproval processing", "Message");
                    CreateHandler(
                    "Promo reapproval processing",
                    "Module.Host.TPM.Handlers.ReapprovingHandler");

                    var currentDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    if (currentDate.DayOfWeek == ((DayOfWeek)DayOfWeek()) || DayOfWeek() > 6)
                    {
                        handlerLogger.Write(true, "Start rolling promo collecting", "Message");
                        handler = CreateHandler(
                        "Rolling promo collecting",
                        "Module.Host.TPM.Handlers.RollingVolumeQTYRecalculationHandler");

                        SetHandlerWaiting(handler, handlerLogger, "ROLLING_VOLUME_QTY_RECALCULATION_HANDLER_TIMEOUT");
                    }

                    handlerLogger.Write(true, "Start outcoming incremental processing", "Message");
                    CreateHandler(
                        "Outcoming incremental processing",
                        "Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler",
                        true);
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
                    handlerLogger.Write(true, String.Format("Night processing wait ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }

        private LoopHandler CreateHandler(string handlerDescription, string handlerName, bool isOutput = false)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                LoopHandler handler = CreateSingleLoopHandler(handlerDescription, handlerName);

                HandlerData data = new HandlerData();
                if (isOutput)
                {
                    string outcomingInterfaceName = AppSettingsManager.GetSetting<string>("OUTCOMING_INCREMENTAL_TO_APOLLO_INTERFACE_NAME", "INCREMENTAL_TO_APOLLO");
                    _Interface.Interface outcomingInterface = context.Set<_Interface.Interface>().Where(x => x.Name == outcomingInterfaceName).FirstOrDefault();
                    data = GetProcessorHandlerData(outcomingInterface);
                }

                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();

                return handler;
            }
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
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
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

        private void SetHandlerWaiting(LoopHandler handler, LogWriter handlerLogger, string timeoutName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                int rollingVolumeQTYRecalculationHandlerTimeout = AppSettingsManager.GetSetting<int>(timeoutName, 1800000);

                SetProcessingFlagUp(context, mainNightProcessingStepPrefix);
                HandlerWaiting(context, handler, rollingVolumeQTYRecalculationHandlerTimeout, handlerLogger.CurrentLogWriter);
            }
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
                var status = context.SqlQuery<Byte>(checkJobStatusScript).FirstOrDefault();
                return int.Parse(status.ToString()) == 1;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        public static void SetProcessingFlagUp(DatabaseContext context, string processingPrefix)
        {
            string setProcessingFlagUpScript = string.Format(Consts.Templates.setProcessingFlagUpTemplate, processingPrefix);
            context.ExecuteSqlCommand(setProcessingFlagUpScript);
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

        private void UnblockPromo(LogWriter handlerLogger, Guid handlerId)
        {
            try
            {
                handlerLogger.Write(true, "Start unblock promoes", "Message");
                using (DatabaseContext context = new DatabaseContext())
                {
                    string sqlScript = $"EXEC [DefaultSchemaSetting].[UnblockPromo] '{handlerId}'";
                    context.ExecuteSqlCommand(sqlScript);
                }
            }
            catch(Exception e)
            {
                handlerLogger.Write(true, e.Message, "Error");
            }
        }
    }
}
