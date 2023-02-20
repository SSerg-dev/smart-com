using Core.Security.Models;
using Core.Settings;
using Looper.Core;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model.Interface;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                    // создаем RS периоды
                    foreach (FileBuffer buffer in fileBuffersAdd)
                    {
                        string pathfile = Path.Combine(filesDir, fileCollectInterfaceSetting.SourcePath, buffer.FileName);
                        List<InputML> inputMLs = PromoHelper.GetInputML(pathfile, cSVProcessInterfaceSetting.Delimiter);
                        List<int> inputMlClients = inputMLs.Select(g => g.FormatCode).Distinct().ToList();
                        foreach (int client in inputMlClients)
                        {
                            RSPeriodHelper.RemoveOldCreateNewRSPeriodML(client, buffer.Id, context);
                        }
                        buffer.Status = Interfaces.Core.Model.Consts.ProcessResult.Complete;
                        buffer.ProcessDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
                    }
                    context.SaveChanges();
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
