using Core.Dependency;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    /// <summary>
    /// Класс для автоматического сброса статуса из Draft(Published) в Draft
    /// </summary>
    public class AutoResetPromoAction : BaseAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    //срок(в неделях) до начала отгрузок, по достижении которого, промо из статуса Draft(Published) автоматически сбрасываются в Draft
                    int autoResetPromoPeriod = settingsManager.GetSetting<int>("AUTO_RESET_PROMO_PERIOD_WEEKS", 1);
                    DateTimeOffset today = DateTimeOffset.Now;
                    PromoStatus draftPublishedStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == StateNames.DRAFT_PUBLISHED);
                    List<Promo> promoToDraft = context.Set<Promo>()
                        .Where(x => x.PromoStatusId == draftPublishedStatus.Id && !x.Disabled)
                        .Where(x => x.DispatchesStart.HasValue &&  DbFunctions.DiffDays(DbFunctions.CreateDateTime(x.DispatchesStart.Value.Year, x.DispatchesStart.Value.Month, x.DispatchesStart.Value.Day, 0, 0, 0 ),
                                                                                        DbFunctions.CreateDateTime(today.Year, today.Month, today.Day, 0, 0, 0)) == -autoResetPromoPeriod * 7).ToList();

                    string message;
                    List<Guid> mainPromoSupportIds = new List<Guid>();
                    foreach (Promo promo in promoToDraft)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            promoStateContext.ChangeState(promo, PromoStates.Draft, "System", out message);
                        }

                        List<PromoProduct> promoProductToDeleteList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        foreach (PromoProduct promoProduct in promoProductToDeleteList)
                        {
                            promoProduct.DeletedDate = System.DateTime.Now;
                            promoProduct.Disabled = true;
                        }
                        //при сбросе статуса в Draft необходимо отвязать бюджеты от промо и пересчитать эти бюджеты
                        List<Guid> promoSupportIds = PromoCalculateHelper.DetachPromoSupport(promo, context);
                        mainPromoSupportIds.AddRange(promoSupportIds);
                    }

                    if (mainPromoSupportIds.Count() > 0)
                    {
                        PromoCalculateHelper.CalculateBudgetsCreateTask(mainPromoSupportIds, null, null, context);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while sending a notification for Product Create Incident: {0}", e.ToString());
                Errors.Add(msg);
            }
        }
    }
}
