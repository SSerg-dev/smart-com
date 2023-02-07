using Looper.Core;
using Module.Persist.TPM.Utils;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                //using (DatabaseContext context = new DatabaseContext())
                //{
                //    List<RollingScenario> RSperiods = context.Set<RollingScenario>()
                //        .Include(g => g.Promoes)
                //        .Include(g => g.PromoStatus)
                //        .Where(g => g.ExpirationDate != null && g.PromoStatus.SystemName != StateNames.APPROVED)
                //        .ToList();
                //    foreach (RollingScenario rs in RSperiods)
                //    {
                //        if (rs.ExpirationDate < DateTimeOffset.Now)
                //        {
                //            RSPeriodHelper.DeleteRSPeriod(rs.Id, context);
                //        }
                //    }
                //}
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
