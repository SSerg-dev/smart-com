using Core.Security.Models;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Frontend.TPM.FunctionalHelpers.RSmode;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model.Interface;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utility.LogWriter;
using Utility.Security;

namespace Module.Host.TPM.Actions
{
    class ProcessMLCalendarAction : BaseAction
    {
        private LogWriter HandlerLogger { get; }
        private Guid UserId { get; }
        private Guid RoleId { get; }
        public string HandlerStatus { get; private set; }
        public int RsId { get; set; }
        public Guid HandlerId { get; set; }
        Stopwatch Stopwatch1 = new Stopwatch();
        public ProcessMLCalendarAction(Guid handlerId, LogWriter Logger, Guid userId, Guid roleId, int rsId)
        {
            HandlerLogger = Logger;
            UserId = userId;
            RoleId = roleId;
            RsId = rsId;
            HandlerId = handlerId;
        }

        public override void Execute()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Stopwatch1.Restart();
                Setting settingTime = context.Set<Setting>().FirstOrDefault(g => g.Name == "ML_TIME_BLOCK");
                List<DateTimeOffset> times = settingTime.Value.Split(';').Select(g => ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(g))).ToList();
                DateTimeOffset TimeNow = TimeHelper.Now();
                if (TimeNow < times[0] || times[1] < TimeNow)
                {
                    HandlerLogger.Write(true, string.Format("Scenario calculation can be started from {0} to {1}", times[0], times[1]), "Error");
                    HandlerStatus = "HasErrors";
                    return;
                }
                TimeSpan timespanm = times[2] - TimeNow;

