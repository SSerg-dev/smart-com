using Core.Settings;
using Interfaces.Core.Common;
using Interfaces.Implementation.Inbound.Collector;
using Looper.Core;
using Module.Persist.TPM.Model.SimpleModel;
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
                handlerLogger.Write(true, String.Format("File processing begin: {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    // настройки
                    string filesDir = AppSettingsManager.GetSetting("INTERFACE_DIRECTORY", "InterfaceFiles");
                    Guid interfaceId = context.Interfaces.FirstOrDefault(g => g.Name == "ML_CALENDAR_ANAPLAN").Id;
                    FileCollectInterfaceSetting fileCollectInterfaceSetting = context.FileCollectInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);
                    string sourceFilesPath = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath);
                    CSVProcessInterfaceSetting cSVProcessInterfaceSetting = context.CSVProcessInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceId);
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
                                   ClientCode = int.Parse(x[3]),
                                   ZREP = int.Parse(x[4]),
                                   StartDate = x[5],
                                   EndDate = x[6],
                                   MechanicMars = x[7],
                                   DiscountMars = int.Parse(x[8]),
                                   MechInstore = x[9],
                                   InstoreDiscount = x[10],
                                   PlannedUplift = x[11],
                                   PlanInStoreShelfPrice = x[12],
                                   TypeML = x[13]
                               })
                               .ToList();

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
                    handlerLogger.Write(true, String.Format("Finish: {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
