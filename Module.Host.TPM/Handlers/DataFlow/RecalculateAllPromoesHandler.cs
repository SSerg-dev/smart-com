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
using Module.Host.TPM.Handlers.MainNightProcessing;

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

            var databaseContext = new DatabaseContext();
            try
            {
                var handlerData = new HandlerData();
                Guid? userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", handlerData, false);
                Guid? roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", handlerData, false);

                var blockedPromoes = databaseContext.Set<BlockedPromo>().Where(x => x.Disabled == false);
                if (blockedPromoes.Count() == 0)
                {
                    //TODO: It is not a bad practice, it just means that you did not think your code through.
                    while (true)
                    {
                        using (DatabaseContext context = new DatabaseContext())
                        {
                            var changesIncidents = context.Set<ChangesIncident>().Where(x => x.ProcessDate == null && !x.Disabled).Take(1000);
                            if (changesIncidents.Count() > 0)
                            {
                                foreach (var item in changesIncidents) item.Disabled = true;
                                context.SaveChanges();
                            }
                            else break;
                        }
                    }
                    //TODO: It is not a bad practice, it just means that you did not think your code through.
                    while (true)
                    {
                        using (DatabaseContext context = new DatabaseContext())
                        {
                            var productChangesIncidents = context.Set<ProductChangeIncident>().Where(x => x.RecalculationProcessDate == null && !x.Disabled).Take(1000);
                            if (productChangesIncidents.Count() > 0)
                            {
                                foreach (var item in productChangesIncidents) item.Disabled = true;
                                context.SaveChanges();
                            }
                            else break;
                        }
                    }

                    var handler = new LoopHandler()
                    {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Filtering for nightly recalculation (DataFlow)",
                        Name = "Module.Host.TPM.Handlers.DataFlow.DataFlowFilteringHandler",
                        ExecutionPeriod = null,
                        CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId == Guid.Empty ? null : userId,
                        RoleId = roleId == Guid.Empty ? null : roleId
                    };

                    handler.SetParameterData(handlerData);
                    databaseContext.LoopHandlers.Add(handler);

                    databaseContext.SaveChanges();
                    handlerLogger.Write(true, "The task for filtering of promoes was created.", "Message");
                }
                else
                {
                    string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                    }
                    data.SetValue<bool>("HasErrors", true);
                    handlerLogger.Write(true, $"Night recalculating is not possible, there are {blockedPromoes.Count()} blocked promoes.", "Error");
                }
            }
            catch (Exception e)
            {
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                using (DatabaseContext context = new DatabaseContext())
                {
                    MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                }
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The Data Flow initialization ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
            }
            finally
            {
                if (databaseContext != null)
                {
                    databaseContext.SaveChanges();
                    ((IDisposable)databaseContext).Dispose();
                }

                stopWatch.Stop();
                handlerLogger.Write(true, String.Format("The Data Flow initialization ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
        }
    }
}
