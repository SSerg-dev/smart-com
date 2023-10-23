using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;
using Module.Persist.TPM.Enum;
using Module.Frontend.TPM.FunctionalHelpers.HiddenMode;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Host.TPM.Handlers
{
    class PrepareNightCalculateHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Prepare night calculate started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    List<RollingScenario> RSperiods = context.Set<RollingScenario>()
                        .Include(g => g.Promoes)
                        .Where(g => g.RSstatus != RSstateNames.APPROVED && g.IsCreateMLpromo && !g.Disabled)
                        .ToList();
                    foreach (RollingScenario rs in RSperiods)
                    {
                        if (rs.ScenarioType == ScenarioType.RS)
                        {
                            HiddenModeHelper.SetTypePromoes(rs.Promoes.ToList(), TPMmode.RS);
                        }
                        if (rs.ScenarioType == ScenarioType.RA)
                        {
                            HiddenModeHelper.SetTypePromoes(rs.Promoes.ToList(), TPMmode.RA);
                        }
                    }
                    RSperiods = context.Set<RollingScenario>()
                        .Include(g => g.Promoes)
                        .Where(g => g.RSstatus == RSstateNames.CALCULATING && g.TaskStatus == TaskStatusNames.INPROGRESS && !g.Disabled)
                        .ToList();
                    context.SaveChanges();
                    RSperiods = RSperiods.Where(g => g.Promoes.Any(f => f.TPMmode == TPMmode.Hidden)).ToList();
                    foreach (RollingScenario rs in RSperiods)
                    {
                        if (rs.ScenarioType == ScenarioType.RS)
                        {
                            HiddenModeHelper.SetTypePromoes(rs.Promoes.ToList(), TPMmode.RS);
                        }
                        if (rs.ScenarioType == ScenarioType.RA)
                        {
                            HiddenModeHelper.SetTypePromoes(rs.Promoes.ToList(), TPMmode.RA);
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
                    handlerLogger.Write(true, String.Format("Prepare night calculate ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
