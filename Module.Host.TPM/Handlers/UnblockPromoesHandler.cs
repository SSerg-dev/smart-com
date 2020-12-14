using Core.Dependency;
using Core.Settings;
using Looper;
using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.LogWriter;
using LoopHandler = Persist.Model.LoopHandler;

namespace Module.Host.TPM.Handlers
{
    public class UnblockPromoesHandler : BaseHandler
    {
        private ISettingsManager SettinsManager
        {
            get
            {
                return (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            }
        }
        private int UnblockPromoHours
        {
            get
            {
                var defaultLoopHandlerInProgressOldHours = 6;
                return this.SettinsManager.GetSetting<int>("UNBLOCK_PROMO_HOURS", defaultLoopHandlerInProgressOldHours);
            }
        }

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            var stopWatch = Stopwatch.StartNew();
            var handlerLogger = new LogWriter(info.HandlerId.ToString());
            var databaseContext = new DatabaseContext();

            handlerLogger.Write(true, $"The {nameof(UnblockPromoesHandler)} task start at {DateTimeOffset.Now}.", "Message");

            try
            {
                var blockedPromoes = this.GetBlockedPromoes(databaseContext).Where(x => x.CreateDate.AddHours(this.UnblockPromoHours) < DateTimeOffset.Now);
                if (blockedPromoes.Any())
                {
                    var realBlockedPromoesForUnblocking = this.GetRealBlockedPromoes(databaseContext, blockedPromoes);
                    this.UnblockPromoes(databaseContext, blockedPromoes);
                    handlerLogger.Write(true, $"Promoes were unblocked: {String.Join(", ", realBlockedPromoesForUnblocking.Select(x => x.Number))}", "Message");
                }
            }
            catch (Exception exception)
            {
                data.SetValue<bool>("HasErrors", true);
                handlerLogger.Write(true, exception.Message, "Error");
            }
            finally
            {
                if (databaseContext != null)
                {
                    databaseContext.SaveChanges();

                    var blockedPromoes = this.GetBlockedPromoes(databaseContext);
                    if (blockedPromoes.Any())
                    {
                        var realBlockedPromoes = this.GetRealBlockedPromoes(databaseContext, blockedPromoes);
                        //handlerLogger.Write(true, $"Promoes are blocked: {String.Join(", ", realBlockedPromoes.Select(x => x.Number))}", "Message");
                        handlerLogger.Write(true, $"Blocked promoes still exist.", "Message");
                    }
                    else
                    {
                        handlerLogger.Write(true, $"No blocked promoes.", "Message");
                    }

                    ((IDisposable)databaseContext).Dispose();
                }

                stopWatch.Stop();
                handlerLogger.Write(true, $"The {nameof(UnblockPromoesHandler)} task complete at {DateTimeOffset.Now}. " + $"Duration: {stopWatch.Elapsed.ToString()}", "Message");
                handlerLogger.UploadToBlob();
            }
        }

        private IEnumerable<BlockedPromoSimple> GetBlockedPromoes(DatabaseContext databaseContext)
        {
            var blockedPromoes = databaseContext.Set<BlockedPromo>().Where(x => x.Disabled == false);
            return blockedPromoes.Select(x => new BlockedPromoSimple { Id = x.Id, PromoId = x.PromoId, CreateDate = x.CreateDate }).ToList();
        }

        private IEnumerable<PromoSimple> GetRealBlockedPromoes(DatabaseContext databaseContext, IEnumerable<BlockedPromoSimple> blockedPromoes)
        {
            var realBlockedPromoes = databaseContext.Set<Promo>().Select(x => new PromoSimple { Id = x.Id, Number = x.Number }).ToList().Where(x => blockedPromoes.Any(y => y.PromoId == x.Id));
            return realBlockedPromoes;
        }

        private void UnblockPromoes(DatabaseContext databaseContext, IEnumerable<BlockedPromoSimple> blockedPromoes)
        {
            if (blockedPromoes.Any())
            {
                var concurentBag = new ConcurrentBag<string>();
                Parallel.ForEach(blockedPromoes, blockedPromo =>
                {
                    concurentBag.Add($"UPDATE [DefaultSchemaSetting].[{nameof(BlockedPromo)}] SET [{nameof(BlockedPromo.Disabled)}] = 1, [{nameof(BlockedPromo.DeletedDate)}] = '{DateTimeOffset.Now}' WHERE [Id] = '{blockedPromo.Id}';");
                });
                var updateScript = String.Join("\n", concurentBag);
                databaseContext.ExecuteSqlCommand(updateScript);
            }
        }
    }

    class PromoSimple
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
    }

    class BlockedPromoSimple
    {
        public Guid Id { get; set; }
        public Guid PromoId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
