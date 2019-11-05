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
    /// Класс для автоматического сброса статуса промо при различных условиях
    /// </summary>
    public class AutoResetPromoAction : BaseAction
    {
        public override void Execute()
        {
            try
            {
				ResetOnApproval();
				ResetDraftPublished();
				ResetApproved();
			}
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while reseting promo status", e.ToString());
                Errors.Add(msg);
            }
        }

		/// <summary>
		/// Сброс статуса промо из DraftPublished в Draft в день начала промо
		/// </summary>
		private void ResetDraftPublished()
		{
			using (DatabaseContext context = new DatabaseContext())
			{
			    DateTimeOffset today = DateTimeOffset.Now;
			    PromoStatus draftPublishedStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == StateNames.DRAFT_PUBLISHED);
                List<Promo> promoToDraft = context.Set<Promo>()
                    .Where(x => x.PromoStatusId == draftPublishedStatus.Id && !x.Disabled).ToList();
				    //.Where(x => x.StartDate.HasValue && DbFunctions.DiffDays(DbFunctions.CreateDateTime(x.StartDate.Value.Year, x.StartDate.Value.Month, x.StartDate.Value.Day, 0, 0, 0), DbFunctions.CreateDateTime(today.Year, today.Month, today.Day, 0, 0, 0)) >= 0).ToList();

			    string message;
			    List<Guid> mainPromoSupportIds = new List<Guid>();
			    foreach (Promo promo in promoToDraft)
			    {
                    if (promo.StartDate.HasValue && promo.StartDate.Value <= today)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            var status = promoStateContext.ChangeState(promo, PromoStates.Draft, "System", out message);
                            if (status)
                            {
                                //Сохранение изменения статуса
                                var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                promoStatusChange.PromoId = promo.Id;
                                promoStatusChange.StatusId = promo.PromoStatusId;
                                promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(promoStatusChange);
                            }
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

                        //необходимо сбросить наименование Promo до "Unpublish Promo"
                        promo.Name = "Unpublish Promo";
                    }
			    }

			    if (mainPromoSupportIds.Count() > 0)
			    {
				    PromoCalculateHelper.CalculateBudgetsCreateTask(mainPromoSupportIds, null, null, context);
			    }

			    context.SaveChanges();
			}
		}

		/// <summary>
		/// Сброс статуса промо из OnApproval в DraftPublished за 24 часа до dispatch date
		/// </summary>
		private void ResetOnApproval()
		{
			using (DatabaseContext context = new DatabaseContext())
			{
				DateTimeOffset today = DateTimeOffset.Now;
                DateTimeOffset dayBeforeToday = today.AddDays(1);
				PromoStatus onApprovalStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == StateNames.ON_APPROVAL);
                List<Promo> promoToDraftPublished = context.Set<Promo>()
                    .Where(x => x.PromoStatusId == onApprovalStatus.Id && !x.Disabled).ToList();
					//.Where(x => x.DispatchesStart.HasValue && DbFunctions.DiffDays(DbFunctions.CreateDateTime(x.DispatchesStart.Value.Year, x.DispatchesStart.Value.Month, x.DispatchesStart.Value.Day, 0, 0, 0), DbFunctions.CreateDateTime(today.Year, today.Month, today.Day, 0, 0, 0)) >= -1).ToList();

				string message;
				foreach (Promo promo in promoToDraftPublished)
				{
                    if (promo.DispatchesStart.HasValue && promo.DispatchesStart.Value <= dayBeforeToday)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            var status = promoStateContext.ChangeState(promo, PromoStates.DraftPublished, "System", out message);
                            if (status)
                            {
                                //Сохранение изменения статуса
                                var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                promoStatusChange.PromoId = promo.Id;
                                promoStatusChange.StatusId = promo.PromoStatusId;
                                promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(promoStatusChange);
                            }
                        }
                    }
				}

				context.SaveChanges();
			}
		}

		/// <summary>
		/// Сброс статуса промо из Approved в Cancelled при наступлении start date
		/// </summary>
		private void ResetApproved()
		{
			using (DatabaseContext context = new DatabaseContext())
			{
				DateTimeOffset today = DateTimeOffset.Now;
				PromoStatus approvedStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == StateNames.APPROVED);
                List<Promo> promoToCancelled = context.Set<Promo>()
                    .Where(x => x.PromoStatusId == approvedStatus.Id && !x.Disabled).ToList();
					//.Where(x => x.StartDate.HasValue && DbFunctions.DiffDays(DbFunctions.CreateDateTime(x.StartDate.Value.Year, x.StartDate.Value.Month, x.StartDate.Value.Day, 0, 0, 0), DbFunctions.CreateDateTime(today.Year, today.Month, today.Day, 0, 0, 0)) >= 0).ToList();

				string message;
				foreach (Promo promo in promoToCancelled)
				{
                    if (promo.StartDate.HasValue && promo.StartDate.Value <= today)
                    {
                        using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
                        {
                            var status = promoStateContext.ChangeState(promo, PromoStates.Cancelled, "System", out message);
                            if (status)
                            {
                                //Сохранение изменения статуса
                                var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                promoStatusChange.PromoId = promo.Id;
                                promoStatusChange.StatusId = promo.PromoStatusId;
                                promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                context.Set<PromoStatusChange>().Add(promoStatusChange);
								context.Set<PromoCancelledIncident>().Add(new PromoCancelledIncident()
								{
									CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
									Promo = promo,
									PromoId = promo.Id
								});
                            }
                        }
                    }
				}

				context.SaveChanges();
			}
		}
	}
}
