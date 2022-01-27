using ProcessingHost.Handlers;
using System;
using System.Linq;
using Looper.Core;
using Utility.LogWriter;
using System.Diagnostics;
using Interfaces.Core.Common;
using Module.Host.TPM.Actions.Notifications;
using System.Collections.Generic;
using Looper.Parameters;
using Interfaces.Implementation.Action;
using System.Threading;
using Module.Persist.TPM;
using System.Web.Http.OData.Query;
using Module.Persist.TPM.Model.TPM;
using System.Linq.Expressions;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers {
    /// <summary>
    /// Класс для запуска экшена по формированияю и рассылке списка промо для которых подходит новый продукт
    /// </summary>
    public class SchedulerExportHandler : BaseHandler {
        public override void Action(HandlerInfo info, ExecuteData data) {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                int year = HandlerDataHelper.GetIncomingArgument<int>("year", info.Data, false);
                IEnumerable<int> clients = HandlerDataHelper.GetIncomingArgument<IEnumerable<int>>("clients", info.Data, false);
                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data);
                Guid handlerId = HandlerDataHelper.GetIncomingArgument<Guid>("HandlerId", info.Data);
                var rawFilters = HandlerDataHelper.GetIncomingArgument<string>("rawFilters", info.Data);
                
                handlerLogger.Write(true, String.Format("Start of calendar export at 10 {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                Thread.Sleep(10000);
                IAction action = new SchedulerExportAction(clients, year, userId, roleId, rawFilters, handlerId);
                action.Execute();

                if (action.Errors.Any()) {
                    data.SetValue<bool>("HasErrors", true);
                    if (handlerLogger != null) {
                        handlerLogger.Write(true, action.Errors, "Error");
                    }
                } else {
                    action.SaveResultToData<FileModel>(info.Data, "ExportFile", "File");
                }
            } catch (Exception e) {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);
                if (handlerLogger != null) {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            } finally {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();
                if (handlerLogger != null) {
                    handlerLogger.Write(true, String.Format("Newsletter notifications ended at  {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");

                    if(!data.GetValue<bool>("HasErrors", false))
                        handlerLogger.Write(true, "ADF Pipeline will process export file!", "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}