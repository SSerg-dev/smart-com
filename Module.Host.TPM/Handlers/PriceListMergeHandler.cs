using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using ProcessingHost.Handlers;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    public class PriceListMergeHandler : BaseHandler
    {
        public override void Action(HandlerInfo handlerInfo, ExecuteData executeData)
        {
            var databaseContext = new DatabaseContext();
            var fileLogWriter = new FileLogWriter(handlerInfo.HandlerId.ToString());

            try
            {
                fileLogWriter.Write(true, String.Format("Price List Merge began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                var needMerge = HandlerDataHelper.GetIncomingArgument<bool>("NeedMerge", handlerInfo.Data, false);
                if (needMerge)
                {
                    var priceListMergeAction = new PriceListMergeAction(databaseContext, fileLogWriter, executeData);
                    priceListMergeAction.Execute();
                }
                else
                {
                    var handlerData = new HandlerData();
                    HandlerDataHelper.SaveIncomingArgument("NeedMerge", true, handlerData, visible: false, throwIfNotExists: false);

                    var curHandler = databaseContext.Set<LoopHandler>().Where(x => x.Id == handlerInfo.HandlerId).FirstOrDefault();
                    var userId = curHandler?.UserId ?? null;
                    var roleId = curHandler?.RoleId ?? null;

                    var handler = new LoopHandler()
                    {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Datalake Price List -> TPM Price List",
                        Name = "Module.Host.TPM.Handlers.PriceListMergeHandler",
                        ExecutionPeriod = null,
                        CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId,
                        RoleId = roleId
                    };

                    handler.SetParameterData(handlerData);
                    databaseContext.LoopHandlers.Add(handler);
                }
                HandlerDataHelper.SaveIncomingArgument("NeedMerge", false, handlerInfo.Data, visible: false, throwIfNotExists: false);

                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                executeData.SetValue("HasErrors", true);
                fileLogWriter.Write(true, e.ToString(), "Error");
            }
            finally
            {
                if (databaseContext != null)
                {
                    (databaseContext as IDisposable)?.Dispose();
                }

                fileLogWriter.Write(true, String.Format("Price List Merge ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
        }
    }
}
