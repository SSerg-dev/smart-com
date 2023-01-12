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
                    List<ClientTreeHierarchyView> clientTreeHierarchyView = context.Set<ClientTreeHierarchyView>().Skip(3).ToList();
                    foreach (ClientTreeHierarchyView treeHierarchyView in clientTreeHierarchyView)
                    {
                        List<string> clientsString = treeHierarchyView.Hierarchy.Split('.').ToList();
                        List<int> clients = clientsString.ConvertAll(int.Parse);
                        int generalClient = treeHierarchyView.Id;
                        var promoes = GetConstraintedQueryPromo(context, clients);
                        var Ppa = LiveMetricsDashboard.GetPPA(promoes);
                        var Pct = LiveMetricsDashboard.GetPCT(promoes);
                        // записывать
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
                    handlerLogger.Write(true, String.Format("Calculate Metrics Live History ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
        private static IEnumerable<PromoGridView> GetConstraintedQueryPromo(DatabaseContext Context, List<int> clients)
        {


            IQueryable<PromoGridView> query = Context.Set<PromoGridView>()
                .AsNoTracking()
                .Where(g => clients.Contains((int)g.ClientTreeId));
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, TPMmode.Current, FilterQueryModes.Active);
            query = query.Where(x => !x.IsOnHold);
            return query.ToList();
        }
    }
}