                string filesDir = AppSettingsManager.GetSetting("INTERFACE_DIRECTORY", "InterfaceFiles");
                Guid interfaceId = context.Interfaces.FirstOrDefault(g => g.Name == "ML_CALENDAR_ANAPLAN").Id;
                FileCollectInterfaceSetting fileCollectInterfaceSetting = context.FileCollectInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);
                string sourceFilesPath = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath);
                CSVProcessInterfaceSetting cSVProcessInterfaceSetting = context.CSVProcessInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);
                StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(context);
                UserInfo user = null;
                RoleInfo role = null;
                RollingScenario rollingScenario = new RollingScenario();
                if (RsId == 0)
                {
                    var authorizationManager = new SystemAuthorizationManager();
                    user = authorizationManager.GetCurrentUser();
                    role = authorizationManager.GetCurrentRole();
                    rollingScenario = context.Set<RollingScenario>()
                        .Include(g => g.Promoes)
                        .OrderBy(g => g.RSId).FirstOrDefault(g => g.IsMLmodel && g.RSstatus == RSstateNames.WAITING);
                }
                else
                {
                    var authorizationManager = new ReAuthorizationManager(context, UserId, RoleId);
                    user = authorizationManager.GetCurrentUser();
                    role = authorizationManager.GetCurrentRole();
                    rollingScenario = context.Set<RollingScenario>()
                        .Include(g => g.Promoes)
                        .FirstOrDefault(g => g.RSId == RsId && g.RSstatus == RSstateNames.WAITING);
                }

                if (rollingScenario == null)
                {
                    HandlerLogger.Write(true, "Missing RS Period", "Message");
                }
                FileBuffer buffer = context.Set<FileBuffer>().FirstOrDefault(g => g.InterfaceId == interfaceId && g.Id == rollingScenario.FileBufferId && g.Status == Interfaces.Core.Model.Consts.ProcessResult.None);

                string pathfile = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath, buffer.FileName);
                List<InputML> inputMLs = PromoHelper.GetInputML(pathfile, cSVProcessInterfaceSetting.Delimiter);
                List<int> inputMlIds = inputMLs.Select(g => g.PromoId).Distinct().ToList();
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (int inputMlId in inputMlIds)
                        {
                            InputML firstInputML = inputMLs.FirstOrDefault(g => g.PromoId == inputMlId);
                            Promo promo = PromoHelper.CreateRSDefaultPromo(context);

                            promo.BudgetYear = TimeHelper.ThisStartYear().Year;

                            ClientTree clientTree = context.Set<ClientTree>().Where(x => x.EndDate == null && x.ObjectId == firstInputML.FormatCode).FirstOrDefault();
                            promo.ClientHierarchy = clientTree.FullPathName;
                            promo.ClientTreeId = clientTree.ObjectId;
                            promo.ClientTreeKeyId = clientTree.Id;
                            promo.DeviationCoefficient = clientTree.DeviationCoefficient.Value * 100;

                            promo.StartDate = firstInputML.StartDate;
                            promo.EndDate = firstInputML.EndDate;
                            PromoHelper.ClientDispatchDays clientDispatchDays = PromoHelper.GetClientDispatchDays(clientTree);
                            if (clientDispatchDays.IsStartAdd)
                            {
                                promo.DispatchesStart = firstInputML.StartDate.AddDays(clientDispatchDays.StartDays);
                            }
                            else
                            {
                                promo.DispatchesStart = firstInputML.StartDate.AddDays(-clientDispatchDays.StartDays);
                            }
                            if (clientDispatchDays.IsEndAdd)
                            {
                                promo.DispatchesEnd = firstInputML.EndDate.AddDays(clientDispatchDays.EndDays);
                            }
                            else
                            {
                                promo.DispatchesEnd = firstInputML.EndDate.AddDays(-clientDispatchDays.EndDays);
                            }
                            if ((DateTimeOffset)promo.DispatchesStart < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)promo.EndDate)
                            {
                                HandlerLogger.Write(true, string.Format("ML Promo: {0} is not in the RS period, startdate: {1:yyyy-MM-dd HH:mm:ss}", inputMlId, promo.StartDate), "Warning");
                            }
                            else if (promo.StartDate > promo.EndDate || promo.DispatchesStart > promo.DispatchesEnd)
                            {
                                HandlerLogger.Write(true, string.Format("ML Promo: {0} the start date is greater than the end date", inputMlId), "Warning");
                            }
                            else
                            {

                                List<string> zreps = inputMLs.Where(g => g.PromoId == inputMlId).Select(g => g.ZREP.ToString()).ToList();
                                List<Product> products = context.Set<Product>().Where(g => zreps.Contains(g.ZREP)).ToList();
                                promo.InOutProductIds = string.Join(";", products.Select(g => g.Id));

                                Mechanic mechanic = context.Set<Mechanic>().FirstOrDefault(g => g.SystemName == firstInputML.MechanicMars && g.PromoTypesId == promo.PromoTypesId);
                                promo.MarsMechanicId = mechanic.Id;
                                promo.MarsMechanicDiscount = firstInputML.DiscountMars;
                                Mechanic mechanicInstore = context.Set<Mechanic>().FirstOrDefault(g => g.SystemName == firstInputML.MechInstore && g.PromoTypesId == promo.PromoTypesId);
                                promo.PlanInstoreMechanicId = mechanicInstore.Id;
                                promo.PlanInstoreMechanicDiscount = firstInputML.InstoreDiscount;

                                promo.PlanInStoreShelfPrice = firstInputML.PlanInStoreShelfPrice;
                                promo.PlanPromoUpliftPercent = firstInputML.PlannedUplift;
                                promo.PlanPromoUpliftPercentPI = firstInputML.PlannedUplift;
                                promo.CalculateML = true;

                                PromoHelper.ReturnName returnName = PromoHelper.GetNamePromo(context, mechanic, products.FirstOrDefault(), firstInputML.DiscountMars);
                                promo.Name = returnName.Name;
                                promo.ProductHierarchy = returnName.ProductTree.FullPathName;
                                promo.ProductTreeObjectIds = returnName.ProductTree.ObjectId.ToString();

                                promo.MLPromoId = buffer.FileName + "_" + firstInputML.PromoId;
                                HandlerLogger.Write(true, string.Format("Promo {0} processing has started", promo.MLPromoId), "Message");
                                promo = PromoHelper.SavePromo(promo, context, user, role);
                                rollingScenario.Promoes.Add(promo);
                            }
                        }
                        if (Stopwatch1.Elapsed >= timespanm)
                        {
                            throw new ArgumentException();
                        }
                        transaction.Commit();
                    }
                    catch (ArgumentException)
                    {
                        transaction.Rollback();
                        HandlerLogger.Write(true, "The scenario calculation was not completed during the day. Please repeat the calculation procedure.", "Error");
                        HandlerStatus = "HasErrors";
                        return;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        HandlerLogger.Write(true, e.Message, "Error");
                        HandlerStatus = "HasErrors";
                        return;
                    }
                }
                if (rollingScenario.Promoes.Count > 0)
                {
                    rollingScenario.RSstatus = RSstateNames.CALCULATING;
                    rollingScenario.TaskStatus = TaskStatusNames.INPROGRESS;
                    HandlerLogger.Write(true, string.Format("RS period: {0}. {1} added promo", rollingScenario.RSId, rollingScenario.Promoes.Count), "Message");
                }
                else
                {
                    rollingScenario.RSstatus = RSstateNames.WAITING;
                    rollingScenario.TaskStatus = TaskStatusNames.ERROR;
                    HandlerLogger.Write(true, string.Format("RS period: {0}. No added promo", rollingScenario.RSId), "Message");
                }
                buffer.ProcessDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
                buffer.Status = Interfaces.Core.Model.Consts.ProcessResult.Complete;
                rollingScenario.HandlerId = HandlerId;
                context.SaveChanges();
                if (rollingScenario.Promoes.Count > 0)
                {
                    MLTaskModel mLTask = new MLTaskModel
                    {
                        HandlerId = HandlerId,
                        ClientId = (int)rollingScenario.Promoes.FirstOrDefault().ClientTreeId
                    };
                    string numbers = RSmodeHelper.AddDisableRSPromoFromMLPeriod(rollingScenario.Promoes.ToList(), context);
                    if (!string.IsNullOrEmpty(numbers))
                    {
                        HandlerLogger.Write(true, string.Format("RS period: {0}. {1} mark to delete RS promo", rollingScenario.RSId, numbers), "Message");
                    }
                    CloudTask cloudTask = new CloudTask
                    {
                        PipeLine = "ProcessMLCalendarHandler",
                        CreateDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now),
                        Status = Looper.Consts.StatusName.WAITING,
                        Model = mLTask.GetType().Name,
                        ModelJson = JsonConvert.SerializeObject(mLTask)
                    };
                    context.Set<CloudTask>().Add(cloudTask);
                    context.SaveChanges();

                }
            }
        }
    }
}
