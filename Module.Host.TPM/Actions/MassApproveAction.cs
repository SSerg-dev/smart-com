using Core.Security.Models;
using Interfaces.Implementation.Action;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    class MassApproveAction : BaseAction
    {
        private List<string> PromoesList { get; }
        private LogWriter HandlerLogger { get; }
        private Guid UserId { get; }
        private Guid RoleId { get; }
        public string HandlerStatus { get; private set; }

        public MassApproveAction(string promoNumbers, LogWriter Logger, Guid userId, Guid roleId)
        {
            PromoesList = promoNumbers.Split(',').ToList();
            HandlerLogger = Logger;
            UserId = userId;
            RoleId = roleId;
        }

        public override void Execute()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var promoList = context.Set<Promo>().Where(x => !x.Disabled && PromoesList.Contains(x.Number.ToString())).ToList();

                ValidatePromoes(ref promoList);
                TryToApprove(promoList, context);
            }
        }

        private void TryToApprove(List<Promo> promoList, DatabaseContext context)
        {
            var role = context.Set<Role>().Where(x => x.Id == RoleId && !x.Disabled).Select(x => x.SystemName).FirstOrDefault();
            string message, approvedPromo = "";

            for (var i = 0; i < promoList.Count(); i++)
            {
                var promo = promoList[i];
                PromoStateContext promoStateContext = GetPromoStateContext(context, ref promo);
                promoStateContext.ChangeState(promo, role, out message);

                if (promo.PromoStatusId == context.Set<PromoStatus>().FirstOrDefault(s => s.SystemName == "Approved" && !s.Disabled).Id)
                {
                    DeleteChildPromoes(promo.Id, getUserInfo(UserId), out message, context);
                }
                if (!String.IsNullOrEmpty(message))
                {
                    HandlerLogger.Write(true, promo.Number.ToString() + " " + message, "Error");
                    HandlerStatus = "HasErrors";
                }
                else
                {
                    approvedPromo += promo.Number.ToString() + ", ";
                }
            }

            context.SaveChanges();

            if (!String.IsNullOrEmpty(approvedPromo))
            {
                HandlerLogger.Write(true, "Approved promo: " + approvedPromo.TrimEnd(',', ' '), "Message");
            }
        }

        private void ValidatePromoes(ref List<Promo> promoes)
        {
            var notValidPromoes = promoes.Where(x => x.PromoStatus.SystemName != "OnApproval").ToList();
            if (notValidPromoes.Any())
            {
                promoes = promoes.Where(x => !notValidPromoes.Any(p => p.Id == x.Id)).ToList();
                string message = "Promoes in wrong status: " + String.Join(", ", notValidPromoes.Select(x => x.Number).ToArray());

                HandlerLogger.Write(true, message, "Error");
                HandlerStatus = "HasErrors";
            }
        }

        private PromoStateContext GetPromoStateContext(DatabaseContext context, ref Promo promo)
        {
            //Переводим в статус и делаем копию промо из-за особой механики работы изменения статусов
            Promo promoCopy = new Promo(promo);
            var approvedStatus = context.Set<PromoStatus>().Where(x => x.SystemName == "Approved" && !x.Disabled).FirstOrDefault();
            promo.PromoStatus = approvedStatus;
            promo.PromoStatusId = approvedStatus.Id;
            return new PromoStateContext(context, promoCopy);
        }
        private void DeleteChildPromoes(Guid modelId, UserInfo user, out string childmessage, DatabaseContext context)
        {
            childmessage = string.Empty;
            var ChildPromoes = context.Set<Promo>().Where(p => p.MasterPromoId == modelId && !p.Disabled).ToList();
            var statuses = context.Set<PromoStatus>().ToList();
            var DeletedId = statuses.FirstOrDefault(s => s.SystemName == "Deleted" && !s.Disabled).Id;
            var DraftPublishedId = statuses.FirstOrDefault(s => s.SystemName == "DraftPublished" && !s.Disabled).Id;
            var OnApprovalId = statuses.FirstOrDefault(s => s.SystemName == "OnApproval" && !s.Disabled).Id;
            var ApprovedId = statuses.FirstOrDefault(s => s.SystemName == "Approved" && !s.Disabled).Id;
            List<Guid> mainPromoSupportIds = new List<Guid>();
            List<Guid> mainBTLIds = new List<Guid>();
            foreach (var ChildPromo in ChildPromoes)
            {
                if (ChildPromo.PromoStatusId != DeletedId)
                {
                    using (PromoStateContext childStateContext = new PromoStateContext(context, ChildPromo))
                    {
                        if (childStateContext.ChangeState(ChildPromo, PromoStates.Deleted, "System", out childmessage))
                        {
                            ChildPromo.DeletedDate = DateTime.Now;
                            ChildPromo.Disabled = true;
                            ChildPromo.PromoStatusId = statuses.FirstOrDefault(e => e.SystemName == "Deleted").Id;

                            List<PromoProduct> promoProductToDeleteList = context.Set<PromoProduct>().Where(x => x.PromoId == ChildPromo.Id && !x.Disabled).ToList();
                            foreach (PromoProduct promoProduct in promoProductToDeleteList)
                            {
                                promoProduct.DeletedDate = System.DateTime.Now;
                                promoProduct.Disabled = true;
                            }
                            ChildPromo.NeedRecountUplift = true;
                            //необходимо удалить все коррекции
                            var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
                            List<PromoProductsCorrection> promoProductCorrectionToDeleteList = context.Set<PromoProductsCorrection>()
                                .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
                            foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                            {
                                promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                                promoProductsCorrection.Disabled = true;
                                promoProductsCorrection.UserId = (Guid)user.Id;
                                promoProductsCorrection.UserName = user.Login;
                            }
                            // удалить дочерние промо
                            var PromoesUnlink = context.Set<Promo>().Where(p => p.MasterPromoId == ChildPromo.Id && !p.Disabled).ToList();
                            foreach (var childpromo in PromoesUnlink)
                            {
                                childpromo.MasterPromoId = null;
                            }
                            

                            //при сбросе статуса в Draft необходимо отвязать бюджеты от промо и пересчитать эти бюджеты
                            List<Guid> promoSupportIds = PromoCalculateHelper.DetachPromoSupport(ChildPromo, context);
                            foreach (var psId in promoSupportIds)
                            {
                                if (!mainPromoSupportIds.Contains(psId))
                                {
                                    mainPromoSupportIds.Add(psId);
                                }
                            }
                            BTLPromo btlPromo = context.Set<BTLPromo>().Where(x => !x.Disabled && x.PromoId == ChildPromo.Id).FirstOrDefault();
                            if (btlPromo != null)
                            {
                                btlPromo.DeletedDate = DateTimeOffset.UtcNow;
                                btlPromo.Disabled = true;
                                mainBTLIds.Add(btlPromo.BTLId);
                            }

                            //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                            if (ChildPromo.InOut.HasValue && ChildPromo.InOut.Value)
                            {
                                PromoHelper.DisableIncrementalPromo(context, ChildPromo);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            if (mainPromoSupportIds.Count() > 0)
            {
                PromoCalculateHelper.CalculateBudgetsCreateTask(mainPromoSupportIds, null, null, context);
            }
            foreach (Guid btlId in mainBTLIds)
            {
                PromoCalculateHelper.CalculateBTLBudgetsCreateTask(btlId.ToString(), null, null, context, safe: true);
            }
            context.SaveChanges();
        }
        private UserInfo getUserInfo(Guid userId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId && !u.Disabled);
                return new UserInfo(null)
                {
                    Id = user != null ? user.Id : (Guid?)null,
                    Login = user.Name
                };
            }
        }
    }
}
