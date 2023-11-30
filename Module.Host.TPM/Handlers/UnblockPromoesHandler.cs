using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Utility.LogWriter;

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
            Stopwatch stopWatch = Stopwatch.StartNew();
            LogWriter handlerLogger = new LogWriter(info.HandlerId.ToString());
            DatabaseContext databaseContext = new DatabaseContext();

            handlerLogger.Write(true, $"The {nameof(UnblockPromoesHandler)} task start at {DateTimeOffset.Now}.", "Message");

            try
            {
                List<BlockedPromo> blockedPromoes = GetBlockedPromoes(databaseContext).Where(x => x.CreateDate.AddHours(UnblockPromoHours) < DateTimeOffset.Now).ToList();
                if (blockedPromoes.Any())
                {
                    var realBlockedPromoesForUnblocking = this.GetRealBlockedPromoes(databaseContext, blockedPromoes);
                    UnblockPromoes(databaseContext, blockedPromoes);
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

        private List<BlockedPromo> GetBlockedPromoes(DatabaseContext databaseContext)
        {
            return databaseContext.Set<BlockedPromo>().Include(g => g.PromoBlockedStatus).Where(x => x.Disabled == false).ToList();
        }

        private IEnumerable<PromoSimple> GetRealBlockedPromoes(DatabaseContext databaseContext, List<BlockedPromo> blockedPromoes)
        {
            var realBlockedPromoes = databaseContext.Set<Promo>().Select(x => new PromoSimple { Id = x.Id, Number = x.Number }).ToList().Where(x => blockedPromoes.Any(y => y.PromoBlockedStatusId == x.Id));
            return realBlockedPromoes;
        }

        private void UnblockPromoes(DatabaseContext databaseContext, List<BlockedPromo> blockedPromoes)
        {
            if (blockedPromoes.Any())
            {
                foreach (BlockedPromo blocked in blockedPromoes)
                {
                    blocked.Disabled = true;
                    blocked.DeletedDate = DateTimeOffset.Now;
                    blocked.PromoBlockedStatus.Blocked = false;
                }
                databaseContext.SaveChanges();
            }
        }
    }

    class PromoSimple
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
    }
}
