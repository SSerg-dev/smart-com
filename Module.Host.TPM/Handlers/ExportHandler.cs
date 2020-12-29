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
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.DTO;
using System.Reflection;
using Persist;
using System.Text.RegularExpressions;
using LinqToQuerystring;
using Core.Security;

namespace Module.Host.TPM.Handlers
{
    public class ExportHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());

                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data);
                Type tModel = HandlerDataHelper.GetIncomingArgument<Type>("TModel", info.Data);
                Type tKey = HandlerDataHelper.GetIncomingArgument<Type>("TKey", info.Data);
                Type columnInstance = HandlerDataHelper.GetIncomingArgument<Type>("GetColumnInstance", info.Data);
                string getColumnMethodName = HandlerDataHelper.GetIncomingArgument<string>("GetColumnMethod", info.Data);
                string additionalColumn = HandlerDataHelper.GetIncomingArgument<string>("GetColumnMethodParams", info.Data, throwIfNotExists: false);
                string sqlString = HandlerDataHelper.GetIncomingArgument<string>("SqlString", info.Data, throwIfNotExists: false);
                Type exportModel = HandlerDataHelper.GetIncomingArgument<Type>("ExportModel", info.Data, throwIfNotExists: false);
                bool simpleModel = HandlerDataHelper.GetIncomingArgument<bool>("SimpleModel", info.Data, throwIfNotExists: false);
                bool IsActuals = HandlerDataHelper.GetIncomingArgument<bool>("IsActuals", info.Data, throwIfNotExists: false);
                string customFileName = HandlerDataHelper.GetIncomingArgument<string>("CustomFileName", info.Data, throwIfNotExists: false);
                string url = HandlerDataHelper.GetIncomingArgument<string>("URL", info.Data, throwIfNotExists: false);

                var getColumnMethod = columnInstance.GetMethod(getColumnMethodName);
                object[] columnsParam;
                if (IsActuals || additionalColumn != null)
                {
                    columnsParam = new object[] { additionalColumn };
                } else
                {
                    columnsParam = null;
                }
                var columns = getColumnMethod.Invoke(null, columnsParam);

                Type type = typeof(ExportAction<,>).MakeGenericType(tModel, tKey);
                IAction action = Activator.CreateInstance(type, userId, roleId, columns, sqlString, exportModel, simpleModel, url, IsActuals, customFileName) as IAction;
                MethodInfo execute = type.GetMethod(nameof(action.Execute));

                handlerLogger.Write(true, String.Format("Start of {0} export at {1:yyyy-MM-dd HH:mm:ss}", tModel.Name, ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                Thread.Sleep(10000);

                execute.Invoke(action, null);
                
                if (action.Errors.Any())
                {
                    data.SetValue<bool>("HasErrors", true);
                    if (handlerLogger != null)
                    {
                        handlerLogger.Write(true, action.Errors, "Error");
                    }
                }
                else
                {
                    action.SaveResultToData<FileModel>(info.Data, "ExportFile", "File");
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
                    handlerLogger.Write(true, String.Format("End of export at  {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");

                    if (!data.GetValue<bool>("HasErrors", false))
                        handlerLogger.Write(true, "You can download the file!", "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}