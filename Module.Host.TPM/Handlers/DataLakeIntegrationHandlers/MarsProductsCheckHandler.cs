﻿using Interfaces.Core.Common;
using Interfaces.Implementation.Action;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Host.TPM.Actions.DataLakeIntegrationActions;
using Module.Host.TPM.Actions.Notifications;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.DataLakeIntegrationHandlers
{
	/// <summary>
	/// Класс для запуска экшена по проверке наличия новых или обновленных продуктов из DataLake
	/// </summary>
	public class MarsProductsCheckHandler : BaseHandler
    {
        private Guid? roleId;
        private Guid? userId;
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    handlerLogger = new LogWriter(info.HandlerId.ToString());
                    handlerLogger.Write(true, String.Format("Synchronization materials with products began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                    //string param = HandlerDataHelper.GetIncomingArgument<paramType>("paramName", info.Data).Value;

                    LoopHandler currentHandler = context.LoopHandlers.Find(info.HandlerId);
                    if (currentHandler != null)
                    {
                        userId = currentHandler.UserId;
                        roleId = currentHandler.RoleId;
                    }

                    IAction action = new MarsProductsCheckAction(info.HandlerId.ToString(), userId, roleId);
                    action.Execute();
                    // Если в процессе выполнения возникли ошибки, статус задачи устанавливаем ERROR
                    if (action.Results.Any())
                    {
                        string[] exceptMsgs = { "DataLakeSyncSourceRecordCount", "DataLakeSyncResultRecordCount", "ErrorCount", "WarningCount", "DataLakeSyncResultFilesModel" };
                        var results = action.Results.Keys.Where(x => !exceptMsgs.Contains(x));
                        if (handlerLogger != null)
                        {
                            handlerLogger.Write(true, results, "Message");
                        }
                    }
                    if (action.Warnings.Any())
                    {
                        data.SetValue<bool>("HasWarnings", true);
                        if (handlerLogger != null)
                        {
                            handlerLogger.Write(true, action.Warnings, "Warning");
                        }
                    }
                    if (action.Errors.Any())
                    {
                        data.SetValue<bool>("HasErrors", true);
                        if (handlerLogger != null)
                        {
                            handlerLogger.Write(true, action.Errors, "Error");
                        }
                    }

                    action.SaveResultToData<int>(info.Data, "DataLakeSyncSourceRecordCount");
                    action.SaveResultToData<int>(info.Data, "DataLakeSyncResultRecordCount");
                    HandlerDataHelper.SaveOutcomingArgument<int>("ErrorCount", action.GetResult<int>("ErrorCount", 0), info.Data, true, false);
                    action.SaveResultToData<int>(info.Data, "WarningCount");
                    action.SaveResultToData<DataLakeSyncResultFilesModel>(info.Data, "DataLakeSyncResultFilesModel");

                    string resultStatus = action.GetResult<string>("DataLakeSyncResultStatus", "ERROR");
                    data.SetValue<string>("DataLakeSyncResultStatus", resultStatus);
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
                    handlerLogger.Write(true, String.Format("Synchronization materials with products ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
