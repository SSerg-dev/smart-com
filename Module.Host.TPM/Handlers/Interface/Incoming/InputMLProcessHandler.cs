using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Interfaces.Core.Common;
using Interfaces.Implementation.Inbound.Collector;
using Looper.Core;
using Module.Frontend.TPM.Controllers;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model.Interface;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.FileWorker;
using Utility.LogWriter;
using Utility.Security;

namespace Module.Host.TPM.Handlers.Interface.Incoming
{
    public class InputMLProcessHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, string.Format("File processing begin: {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    // настройки
                    string filesDir = AppSettingsManager.GetSetting("INTERFACE_DIRECTORY", "InterfaceFiles");
                    Guid interfaceId = context.Interfaces.FirstOrDefault(g => g.Name == "ML_CALENDAR_ANAPLAN").Id;
                    FileCollectInterfaceSetting fileCollectInterfaceSetting = context.FileCollectInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);
                    string sourceFilesPath = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath);
                    CSVProcessInterfaceSetting cSVProcessInterfaceSetting = context.CSVProcessInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);

                    var authorizationManager = new SystemAuthorizationManager();
                    UserInfo user = authorizationManager.GetCurrentUser();
                    RoleInfo role = authorizationManager.GetCurrentRole();
                    StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(context);
                    // загружаем новые в FileBuffer
                    IEnumerable<string> files = Directory.EnumerateFiles(sourceFilesPath, fileCollectInterfaceSetting.SourceFileMask, SearchOption.TopDirectoryOnly);
                    IEnumerable<string> fileNames = files.Select(g => Path.GetFileName(g)).OrderBy(f => f);
                    List<FileBuffer> fileBuffers = context.FileBuffers.Where(g => g.InterfaceId == interfaceId).ToList();
                    IEnumerable<string> fBufferNames = fileBuffers.Select(g => g.FileName).OrderBy(f => f);
                    IEnumerable<string> NotPresents = fileNames.Except(fBufferNames);
                    List<FileBuffer> fileBuffersAdd = new List<FileBuffer>();

                    foreach (string filename in NotPresents)
                    {
                        string file = files.FirstOrDefault(g => Path.GetFileName(g) == filename);
                        FileBuffer fileBuffer = new FileBuffer()
                        {
                            Id = Guid.NewGuid(),
                            CreateDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now),
                            FileName = filename,
                            HandlerId = info.HandlerId,
                            InterfaceId = interfaceId,
                            UserId = null,
                            ProcessDate = null,
                            Status = Interfaces.Core.Model.Consts.ProcessResult.None
                        };
                        fileBuffersAdd.Add(fileBuffer);
                    }
                    context.FileBuffers.AddRange(fileBuffersAdd);
                    context.SaveChanges();
                    // обработка файлов с статусами Interfaces.Core.Model.Consts.ProcessResult.None
                    List<FileBuffer> fileBuffersNone = context.FileBuffers
                        .Where(g => g.InterfaceId == interfaceId && g.Status == Interfaces.Core.Model.Consts.ProcessResult.None)
                        .OrderBy(d => d.CreateDate)
                        .ToList();

                    foreach (FileBuffer buffer in fileBuffersNone)
                    {
                        string pathfile = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath, buffer.FileName);
                        var Lines = File.ReadAllLines(pathfile, Encoding.UTF8).ToList();
                        List<InputML> inputMLs = Lines
                               .Skip(1)
                               .Select(x => x.Split(char.Parse(cSVProcessInterfaceSetting.Delimiter)))
                               .Select(x => new InputML
                               {
                                   PromoId = int.Parse(x[0]),
                                   PPG = x[1],
                                   Format = x[2],
                                   ZREP = int.Parse(x[3]),
                                   StartDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[4])),
                                   EndDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[5])),
                                   MechanicMars = x[6],
                                   DiscountMars = double.Parse(x[7], CultureInfo.InvariantCulture),
                                   MechInstore = x[8],
                                   InstoreDiscount = double.Parse(x[9], CultureInfo.InvariantCulture),
                                   PlannedUplift = double.Parse(x[10], CultureInfo.InvariantCulture),
                                   PlanInStoreShelfPrice = double.Parse(x[11], CultureInfo.InvariantCulture),
                                   FormatCode = int.Parse(x[12]),
                                   Source = x[13],
                                   BaseLSV = double.Parse(x[14], CultureInfo.InvariantCulture),
                                   TotalLSV = double.Parse(x[15], CultureInfo.InvariantCulture),
                               })
                               .ToList();
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

                            if (((DateTimeOffset)promo.StartDate).AddDays(15) < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)promo.EndDate)
                            {
                                handlerLogger.Write(true, string.Format("ML Promo: {0} is not in the RS period, startdate: {1:yyyy-MM-dd HH:mm:ss}", promo.MLPromoId, promo.StartDate), "Message");
                            }
                            else
                            {
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
                                promoes.Add(promo);
                            }
                        }
                        RSPeriodHelper.RemoveOldCreateNewRSPeriodML(promoes.FirstOrDefault(), context);
                    }
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
                    handlerLogger.Write(true, string.Format("Finish: {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
