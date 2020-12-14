using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.MarsCalendar;
using Looper.Parameters;
using Persist;
using Persist.Model;
using Persist.Model.Interface;
using Looper.Core;
using Core.Security;

namespace Module.Persist.TPM.Utils
{
    class ConfigManager
    {
        //Inbound
        public static void AddInputBaselineInterfaceSettings(bool updateInterfaces, DatabaseContext context)
        {
            string fileMask = @"*.dat*";
            string collectHandler = "ProcessingHost.Handlers.Interface.Incoming.FileCollectHandler";
            string processHandlerName = "Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler";
            string interfaceName = "BASELINE_APOLLO";
            string description = "Input Baseline from Apollo";
            string direction = "INBOUND";
            string sourcePath = @"D:/Interfaces";
            string delimiter = ",";
            bool useQuoting = false;
            string quoteChar = "";
            context.LoopHandlers.RemoveRange(context.LoopHandlers.Where(x => x.Name == "Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler"));
            context.LoopHandlers.RemoveRange(context.LoopHandlers.Where(x => x.Name == "ProcessingHost.Handlers.Interface.Incoming.FileCollectHandler"));
            // Добавить интерфейс
            Interface @interface = GetInterfaceInboundCSV(
                context,
                updateInterfaces,
                interfaceName,
                description,
                direction,
                fileMask,
                sourcePath,
                collectHandler,
                processHandlerName,
                delimiter,
                useQuoting,
                quoteChar);

            LoopHandler h1 = new LoopHandler()
            {
                Name = collectHandler,
                Description = "Incoming files collecting",
                ConfigurationName = "PROCESSING",
                ExecutionPeriod = 300000,
                ExecutionMode = Looper.Consts.ExecutionModes.PERIOD,
                CreateDate = DateTimeOffset.Now,
                LastExecutionDate = null,
                NextExecutionDate = null,
                UserId = null,
            };
            h1.SetParameterData(GetCollectorHandlerData(@interface));
            context.LoopHandlers.Add(h1);

            LoopHandler h2 = new LoopHandler()
            {
                Name = processHandlerName,
                Description = "Incoming baseline processing",
                ConfigurationName = "PROCESSING",
                ExecutionPeriod = 86400000,
                ExecutionMode = Looper.Consts.ExecutionModes.PERIOD,
                CreateDate = DateTimeOffset.Now,
                LastExecutionDate = null,
                NextExecutionDate = null,
                UserId = null,
            };
            h2.SetParameterData(GetProcessorInputHandlerData(@interface));
            context.LoopHandlers.Add(h2);
        }

        private static Interface GetInterfaceInboundCSV(DatabaseContext context, bool needUpdate, string interfaceName, string description, string direction, string fileMask, string sourcePath, string collectHandlerName, string processHandlerName, string delimiter, bool useQuoting, string quoteChar)
        {
            Interface @interface;
            if (needUpdate)
            {
                // Добавить интерфейс
                @interface = new Interface()
                {
                    Name = interfaceName,
                    Description = description,
                    Direction = direction
                };
                context.Interfaces.Add(@interface);
                // Добавить настройки сбора файлов
                context.FileCollectInterfaceSettings.Add(new FileCollectInterfaceSetting()
                {
                    Interface = @interface,
                    SourcePath = sourcePath,
                    SourceFileMask = fileMask,
                    CollectHandler = collectHandlerName
                });
                // Добавить настройки разбора файлов
                context.CSVProcessInterfaceSettings.Add(new CSVProcessInterfaceSetting()
                {
                    Interface = @interface,
                    Delimiter = delimiter,
                    UseQuoting = useQuoting,
                    QuoteChar = quoteChar,
                    ProcessHandler = processHandlerName
                });
                context.SaveChanges();
            }
            else
            {
                @interface = context.Interfaces.FirstOrDefault(x => x.Name.Equals(interfaceName));
                if (@interface == null)
                {
                    throw new Exception(String.Format("Interface '{0}' not fount in database", interfaceName));
                }
            }
            return @interface;
        }

        private static HandlerData GetCollectorHandlerData(Interface @interface)
        {
            HandlerData data = new HandlerData();
            InterfaceFileListModel viewFiles = new InterfaceFileListModel()
            {
                FilterType = "INTERFACE"
            };
            ManualFileCollectModel manualCollect = new ManualFileCollectModel();
            HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, data, true, false);
            HandlerDataHelper.SaveIncomingArgument<ManualFileCollectModel>("ManualCollect", manualCollect, data, true, false);
            HandlerDataHelper.SaveIncomingArgument<Guid?>("UserId", null, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid>("InterfaceId", @interface.Id, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<string>("InterfaceName", @interface.Name, data, true, false);
            return data;
        }

        private static HandlerData GetProcessorInputHandlerData(Interface @interface)
        {
            HandlerData data = new HandlerData();
            InterfaceFileListModel viewFiles = new InterfaceFileListModel()
            {
                FilterType = "INTERFACE"
            };
            //ManualFileProcessModel manualProcess = new ManualFileProcessModel();
            HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, data, true, false);
            //HandlerDataHelper.SaveIncomingArgument<ManualFileProcessModel>("ManualProcess", manualProcess, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid?>("UserId", null, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid>("InterfaceId", @interface.Id, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<string>("InterfaceName", @interface.Name, data, true, false);
            return data;
        }

