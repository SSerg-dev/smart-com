using Interfaces.Implementation.Action;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    class ReapprovingAction : BaseAction
    {
        private List<string> Promos { get; }

        public ReapprovingAction(List<string> promos)
        {
            Promos = promos;
        }

        public override void Execute()
        {
            PromoStatusChange();
        }

        private void PromoStatusChange()
        {
            if(Promos != null && Promos?.Count > 0)
            {
                string[] canBeReturnedToOnApproval = { "OnApproval", "Approved", "Planned" };

                using (DatabaseContext context = new DatabaseContext())
                {
                    var promos = context.Set<Promo>().Where(p => Promos.Contains(p.Number.ToString()));
                    foreach (var promo in promos)
                    {
                        // возврат в статус OnApproval при изменении набора продуктов(с проверкой NoNego)
                        if (canBeReturnedToOnApproval.Contains(promo.PromoStatus.SystemName))
                        {
                            PromoStatus draftPublished = context.Set<PromoStatus>()
                                                                .First(x => x.SystemName
                                                                .ToLower() == "draftpublished" && !x.Disabled);
                            promo.PromoStatus = draftPublished;
                            promo.PromoStatusId = draftPublished.Id;
                            promo.IsAutomaticallyApproved = false;
                            promo.IsCMManagerApproved = false;
                            promo.IsDemandFinanceApproved = false;
                            promo.IsDemandPlanningApproved = false;
                            PromoStateContext promoStateContext = new PromoStateContext(context, promo);

                            PromoStatus onApproval = context.Set<PromoStatus>()
                                                            .First(x => x.SystemName
                                                            .ToLower() == "onapproval" && !x.Disabled);
                            promo.PromoStatus = onApproval;
                            promo.PromoStatusId = onApproval.Id;

                            string statusChangeError;
                            var status = promoStateContext.ChangeState(promo, "System", out statusChangeError);
                            if (status)
                            {
                                //Сохранение изменения статуса
                                var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                promoStatusChange.PromoId = promo.Id;
                                promoStatusChange.StatusId = promo.PromoStatusId;
                                promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(promoStatusChange);
                                Results.Add(String.Format("Status of Promo {0} changed to {1}", promo.Number, promo.PromoStatus.Name), "");
                            }

                            if (statusChangeError != null && statusChangeError != string.Empty)
                            {
                                string logLine = String.Format("Error while changing status of Promo {0}: {1}", promo.Number, statusChangeError);
                                Errors.Add(logLine);
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
            else if(Promos == null)
            {
                Errors.Add("Unable to find Promoes for reapproval");
            }
            else
            {
                Warnings.Add("No promo for reapproval");
            }          
        }      
    }
}
