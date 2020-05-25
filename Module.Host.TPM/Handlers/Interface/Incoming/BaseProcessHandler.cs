using Interfaces.Core.Common;
using Interfaces.Implementation.Action;
using Looper.Core;
using Looper.Parameters;
using ProcessingHost.Handlers;
using System;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.Interface.Incoming
{
    public abstract class BaseProcessHandler : BaseHandler
    {
        protected abstract IAction GetProcessorAction(HandlerInfo info, Guid interfaceId, FileBufferModel fileBuffer);

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Task started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                // передать HandlerId, InterfaceId и UserId
                Guid interfaceId = HandlerDataHelper.GetIncomingArgument<Guid>("InterfaceId", info.Data);
                Guid? userId = HandlerDataHelper.GetIncomingArgument<Guid?>("UserId", info.Data);
                FileBufferModel fileBuffer = HandlerDataHelper.GetIncomingArgument<FileBufferModel>("FileBuffer", info.Data, false);
                IAction action = GetProcessorAction(info, interfaceId, fileBuffer);
                action.Execute();

                // Обработать ошибки
                if (action.Errors.Any())
                {
                    data.SetValue("HasErrors", true);
                    //Работает в тысячи раз быстрее чем обычный цикл
                    string s = "[ERROR]: " + String.Join(Environment.NewLine + "[ERROR]: ", action.Errors.ToArray()) + Environment.NewLine;
                    handlerLogger.Write(true, s);
                }

                // Обработать предупреждения
                if (action.Warnings.Any())
                {
                    data.SetValue("HasWarnings", true);
                    //Работает в тысячи раз быстрее чем обычный цикл
                    string s = "[WARNING]: " + String.Join(Environment.NewLine + "[WARNING]: ", action.Warnings.ToArray()) + Environment.NewLine;
                    handlerLogger.Write(true, s);
                }

                // Результат выполнения задачи
                if (fileBuffer != null)
                {
                    action.SaveResultToData<int>(info.Data, "ImportSourceRecordCount");
                    action.SaveResultToData<int>(info.Data, "ImportResultRecordCount");
                }

                //InterfaceFileListModel viewFiles = new InterfaceFileListModel() {
                //    FilterType = fileBuffer == null ? "INTERFACE" : "HANDLER"
                //};
                //HandlerDataHelper.SaveOutcomingArgument<InterfaceFileListModel>("FileList", viewFiles, info.Data, true, false);
                HandlerDataHelper.SaveOutcomingArgument<int>("ErrorCount", action.Errors.Count, info.Data);
                HandlerDataHelper.SaveOutcomingArgument<int>("WarningCount", action.Warnings.Count, info.Data);
                handlerLogger.Write(true, String.Format("Task ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
            catch (Exception e)
            {
                data.SetValue("HasErrors", true);
                logger.Error(e);
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
        }
    }
}
