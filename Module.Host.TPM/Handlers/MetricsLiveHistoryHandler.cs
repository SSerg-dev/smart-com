using Looper.Core;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.ScriptGenerator.Filter;
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
    class MetricsLiveHistoryHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Calculate Metrics Live History started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                using (DatabaseContext context = new DatabaseContext())
                {
                    List<ClientTreeHierarchyView> clientTreeHierarchyView = context.Set<ClientTreeHierarchyView>().ToList();
                    clientTreeHierarchyView = clientTreeHierarchyView.Skip(3).ToList();
                    List<MetricsLiveHistory> metricsLiveHistories = new List<MetricsLiveHistory>();
                    foreach (ClientTreeHierarchyView treeHierarchyView in clientTreeHierarchyView)
                    {
                        int generalClient = treeHierarchyView.Id;
                        List<PromoGridView> promoes = GetConstraintedQueryPromo(context, generalClient);
                        var Ppa = LiveMetricsDashboard.GetPPA(promoes);
                        var Pct = LiveMetricsDashboard.GetPCT(promoes);
                        // записывать
                        metricsLiveHistories.Add(new MetricsLiveHistory
                        {
                            ClientTreeId = generalClient,
                            Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value,
                            Type = TypeMetrics.PPA,
                            Value = Ppa.ValueReal,
                            ValueLSV = Ppa.ValueLSVReal
                        });
                        metricsLiveHistories.Add(new MetricsLiveHistory
                        {
                            ClientTreeId = generalClient,
                            Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value,
                            Type = TypeMetrics.PCT,
                            Value = Pct.ValueReal,
                            ValueLSV = Pct.ValueLSVReal
                        });
                    }
                    context.Set<MetricsLiveHistory>().AddRange(metricsLiveHistories);
                    context.SaveChanges();
                    handlerLogger.Write(true, string.Format("Metrics Live Histories records add {0}.", metricsLiveHistories.Count), "Message");
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
                    handlerLogger.Write(true, String.Format("Calculate Metrics Live History ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
        private static List<PromoGridView> GetConstraintedQueryPromo(DatabaseContext Context, int clientId)
        {


            IQueryable<PromoGridView> query = Context.Set<PromoGridView>()
                .AsNoTracking()
                .Where(x => !x.IsOnHold);

            List<string> clients = new List<string> { clientId.ToString() };
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, TPMmode.Current, clients, hierarchy, FilterQueryModes.Active);
            return query.ToList();
        }
    }
}
