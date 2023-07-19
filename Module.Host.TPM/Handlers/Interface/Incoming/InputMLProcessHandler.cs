﻿using Core.Security.Models;
using Core.Settings;
using Looper.Core;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
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
                    Guid interfaceIdRS = context.Interfaces.FirstOrDefault(g => g.Name == "ML_CALENDAR_ANAPLAN_RS").Id;
                    FileCollectInterfaceSetting fileCollectInterfaceSettingRS = context.FileCollectInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceIdRS);
                    string sourceFilesPathRS = Path.Combine(filesDir, fileCollectInterfaceSettingRS.SourcePath);
                    CSVProcessInterfaceSetting cSVProcessInterfaceSettingRS = context.CSVProcessInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceIdRS);

                    // загружаем новые в FileBuffer
                    IEnumerable<string> files = Directory.EnumerateFiles(sourceFilesPathRS, fileCollectInterfaceSettingRS.SourceFileMask, SearchOption.TopDirectoryOnly);
                    IEnumerable<string> fileNames = files.Select(g => Path.GetFileName(g)).OrderBy(f => f);
                    List<FileBuffer> fileBuffers = context.FileBuffers.Where(g => g.InterfaceId == interfaceIdRS).ToList();
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
                            InterfaceId = interfaceIdRS,
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
                        string pathfile = Path.Combine(filesDir, fileCollectInterfaceSettingRS.SourcePath, buffer.FileName);
                        List<InputML> inputMLs = PromoHelper.GetInputML(pathfile, cSVProcessInterfaceSettingRS.Delimiter);
                        List<int> inputMlClients = inputMLs.Select(g => g.FormatCode).Distinct().ToList();
                        foreach (int client in inputMlClients)
                        {
                            ScenarioHelper.RemoveOldCreateNewRSPeriodML(client, buffer.Id, context);
                        }
                        if (inputMlClients.Count == 0)
                        {
                            handlerLogger.Write(true, string.Format("Empty file or error format, filename: {0}", buffer.FileName), "Error");
                            data.SetValue<bool>("HasErrors", true);
                            logger.Error(new Exception(string.Format("Empty file or error format, filename: {0}", buffer.FileName)));
                        }
                    }
                    context.SaveChanges();
                    ReadMLRA(context, info, data, handlerLogger);
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
        private void ReadMLRA(DatabaseContext context, HandlerInfo info, ExecuteData data, LogWriter handlerLogger)
        {
            // настройки
            string filesDir = AppSettingsManager.GetSetting("INTERFACE_DIRECTORY", "InterfaceFiles");
            Guid interfaceIdRA = context.Interfaces.FirstOrDefault(g => g.Name == "ML_CALENDAR_ANAPLAN_RA").Id;
            FileCollectInterfaceSetting fileCollectInterfaceSettingRA = context.FileCollectInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceIdRA);
            string sourceFilesPathRA = Path.Combine(filesDir, fileCollectInterfaceSettingRA.SourcePath);
            CSVProcessInterfaceSetting cSVProcessInterfaceSettingRA = context.CSVProcessInterfaceSettings.FirstOrDefault(g => g.InterfaceId == interfaceIdRA);

            // загружаем новые в FileBuffer
            IEnumerable<string> files = Directory.EnumerateFiles(sourceFilesPathRA, fileCollectInterfaceSettingRA.SourceFileMask, SearchOption.TopDirectoryOnly);
            IEnumerable<string> fileNames = files.Select(g => Path.GetFileName(g)).OrderBy(f => f);
            List<FileBuffer> fileBuffers = context.FileBuffers.Where(g => g.InterfaceId == interfaceIdRA).ToList();
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
                    InterfaceId = interfaceIdRA,
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
                string pathfile = Path.Combine(filesDir, fileCollectInterfaceSettingRA.SourcePath, buffer.FileName);
                List<InputML> inputMLs = PromoHelper.GetInputML(pathfile, cSVProcessInterfaceSettingRA.Delimiter);
                List<int> inputMlClients = inputMLs.Select(g => g.FormatCode).Distinct().ToList();
                foreach (int client in inputMlClients)
                {
                    ScenarioHelper.RemoveOldCreateNewRAPeriodML(client, buffer.Id, context);
                }
                if (inputMlClients.Count == 0)
                {
                    handlerLogger.Write(true, string.Format("Empty file or error format, filename: {0}", buffer.FileName), "Error");
                    data.SetValue<bool>("HasErrors", true);
                    logger.Error(new Exception(string.Format("Empty file or error format, filename: {0}", buffer.FileName)));
                }
            }
            context.SaveChanges();
        }
    }
}
