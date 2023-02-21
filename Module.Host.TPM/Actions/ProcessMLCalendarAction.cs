using Core.Security.Models;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ProcessMLCalendarAction(LogWriter Logger, Guid userId, Guid roleId, int rsId)
        {
            HandlerLogger = Logger;
            UserId = userId;
            RoleId = roleId;
            RsId = rsId;
        }

        public override void Execute()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
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
                FileBuffer buffer = context.Set<FileBuffer>().FirstOrDefault(g => g.InterfaceId == interfaceId && g.Id == rollingScenario.FileBufferId);

                string pathfile = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath, buffer.FileName);
                List<InputML> inputMLs = PromoHelper.GetInputML(pathfile, cSVProcessInterfaceSetting.Delimiter);
                List<int> inputMlIds = inputMLs.Select(g => g.PromoId).Distinct().ToList();
                List<Promo> promoes = new List<Promo>();
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
                        HandlerLogger.Write(true, string.Format("ML Promo: {0} is not in the RS period, startdate: {1:yyyy-MM-dd HH:mm:ss}", promo.MLPromoId, promo.StartDate), "Message");
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

                        promo.CalculateML = true;

                        PromoHelper.ReturnName returnName = PromoHelper.GetNamePromo(context, mechanic, products.FirstOrDefault(), firstInputML.DiscountMars);
                        promo.Name = returnName.Name;
                        promo.ProductHierarchy = returnName.ProductTree.FullPathName;
                        promo.ProductTreeObjectIds = returnName.ProductTree.ObjectId.ToString();

                        promo = PromoHelper.SavePromo(promo, context, user, role);
                        rollingScenario.Promoes.Add(promo);
                    }
                }
                rollingScenario.RSstatus = RSstateNames.CALCULATING;
                rollingScenario.TaskStatus = TaskStatusNames.INPROGRESS;
                context.SaveChanges();

            }
        }
    }
}