        //Outbound
        public static void AddOutputIncrementalInterfaceSettings(bool updateInterfaces, DatabaseContext context)
        {
            string TargetFileMask = @"IN_PROMO_UPLIFT_DMD_STG_0125.dat";
            string sendHandlerName = "ProcessingHost.Handlers.Interface.Outcoming.FileSendHandler";
            string processHandlerName = "Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler";
            string interfaceName = "INCREMENTAL_TO_APOLLO";
            string description = "Output Incremental To Apollo";
            string direction = "OUTBOUND";
            string targetPath = @"D:/OutboundInterfaces";
            string delimiter = ",";
            bool useQuoting = false;
            string quoteChar = "";
            context.LoopHandlers.RemoveRange(context.LoopHandlers.Where(x => x.Name == "Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler"));
            context.LoopHandlers.RemoveRange(context.LoopHandlers.Where(x => x.Name == "ProcessingHost.Handlers.Interface.Outcoming.FileSendHandler"));
            // Добавить интерфейс
            Interface @interface = GetInterfaceOutboundCSV(
                context,
                updateInterfaces,
                interfaceName,
                description,
                direction,
                TargetFileMask,
                targetPath,
                sendHandlerName,
                processHandlerName,
                delimiter,
                useQuoting,
                quoteChar);

            LoopHandler h1 = new LoopHandler()
            {
                Name = sendHandlerName,
                Description = "Outcoming files sending",
                ConfigurationName = "PROCESSING",
                ExecutionPeriod = 300000,
                ExecutionMode = Looper.Consts.ExecutionModes.PERIOD,
                CreateDate = DateTimeOffset.Now,
                LastExecutionDate = null,
                NextExecutionDate = null,
                UserId = null,
            };
            h1.SetParameterData(GetDeliveryHandlerData(@interface));
            context.LoopHandlers.Add(h1);

            LoopHandler h2 = new LoopHandler()
            {
                Name = processHandlerName,
                Description = "Outcoming incremental processing",
                ConfigurationName = "PROCESSING",
                ExecutionPeriod = 86400000,
                ExecutionMode = Looper.Consts.ExecutionModes.PERIOD,
                CreateDate = DateTimeOffset.Now,
                LastExecutionDate = null,
                NextExecutionDate = null,
                UserId = null,
            };
            h2.SetParameterData(GetProcessorOutputHandlerData(@interface));
            context.LoopHandlers.Add(h2);
        }

        private static Interface GetInterfaceOutboundCSV(DatabaseContext context, bool needUpdate, string interfaceName, string description, string direction, string TargetFileMask, string targetPath, string sendHandlerName, string processHandlerName, string delimiter, bool useQuoting, string quoteChar)
        {
            Interface @interface;
            if (needUpdate)
            {
                // Добавить интерфейс
                @interface = new Interface()
                {
                    Name = interfaceName,
                    Description = description,
                    Direction = direction
                };
                context.Interfaces.Add(@interface);
                // Добавить настройки отправки файлов
                context.FileSendInterfaceSettings.Add(new FileSendInterfaceSetting()
                {
                    Interface = @interface,
                    TargetPath = targetPath,
                    TargetFileMask = TargetFileMask,
                    SendHandler = sendHandlerName
                });
                // Добавить настройки сбора файла
                context.CSVExtractInterfaceSettings.Add(new CSVExtractInterfaceSetting()
                {
                    Interface = @interface,
                    ExtractHandler = processHandlerName,
                    Delimiter = delimiter,
                    FileNameMask = TargetFileMask,
                    QuoteChar = quoteChar,
                    UseQuoting = useQuoting
                });
                context.SaveChanges();
            }
            else
            {
                @interface = context.Interfaces.FirstOrDefault(x => x.Name.Equals(interfaceName));
                if (@interface == null)
                {
                    throw new Exception(String.Format("Interface '{0}' not fount in database", interfaceName));
                }
            }
            return @interface;
        }

        private static HandlerData GetDeliveryHandlerData(Interface @interface)
        {
            HandlerData data = new HandlerData();
            InterfaceFileListModel viewFiles = new InterfaceFileListModel()
            {
                FilterType = "INTERFACE"
            };
            ManualFileCollectModel manualCollect = new ManualFileCollectModel();
            HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, data, true, false);
            HandlerDataHelper.SaveIncomingArgument<ManualFileCollectModel>("ManualCollect", manualCollect, data, true, false);
            HandlerDataHelper.SaveIncomingArgument<Guid?>("UserId", null, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid>("InterfaceId", @interface.Id, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<string>("InterfaceName", @interface.Name, data, true, false);
            return data;
        }

        private static HandlerData GetProcessorOutputHandlerData(Interface @interface)
        {
            HandlerData data = new HandlerData();
            InterfaceFileListModel viewFiles = new InterfaceFileListModel()
            {
                FilterType = "INTERFACE"
            };
            //ManualFileProcessModel manualProcess = new ManualFileProcessModel();
            HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, data, true, false);
            //HandlerDataHelper.SaveIncomingArgument<ManualFileProcessModel>("ManualProcess", manualProcess, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid?>("UserId", null, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<Guid>("InterfaceId", @interface.Id, data, false, false);
            HandlerDataHelper.SaveIncomingArgument<string>("InterfaceName", @interface.Name, data, true, false);
            return data;
        }
    }
}
