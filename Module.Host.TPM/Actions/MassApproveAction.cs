using Interfaces.Implementation.Action;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                HandlerLogger.Write(true, "Approved promo: " + approvedPromo.TrimEnd(',',' '), "Message");
            }
        }

        private void ValidatePromoes(ref List<Promo> promoes)
        {
            var notValidPromoes = promoes.Where(x => x.PromoStatus.SystemName != "OnApproval").ToList();
            if (notValidPromoes.Any())
            {
                promoes = promoes.Where(x => !notValidPromoes.Any(p => p.Id == x.Id)).ToList();
                string message = "Promoes in wrong status: " + String.Join(", ", notValidPromoes.Select(x=>x.Number).ToArray());

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
    }
}
