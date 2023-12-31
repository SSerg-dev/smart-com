﻿using Looper.Core;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class RSperiodExpiredHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Decline expired RS periods started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    List<RollingScenario> RSperiods = context.Set<RollingScenario>()
                        .Include(g => g.Promoes)
                        .Where(g => g.ExpirationDate != null && g.RSstatus != RSstateNames.APPROVED)
                        .ToList();
                    foreach (RollingScenario rs in RSperiods)
                    {
                        if (rs.ExpirationDate < DateTimeOffset.Now)
                        {
                            ScenarioHelper.DeleteScenarioPeriod(rs.Id, context);
                        }
                    }
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
                    handlerLogger.Write(true, String.Format("Decline expired RS periods ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
