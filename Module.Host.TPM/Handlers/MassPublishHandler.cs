using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Utils;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class MassPublishHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, string.Format("Processing Mass Promo Publication began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                List<Guid> PromoIds = HandlerDataHelper.GetIncomingArgument<List<Guid>>("PromoIds", info.Data, false);
                Guid userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
                Guid roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);

                MassPublishAction action = new MassPublishAction(handlerLogger, userId, roleId, PromoIds, info.HandlerId);
                action.Execute();
                if (!string.IsNullOrEmpty(action.HandlerStatus))
                {
                    data.SetValue<bool>(action.HandlerStatus, true);
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
                    handlerLogger.Write(true, string.Format("Processing Mass Promo Publication ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
