using AutoMapper;
using Core.Data;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Core.MarsCalendar;
using Utility;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.DTO;
using Core.Settings;
using Module.Persist.TPM.PromoStateControl;
using System.Web.Http.Results;
using System.IO;
using Persist.ScriptGenerator.Filter;
using System.Net.Http.Headers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Frontend.TPM.Util;

namespace Module.Frontend.TPM.Controllers {
    public class PromoesController : EFContextController {

        private readonly IAuthorizationManager authorizationManager;

        public PromoesController() { }

        public PromoesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Promo> GetConstraintedQuery(bool canChangeStateOnly = false) {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<Promo> query = Context.Set<Promo>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<Promo> GetPromo([FromODataUri] Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetPromoes(bool canChangeStateOnly = false) {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetCanChangeStatePromoes(bool canChangeStateOnly = false) {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<Promo> patch) {
            var model = Context.Set<Promo>().Find(key);
            if (model == null) {
                return NotFound();
            }
            patch.Put(model);

            try {
                //Установка полей по дереву ProductTree
                //SetPromoByProductTree(model);
                //Установка дат в Mars формате
                SetPromoMarsDates(model);
                //Установка полей по дереву ClientTree
                SetPromoByClientTree(model);

                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(Promo model) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
                model.DispatchesStart = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesStart);
                model.DispatchesEnd = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesEnd);

                DateTimeOffset? ChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.Now);

                if (model.EventId == null) {
                    Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == "Standard promo");
                    if (promoEvent == null) {
                        return InternalServerError(new Exception("Event 'Standard promo' not found"));
                    }

                    model.EventId = promoEvent.Id;
                    model.EventName = promoEvent.Name;
                } else {
                    Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Id == model.EventId);
                    if (promoEvent == null) {
                        return InternalServerError(new Exception("Event not found"));
                    }
                    model.EventName = promoEvent.Name;
                }

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, null);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status) {
                    return InternalServerError(new Exception(message));
                }

                Promo proxy = Context.Set<Promo>().Create<Promo>();
                Promo result = (Promo) Mapper.Map(model, proxy, typeof(Promo), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

                if (result.CreatorId == null) {
                    result.CreatorId = user.Id;
                    result.CreatorLogin = user.Login;
                }

                Context.Set<Promo>().Add(result);
                Context.SaveChanges();
				// Добавление продуктов
				bool isSubrangeChanged = false;
                List<PromoProductTree> promoProductTrees = AddProductTrees(model.ProductTreeObjectIds, result, out isSubrangeChanged);

                //Установка полей по дереву ProductTree
                SetPromoByProductTree(result, promoProductTrees);
                //Установка дат в Mars формате
                SetPromoMarsDates(result);
                //Установка полей по дереву ClientTree
                SetPromoByClientTree(result);
                //Установка механик
                SetMechanic(result);
                SetMechanicIA(result);

                //Установка начального статуса
                PromoStatusChange psc = Context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                psc.PromoId = result.Id;
                psc.StatusId = result.PromoStatusId;
                psc.UserId = (Guid) user.Id;
                psc.RoleId = (Guid) user.GetCurrentRole().Id;
                psc.Date = DateTimeOffset.UtcNow;
                Context.Set<PromoStatusChange>().Add(psc);

                //Установка времени последнгего присвоения статуса Approved
                if (result.PromoStatus != null && result.PromoStatus.SystemName == "Approved") {
                    result.LastApprovedDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.UtcNow);
                }


                // Для draft не проверяем и не считаем && если у промо есть признак InOut, то Uplift считать не нужно.
                if (result.PromoStatus.SystemName.ToLower() != "draft") {
                    // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)
                    List<Product> filteredProducts; // продукты, подобранные по фильтрам
                    CheckSupportInfo(result, promoProductTrees, out filteredProducts);
                    //создание отложенной задачи, выполняющей подбор аплифта и расчет параметров
                    CalculatePromo(result, false, false, true);
                }
                else
                {
                    // Добавить запись в таблицу PromoProduct при сохранении.
                    string error;
                    PlanProductParametersCalculation.SetPromoProduct(Context.Set<Promo>().First(x => x.Number == result.Number).Id, Context, out error, true, promoProductTrees);
                    // Создаём инцидент для draft сразу
                    PromoHelper.WritePromoDemandChangeIncident(Context, result);
                }

                result.LastChangedDate = ChangedDate;
                Context.SaveChanges();

                return Created(result);
            } catch (Exception e) {
                return InternalServerError(e);
            }

        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<Promo> patch) {
            try {
                var model = Context.Set<Promo>().Find(key);
				var isSubrangeChanged = false;

                if (model == null) {
                    return NotFound();
                }
                DateTimeOffset? ChangedDate = DateTimeOffset.UtcNow;
                
                Promo promoCopy = new Promo(model);
                patch.Patch(model);
                if (!String.IsNullOrEmpty(model.AdditionalUserTimestamp))
                    FixateTempPromoProductsCorrections(model.Id, model.AdditionalUserTimestamp);
            
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
                model.DispatchesStart = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesStart);
                model.DispatchesEnd = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesEnd);

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;
                bool needToCreateDemandIncident = false;

                string message;
                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);
                if (!status) {
                    return InternalServerError(new Exception(message));
                }

                String statusName = Context.Set<PromoStatus>().FirstOrDefault(s => s.Id == model.PromoStatusId && !s.Disabled).SystemName;
                String prevStatusName = Context.Set<PromoStatus>().FirstOrDefault(s => s.Id == promoCopy.PromoStatusId && !s.Disabled).SystemName;
                bool needDetachPromoSupport = statusName.ToLower() == "draft" && prevStatusName.ToLower() == "draftpublished";

                if (statusName.ToLower() == "started"
                        || statusName.ToLower() == "finished"
                        || statusName.ToLower() == "closed")
                {
                    // из-за проблемы с поясами отсекаем его
                    // P.S. вот тут как бы очень не удобно использовать Nullable тип (и по факту быть не должно)
                    // Когда ветка TimeZone будет принята, то такой подход будет не нужен
                    DateTime? newStartDate = model.StartDate.HasValue ? model.StartDate.Value.DateTime : new DateTime?();
                    DateTime? oldStartDate = promoCopy.StartDate.HasValue ? promoCopy.StartDate.Value.DateTime : new DateTime?();
                    DateTime? newEndDate = model.EndDate.HasValue ? model.EndDate.Value.DateTime : new DateTime?();
                    DateTime? oldEndDate = promoCopy.EndDate.HasValue ? promoCopy.EndDate.Value.DateTime : new DateTime?();
                    DateTime? newDispatchesStart = model.DispatchesStart.HasValue ? model.DispatchesStart.Value.DateTime : new DateTime?();
                    DateTime? oldDispatchesStart = promoCopy.DispatchesStart.HasValue ? promoCopy.DispatchesStart.Value.DateTime : new DateTime?();
                    DateTime? newDispatchesEnd = model.DispatchesEnd.HasValue ? model.DispatchesEnd.Value.DateTime : new DateTime?();
                    DateTime? oldDispatchesEnd = promoCopy.DispatchesEnd.HasValue ? promoCopy.DispatchesEnd.Value.DateTime : new DateTime?();

                    if
                    (
                        model.ClientHierarchy != promoCopy.ClientHierarchy
                        || model.ClientTreeId != promoCopy.ClientTreeId
                        || model.ProductHierarchy != promoCopy.ProductHierarchy
                        || model.MarsMechanic != model.MarsMechanic
                        || model.MarsMechanicId != promoCopy.MarsMechanicId
                        || model.MarsMechanicTypeId != promoCopy.MarsMechanicTypeId
                        || model.PlanInstoreMechanic != promoCopy.PlanInstoreMechanic
                        || model.PlanInstoreMechanicId != promoCopy.PlanInstoreMechanicId
                        || model.PlanInstoreMechanicTypeId != promoCopy.PlanInstoreMechanicTypeId
                        || newStartDate != oldStartDate
                        || newEndDate != oldEndDate
                        || newDispatchesStart != oldDispatchesStart
                        || newDispatchesEnd != oldDispatchesEnd
                        || model.EventId != promoCopy.EventId
                        || model.CalendarPriority != promoCopy.CalendarPriority
                    )
                    {
                        //Для избегания ошибок, когда скидка = 0
                        double? d1 = model.PlanInstoreMechanicDiscount,
                            d2 = promoCopy.PlanInstoreMechanicDiscount;
                        if ((d1 == 0) || (d1 == null)) d1 = 0;
                        if ((d2 == 0) || (d2 == null)) d2 = 0;
                        if (d2 != d1)
                        {
                            d1 = model.MarsMechanicDiscount;
                            d2 = promoCopy.MarsMechanicDiscount;
                            if ((d1 == 0) || (d1 == null)) d1 = 0;
                            if ((d2 == 0) || (d2 == null)) d2 = 0;
                            if (d2 != d1)
                                return InternalServerError(new Exception("Promo Basic can't be changed"));
                        }
                    }
                }

				//Сохранение изменения статуса.
				if (promoCopy.PromoStatusId != model.PromoStatusId
                    || promoCopy.IsDemandFinanceApproved != model.IsDemandFinanceApproved
                    || promoCopy.IsCMManagerApproved != model.IsCMManagerApproved
                    || promoCopy.IsDemandPlanningApproved != model.IsDemandPlanningApproved)
                {
                    PromoStatusChange psc = Context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                    psc.PromoId = model.Id;
                    psc.StatusId = model.PromoStatusId;
                    psc.UserId = (Guid)user.Id;
                    psc.RoleId = (Guid)user.GetCurrentRole().Id;
                    psc.Date = DateTimeOffset.UtcNow;
                    Context.Set<PromoStatusChange>().Add(psc);

					//Установка времени последнего присвоения статуса Approved
					if (statusName != null && statusName.ToLower() == "approved")
					{
						model.LastApprovedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
					}
				}

                SetBrandTechIdPromo(model);

				if (statusName.ToLower() != "cancelled" && (statusName.ToLower() != "finished" || statusName.ToLower() == "finished" && userRole == "SupportAdministrator"))
                {
                    List<PromoProductTree> promoProductTrees = new List<PromoProductTree>();
                    if (!model.LoadFromTLC)
                    {
                        // Добавление продуктов                
                        promoProductTrees = AddProductTrees(model.ProductTreeObjectIds, model, out isSubrangeChanged);
                    }

                    bool needRecalculatePromo = NeedRecalculatePromo(model, promoCopy);
                    bool needResetUpliftCorrections = false;
                    if (model.NeedRecountUplift != null && promoCopy.NeedRecountUplift != null && model.NeedRecountUplift != promoCopy.NeedRecountUplift)
                    {
                        needResetUpliftCorrections = true;
                    }
                    model.AdditionalUserTimestamp = null;

                    //для ускорения перехода в следующий статус (если нет изменений параметров промо, то пропускаем следующие действия)
                    if (needRecalculatePromo || statusName.ToLower() == "draft")
                    {
                        //Установка полей по дереву ProductTree
                        SetPromoByProductTree(model, promoProductTrees);
                        //Установка дат в Mars формате
                        SetPromoMarsDates(model);
                        //Установка полей по дереву ClientTree
                        SetPromoByClientTree(model);
                        //Установка механик
                        SetMechanic(model);
                        SetMechanicIA(model);

                        if (statusName.ToLower() == "draft")
                        {
                            string error;
                            // Прикрепление продуктов
                            PlanProductParametersCalculation.SetPromoProduct(Context.Set<Promo>().First(x => x.Number == model.Number).Id, Context, out error, true, promoProductTrees);
                        }
                    }

                    if (needRecalculatePromo || patch.GetChangedPropertyNames().Contains("PromoStatusId"))
                    {
                        model.LastChangedDate = ChangedDate;
                        if ((promoCopy.PlanPromoUpliftPercent != null && model.PlanPromoUpliftPercent != null
                            && Math.Round(promoCopy.PlanPromoUpliftPercent.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(model.PlanPromoUpliftPercent.Value, 2, MidpointRounding.AwayFromZero)))
                        {
                            model.LastChangedDateDemand = ChangedDate;
                            model.LastChangedDateFinance = ChangedDate;
                        }
                    }

                    if (model.EventId == null)
                    {
                        Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == "Standard promo");
                        if (promoEvent == null)
                        {
                            return InternalServerError(new Exception("Event 'Standard promo' not found"));
                        }

                        model.EventId = promoEvent.Id;
                        model.EventName = promoEvent.Name;
                    }
                    else
                    {
                        Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Id == model.EventId);
                        if (promoEvent == null) {
                            return InternalServerError(new Exception("Event not found"));
                        }
                        model.EventName = promoEvent.Name;
                    }

                    // для draft не проверяем и не считаем
                    if (statusName.ToLower() != "draft") {
                        // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)
                        List<Product> filteredProducts; // продукты, подобранные по фильтрам
                        CheckSupportInfo(model, promoProductTrees, out filteredProducts);

                        // в статусе On Approval проверяем изменился ли список фильтруемых продуктов (и соответсвенно если узел остался тот же)
                        // проверяем предыдущий на случай, когда утвердил последний и промо перешло в Approved
                        bool changedProducts = false;
                        if ((model.ProductHierarchy == promoCopy.ProductHierarchy) &&
                            (model.PromoStatus.SystemName.ToLower() == "onapproval" || promoCopy.PromoStatus.SystemName.ToLower() == "onapproval"))
                        {
                            List<Product> resultProductList = null;
                            if (!model.InOut.HasValue || !model.InOut.Value)
                            {
                                List<string> eanPCs = PlanProductParametersCalculation.GetProductListFromAssortmentMatrix(model, Context);
                                filteredProducts = PlanProductParametersCalculation.GetCheckedProducts(Context, model);
                                resultProductList = PlanProductParametersCalculation.GetResultProducts(filteredProducts, eanPCs, model, Context);
                            }
                            else 
                            {
                                resultProductList = PlanProductParametersCalculation.GetCheckedProducts(Context, model);
                            }

                            changedProducts = CheckChangesInProductList(model, resultProductList);
                        }

                        //создание отложенной задачи, выполняющей переподбор аплифта и перерассчет параметров                    
                        //если у промо есть признак InOut, то Uplift считать не нужно.
                        if ((changedProducts || needRecalculatePromo) && userRole != "SupportAdministrator") {
                            // если меняем длительность промо, то пересчитываем Marketing TI
                            bool needCalculatePlanMarketingTI = promoCopy.StartDate != model.StartDate || promoCopy.EndDate != model.EndDate;
                            needToCreateDemandIncident = PromoHelper.CheckCreateIncidentCondition(promoCopy, model, patch, isSubrangeChanged);

                            CalculatePromo(model, needCalculatePlanMarketingTI, needResetUpliftCorrections, false, needToCreateDemandIncident, promoCopy.MarsMechanic.Name, promoCopy.MarsMechanicDiscount, promoCopy.DispatchesStart, promoCopy.PlanPromoUpliftPercent, promoCopy.PlanPromoIncrementalLSV); //TODO: Задача создаётся раньше чем сохраняются изменения промо.

                        }                                                        
                        //Сначала проверять заблокированно ли промо, если нет сохранять промо, затем сохранять задачу
                    }
                    else if (needDetachPromoSupport && userRole != "SupportAdministrator")
                    {
                        //UPD: Теперь не надо
                        //UPD(17.09.19): A теперь надо !
                        List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
                        foreach (PromoProduct promoProduct in promoProductToDeleteList)
                        {
                            promoProduct.DeletedDate = System.DateTime.Now;
                            promoProduct.Disabled = true;
                        }

                        //если промо инаут, необходимо убрать записи в IncrementalPromo при сбросе статуса в Draft
                        if (model.InOut.HasValue && model.InOut.Value)
                        {
                            PromoHelper.DisableIncrementalPromo(Context, model);
                        }

                        //при сбросе статуса в Draft необходимо удалить все коррекции
                        var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
                        List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                            .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
                        foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                        {
                            promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                            promoProductsCorrection.Disabled = true;
                            promoProductsCorrection.UserId = (Guid)user.Id;
                            promoProductsCorrection.UserName = user.Login;
                        }

                        //при сбросе статуса в Draft необходимо отвязать бюджеты от промо и пересчитать эти бюджеты
                        PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                    }
                }
                else if (statusName.ToLower() != "finished")
                {
                    //при отмене промо необходимо отвязать бюджеты от промо и пересчитать эти бюджеты
                    PromoCalculateHelper.RecalculateBudgets(model, user, Context);

                    //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                    if (model.InOut.HasValue && model.InOut.Value)
                    {
                        PromoHelper.DisableIncrementalPromo(Context, model);
                    }
                }

				if (model.PromoStatus.SystemName.ToLower() == "cancelled")
				{
                    List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
                    foreach (PromoProduct promoProduct in promoProductToDeleteList)
                    {
                        promoProduct.DeletedDate = System.DateTime.Now;
                        promoProduct.Disabled = true;
                    }
                    model.NeedRecountUplift = true;
                    //необходимо удалить все коррекции
                    var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
                    List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                        .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
                    foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                    {
                        promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                        promoProductsCorrection.Disabled = true;
                        promoProductsCorrection.UserId = (Guid)user.Id;
                        promoProductsCorrection.UserName = user.Login;
                    }


                    // Создание записи инцидента отмены промо
                    Context.Set<PromoCancelledIncident>().Add(new PromoCancelledIncident() { PromoId = model.Id, CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow) });
                }

				// Проверка изменения списка выбранных продуктов
				if (model.InOutProductIds != promoCopy.InOutProductIds && prevStatusName.ToLower() != "draftpublished")
				{
					var oldIds = promoCopy.InOutProductIds.ToLower().Split(';').ToList();
					var newIds = model.InOutProductIds.ToLower().Split(';').ToList();
					var deletedIds = oldIds.Except(newIds).ToList();
					var addedIds = newIds.Except(oldIds).ToList();
					var addedZREPs = Context.Set<Product>().Where(p => addedIds.Any(id => id == p.Id.ToString().ToLower())).Select(p => p.ZREP).ToList();
					var deletedZREPs = Context.Set<Product>().Where(p => deletedIds.Any(id => id == p.Id.ToString().ToLower())).Select(p => p.ZREP).ToList();

					if (addedZREPs.Any() || deletedZREPs.Any())
					{
						Product p = Context.Set<Product>().Where(x => addedZREPs.Contains(x.ZREP) || deletedZREPs.Contains(x.ZREP)).FirstOrDefault();
						if (p != null)
						{
							ProductChangeIncident pci = new ProductChangeIncident()
							{
								Product = p,
								ProductId = p.Id,
								IsRecalculated = false,
								RecalculatedPromoId = model.Id,
								AddedProductIds = addedZREPs.Any() ? String.Join(";", addedZREPs) : null,
								ExcludedProductIds = deletedZREPs.Any() ? String.Join(";", deletedZREPs) : null,
								CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value,
								IsCreate = false,
								IsChecked = true,
							};
							Context.Set<ProductChangeIncident>().Add(pci);
						}
					}
				}
                Context.SaveChanges();

                if (!needToCreateDemandIncident)
                {
                    PromoHelper.WritePromoDemandChangeIncident(Context, model, patch, promoCopy, isSubrangeChanged);
                }

                // TODO: ПЕРЕДЕЛАТЬ, просто оставалось 15 мин до релиза
                if (message != string.Empty && userRole == "DemandPlanning" && statusName.ToLower() == "onapproval")
                {
                    return Content(HttpStatusCode.OK, message);
                }
                else
                    return Updated(model);

            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult RecalculatePromo(Guid promoId) {
            Promo promo = Context.Set<Promo>().Find(promoId);
            if (promo == null)
                return NotFound();

            using (var transaction = Context.Database.BeginTransaction()) {
                try {
                    CalculatePromo(promo, true, false);
                    transaction.Commit();
                } catch (Exception e) {
                    transaction.Rollback();
                    return InternalServerError(e);
                }
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        } 
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ResetPromo(Guid promoId)
        {
            Promo model = Context.Set<Promo>().Find(promoId);
            
            if (model == null)
                return NotFound();
            UserInfo user = authorizationManager.GetCurrentUser();

            PromoHelper.ResetPromo(Context, model, user);
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ChangeResponsible(Guid promoId,string userName)
        {
            Promo model = Context.Set<Promo>().Find(promoId); 
            if (model == null)
                return NotFound();
            string result = PromoHelper.ChangeResponsible(Context, model, userName);
            if (!String.IsNullOrEmpty(result))
            {
                return InternalServerError(new Exception(result));
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
        
        private void SetBrandTechIdPromo(Promo model) {
            if (model.ProductTreeObjectIds != null) {
                List<int> productTreeObjectIds = new List<int>();
                if (model.ProductTreeObjectIds.Length > 0) {
                    productTreeObjectIds = model.ProductTreeObjectIds.Split(';').Select(n => Int32.Parse(n)).ToList();
                }

                int objectId = productTreeObjectIds.FirstOrDefault();

                DateTime dt = DateTime.Now;
                ProductTree productTree = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == objectId);
                if (productTree != null) {
                    Guid? brandId = null;
                    Guid? technologyId = null;
                    Brand brand = null;
                    Technology technology = null;
                    bool end = false;
                    do {
                        if (productTree.Type == "Brand") {
                            brand = Context.Set<Brand>().FirstOrDefault(x => x.Name == productTree.Name);
                            if (brand != null) {
                                brandId = brand.Id;
                            }
                        }
                        if (productTree.Type == "Technology") {
                            technology = Context.Set<Technology>().FirstOrDefault(x => x.Name == productTree.Name);
                            if (technology != null) {
                                technologyId = technology.Id;
                            }
                        }
                        if (productTree.parentId != 1000000) {
                            productTree = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == productTree.parentId);
                        } else {
                            end = true;
                        }
                    } while (!end && productTree != null);

                    BrandTech brandTech = Context.Set<BrandTech>().FirstOrDefault(x => !x.Disabled && x.TechnologyId == technologyId && x.BrandId == brandId);
                    if (brandTech != null) {
                        model.BrandTechId = brandTech.Id;
                    } else {
                        model.BrandTechId = null;
                    }

                    if (brandId != null)
                    {
                        model.BrandId = brandId;
                    }

                    if (technologyId != null)
                    {
                        model.TechnologyId = technologyId;
                    }
                }
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] Guid key) {
            try {
                var model = Context.Set<Promo>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                Promo promoCopy = new Promo(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status) {
                    return InternalServerError(new Exception(message));
                }
                
                Context.SaveChanges();

                PromoHelper.WritePromoDemandChangeIncident(Context, model, true);
                PromoCalculateHelper.RecalculateBudgets(model, user, Context);

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Смена статуса промо
        /// </summary>
        /// <param name="id">Id Промо</param>
        /// <param name="promoNewStatusId">Id статуса</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ChangeStatus(Guid id, Guid promoNewStatusId) {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction()) {
                try {
                    Delta<Promo> patch = new Delta<Promo>((new Promo()).GetType(), new string[] { "PromoStatusId" });
                    patch.TrySetPropertyValue("PromoStatusId", promoNewStatusId);

                    // если возвращается Update, то всё прошло без ошибок
                    var result = Patch(id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>) {
                        transaction.Commit();
                        return Json(new { success = true });
                    } else {
                        ExceptionResult exc = result as ExceptionResult;
                        if (exc != null)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }

                } catch (Exception e) {
                    transaction.Rollback();
                    return InternalServerError(e);
                }
            }
        } 
        /// <summary>
        /// Отклонение промо
        /// </summary>
        /// <param name="rejectPromoId">Id Промо</param>
        /// <param name="rejectReasonId">Id Причины</param>
        /// <param name="rejectComment">Комментарий</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult DeclinePromo([FromODataUri] Guid rejectPromoId, [FromODataUri] Guid rejectReasonId, [FromODataUri] string rejectComment) {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction()) {
                try {
                    Promo promo = Context.Set<Promo>().Find(rejectPromoId);
                    RejectReason rejectreason = Context.Set<RejectReason>().Find(rejectReasonId);

					if (promo == null) {
						throw new Exception("Promo not found");
					} else if (rejectreason == null) {
						throw new Exception("Reject reason not found");
					}
					
					PromoStatus draftPublishedStatus = Context.Set<PromoStatus>().First(n => n.SystemName == "DraftPublished");
                    Delta<Promo> patch = new Delta<Promo>(promo.GetType(), new string[] { "PromoStatusId", "RejectReasonId" });
                    patch.TrySetPropertyValue("PromoStatusId", draftPublishedStatus.Id);
                    patch.TrySetPropertyValue("RejectReasonId", rejectReasonId);

                    // если возвращается Update, то всё прошло без ошибок
                    var result = Patch(promo.Id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>) {
                        UserInfo user = authorizationManager.GetCurrentUser();
                        // Ищем записанное изменение статуса и добавляем комментарий
                        PromoStatusChange psc = Context.Set<PromoStatusChange>().Where(n => n.PromoId == promo.Id && n.UserId == user.Id)
                            .OrderByDescending(n => n.Date).First();

                        psc.RejectReasonId = rejectreason.Id;
                        psc.Comment = rejectreason.SystemName == "Other" ? rejectComment : null;

                        // Создание инцидента для оповещения об отмене промо
						Context.Set<PromoOnRejectIncident>().Add(new PromoOnRejectIncident()
						{
							UserLogin = user.Login,
							Promo = promo,
							CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            PromoId = rejectPromoId
						});

                        Context.SaveChanges();
                        transaction.Commit();

                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = promo }));
                    } else {
                        ExceptionResult exc = result as ExceptionResult;
                        if (exc != null)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }
                } catch (Exception e) {
                    transaction.Rollback();
                    return InternalServerError(e);
                }
            }
        }

        private FilterContainer GetFilter(Delta<Promo> patch, string fieldName) {
            object fieldValue;
            if (patch.TryGetPropertyValue(fieldName, out fieldValue) && fieldValue != null) {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>((string) fieldValue);
                return result;
            } else {
                return null;
            }
        }

        private FilterContainer GetFilter(string fieldValue) {
            if (fieldValue != null) {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>(fieldValue);
                return result;
            } else {
                return null;
            }
        }

        private bool EntityExists(Guid key) {
            return Context.Set<Promo>().Count(e => e.Id == key) > 0;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Promo> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = PromoHelper.GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("Promo", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        
        /// <summary>
        /// Экспорт календаря в эксель
        /// </summary>
        /// <param name="options"></param>
        /// <param name="data">clients - список id клиентов соответствующих фильтру на клиенте, year - год</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ExportSchedule(ODataQueryOptions<Promo> options, ODataActionParameters data) {
            try {
                // TODO: Передавать фильтр в параметры задачи
                //var tsts = options.RawValues.Filter;
                //var tsts = JsonConvert.SerializeObject(options, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                UserInfo user = authorizationManager.GetCurrentUser();
                Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
                RoleInfo role = authorizationManager.GetCurrentRole();
                Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

                IEnumerable<int> clients = (IEnumerable<int>) data["clients"];

                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("clients", clients.ToList(), handlerData, visible: false, throwIfNotExists: false);

                //IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                //List<Promo> promoes = CastQueryToPromo(results);
                if (data.Count() > 1) {
                    HandlerDataHelper.SaveIncomingArgument("year", (int) data["year"], handlerData, visible: false, throwIfNotExists: false);
                }
                using (DatabaseContext context = new DatabaseContext()) {
                    LoopHandler handler = new LoopHandler() {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Scheduler Export",
                        Name = "Module.Host.TPM.Handlers.SchedulerExportHandler",
                        ExecutionPeriod = null,
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId,
                        RoleId = roleId
                    };
                    handler.SetParameterData(handlerData);
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                }
                return Content<string>(HttpStatusCode.OK, "Export task successfully created");
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private string GetUserName(string userName) {
            string[] userParts = userName.Split(new char[] { '/', '\\' });
            return userParts[userParts.Length - 1];
        }

        /// <summary>
        /// Преобразование записей в модель Promo
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private List<Promo> CastQueryToPromo(IQueryable records) {
            List<Promo> castedPromoes = new List<Promo>();
            Promo proxy = Context.Set<Promo>().Create<Promo>();
            foreach (var item in records) {
                if (item is IEntity<Guid>) {
                    Promo result = (Promo) Mapper.Map(item, proxy, typeof(Promo), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
                    castedPromoes.Add(result);
                } else if (item is ISelectExpandWrapper) {
                    var property = item.GetType().GetProperty("Instance");
                    var instance = property.GetValue(item);
                    Promo val = null;
                    if (instance is Promo) {
                        val = (Promo) instance;
                        castedPromoes.Add(val);
                    }
                }
            }
            return castedPromoes;
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей подбор аплифта и расчет параметров промо и продуктов
        /// </summary>
        /// <param name="promo"></param>
        private void CalculatePromo(Promo promo, bool needCalculatePlanMarketingTI, bool needResetUpliftCorrections, bool createDemandIncidentCreate = false, bool createDemandIncidentUpdate = false, string oldMarsMechanic = null, double? oldMarsMechanicDiscount = null, DateTimeOffset? oldDispatchesStart = null, double? oldPlanPromoUpliftPercent = null, double? oldPlanPromoIncrementalLSV = null) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("NeedCalculatePlanMarketingTI", needCalculatePlanMarketingTI, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("needResetUpliftCorrections", needResetUpliftCorrections, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("createDemandIncidentCreate", createDemandIncidentCreate, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("createDemandIncidentUpdate", createDemandIncidentUpdate, data, visible: false, throwIfNotExists: false);

            HandlerDataHelper.SaveIncomingArgument("oldMarsMechanic", oldMarsMechanic, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldMarsMechanicDiscount", oldMarsMechanicDiscount, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldDispatchesStart", oldDispatchesStart, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldPlanPromoUpliftPercent", oldPlanPromoUpliftPercent, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldPlanPromoIncrementalLSV", oldPlanPromoIncrementalLSV, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Uplift, data, Context, promo.Id);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        /// <summary>
        /// Чтение лога задачи 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult ReadPromoCalculatingLog(String promoId) {
            Guid promoGuid = Guid.Parse(promoId);
            String respond = null;
            int codeTo = 0;
            String opDataTo = null;
            string description = null;
            string status = null;
            try {
                Guid? handlerGuid = null;
                BlockedPromo bp = Context.Set<BlockedPromo>().FirstOrDefault(n => n.PromoId == promoGuid && !n.Disabled);

                Promo promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoGuid && !x.Disabled);
                if (promo != null) {
                    handlerGuid = Guid.Parse(promo.BlockInformation.Split('_')[0]);
                    if (handlerGuid != null) {
                        LoopHandler handler = Context.Set<LoopHandler>().FirstOrDefault(x => x.Id == handlerGuid);
                        if (handler != null) {
                            description = handler.Description;
                            status = handler.Status;

                            string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                            string logFileName = String.Format("{0}.txt", handlerGuid);
                            string filePath = System.IO.Path.Combine(logDir, logFileName);
                            if (File.Exists(filePath)) {
                                respond = File.ReadAllText(filePath);
                            } else {
                                respond = "";
                            }
                        }
                    }
                }

                if (bp == null) {
                    codeTo = 1;
                }

                //if (bp != null)
                //{
                //    //Чтение файла лога
                //    handlerGuid = bp.HandlerId;

                //    string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                //    string logFileName = String.Format("{0}.txt", handlerGuid);
                //    string filePath = System.IO.Path.Combine(logDir, logFileName);
                //    if (File.Exists(filePath))
                //    {
                //        respond = File.ReadAllText(filePath);
                //    }
                //    else
                //    {
                //        respond = "";
                //    }
                //}
                //else
                //{
                //    codeTo = 1;
                //}
            } catch (Exception e) {
                respond = e.Message;
                codeTo = 1;
            }

            return Json(new {
                success = true,
                data = respond,
                description,
                status,
                code = codeTo,
                opData = opDataTo
            });
        }

        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult GetHandlerIdForBlockedPromo(string promoId) {
            var guidPromoId = Guid.Parse(promoId);
            var blockedPromo = Context.Set<BlockedPromo>().FirstOrDefault(n => n.PromoId == guidPromoId && !n.Disabled);

            if (blockedPromo != null) {
                return Json(new {
                    success = true,
                    handlerId = blockedPromo.HandlerId
                });
            } else {
                return Json(new {
                    success = true
                });
            }
        }

        //Проверка на наличие ошибок в логе
        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult CheckIfLogHasErrors(string promoId)
        {
            var guidPromoId = Guid.Parse(promoId);
            BlockedPromo calculatingInfo = Context.Set<BlockedPromo>().Where(n => n.PromoId == guidPromoId).OrderByDescending(n => n.CreateDate).FirstOrDefault();
            if (calculatingInfo != null)
            {
                string contentOfLog = null;
                string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                string logFileName = String.Format("{0}.txt", calculatingInfo.HandlerId);
                string filePath = System.IO.Path.Combine(logDir, logFileName);

                if (File.Exists(filePath))
                {
                    contentOfLog = File.ReadAllText(filePath);
                    if (contentOfLog.Contains("[ERROR]"))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { LogHasErrors = true }));
                    }
                }
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { LogHasErrors = false }));
        }

        private void CreateImportTask(string fileName, string importHandler) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel() {
                    LogicType = "Import",
                    Name = System.IO.Path.GetFileName(fileName),
                    DisplayName = System.IO.Path.GetFileName(fileName)
                };

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromo), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromo).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Promo), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportPromo).Name,
                    //Name = "ProcessingHost.Handlers.Import." + importHandler,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }


        //Простановка дат в формате Mars
        public void SetPromoMarsDates(Promo promo) {
            string stringFormatYP2WD = "{0}P{1:D2}W{2}D{3}";

            if (promo.StartDate != null) {
                promo.MarsStartDate = (new MarsDate((DateTimeOffset) promo.StartDate)).ToString(stringFormatYP2WD);
            }
            if (promo.EndDate != null) {
                promo.MarsEndDate = (new MarsDate((DateTimeOffset) promo.EndDate)).ToString(stringFormatYP2WD);
            }
            if (promo.EndDate != null && promo.StartDate != null) {
                promo.PromoDuration = (promo.EndDate - promo.StartDate).Value.Days + 1;
            } else {
                promo.PromoDuration = null;
            }

            if (promo.DispatchesStart != null) {
                promo.MarsDispatchesStart = (new MarsDate((DateTimeOffset) promo.DispatchesStart)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesEnd != null) {
                promo.MarsDispatchesEnd = (new MarsDate((DateTimeOffset) promo.DispatchesEnd)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesStart != null && promo.DispatchesEnd != null) {
                promo.DispatchDuration = (promo.DispatchesEnd - promo.DispatchesStart).Value.Days + 1;
            } else {
                promo.DispatchDuration = null;
            }
        }

        /// <summary>
        /// Установка в промо цвета, бренда и BrandTech на основании дерева продуктов
        /// </summary>
        /// <param name="promo"></param>
        public void SetPromoByProductTree(Promo promo, List<PromoProductTree> promoProducts, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            PromoProductTree product = promoProducts.FirstOrDefault();
            DateTime dt = DateTime.Now;
            if (product != null) {
                //Заполнение Subranges
                IQueryable<ProductTree> ptQuery = context.Set<ProductTree>().Where(x => x.Type == "root"
                    || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));
                IEnumerable<int> promoProductsPTOIds = promoProducts.Select(z => z.ProductTreeObjectId);
                IQueryable<ProductTree> pts = ptQuery.Where(y => promoProductsPTOIds.Contains(y.ObjectId));
                promo.ProductSubrangesList = String.Join(";", pts.Where(x => x.Type == "Subrange").Select(z => z.Name));

                int objectId = product.ProductTreeObjectId;
                ProductTree pt = context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == objectId);
                if (pt != null) {
                    Guid? BrandId = null;
                    Guid? TechId = null;
                    Brand brandTo = null;
                    bool end = false;
                    do {
                        if (pt.Type == "Brand") {
                            brandTo = context.Set<Brand>().FirstOrDefault(x => x.Name == pt.Name);
                            if (brandTo != null) {
                                BrandId = brandTo.Id;
                                promo.BrandId = brandTo.Id;
                            }
                        }
                        if (pt.Type == "Technology") {
                            var tech = context.Set<Technology>().FirstOrDefault(x => x.Name == pt.Name);
                            if (tech != null) {
                                TechId = tech.Id;
                                promo.TechnologyId = tech.Id;
                            }
                        }
                        if (pt.parentId != 1000000) {
                            pt = context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == pt.parentId);
                        } else {
                            end = true;
                        }
                    } while (!end && pt != null);

                    if (brandTo == null) {
                        promo.BrandId = null;
                    }

                    BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => !x.Disabled && x.TechnologyId == TechId && x.BrandId == BrandId);
                    if (bt != null) {
                        promo.BrandTechId = bt.Id;
                        var colors = context.Set<Color>().Where(x => !x.Disabled && x.BrandTechId == bt.Id).ToList();
                        if (colors.Count() == 1) {
                            promo.ColorId = colors.First().Id;
                        } else {
                            promo.ColorId = null;
                        }
                    } else {
                        promo.ColorId = null;
                        promo.BrandTechId = null;
                    }
                } else {
                    promo.ColorId = null;
                }
            }
        }

        /// <summary>
        /// Установка значения единого поля для Mechanic
        /// </summary>
        /// <param name="promo"></param>
        public void SetMechanic(Promo promo, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            // нет механики - нет остального
            if (promo.MarsMechanicId != null) {
                var mechanic = context.Set<Mechanic>().Find(promo.MarsMechanicId);
                string result = mechanic.Name;

                if (promo.MarsMechanicTypeId != null) {
                    var mechanicType = context.Set<MechanicType>().Find(promo.MarsMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.MarsMechanicDiscount + "%";

                promo.Mechanic = result;
            }
        }

        /// <summary>
        /// Установка значения единого поля для Mechanic IA
        /// </summary>
        /// <param name="promo"></param>
        public void SetMechanicIA(Promo promo, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            string result = null;

            // нет механики - нет остального
            if (promo.PlanInstoreMechanicId != null) {
                var mechanic = context.Set<Mechanic>().Find(promo.PlanInstoreMechanicId);
                result = mechanic.Name;

                if (promo.PlanInstoreMechanicTypeId != null) {
                    var mechanicType = context.Set<MechanicType>().Find(promo.PlanInstoreMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.PlanInstoreMechanicDiscount + "%";
            }

            promo.MechanicIA = result;
        }


        /// <summary>
        /// Установка промо по дереву клиентов
        /// </summary>
        /// <param name="promo"></param>
        public void SetPromoByClientTree(Promo promo, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            int? ClientTreeId = promo.ClientTreeId;
            String resultMultiBaseStr = "";
            if (promo.ClientTreeId != null) {

                IQueryable<ClientTree> ctQuery = context.Set<ClientTree>().Where(x => x.Type == "root"
               || (DateTime.Compare(x.StartDate, DateTime.Now) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0)));
                ClientTree ct = ctQuery.FirstOrDefault(y => y.ObjectId == promo.ClientTreeId); ;
                if (ct != null) {
                    promo.ClientName = ct.Name;
                }
                while (ct != null && ct.depth != 0) {
                    if (ct.depth == 1) {
                        promo.Client1LevelName = ct.Name;
                    } else if (ct.depth == 2) {
                        promo.Client2LevelName = ct.Name;
                    }
                    ct = ctQuery.FirstOrDefault(y => y.ObjectId == ct.parentId);
                }

                int? upBaseClientId = RecursiveUpBaseClientsFind(ClientTreeId, context);
                if (upBaseClientId.HasValue) {
                    resultMultiBaseStr = upBaseClientId.ToString();
                } else {
                    resultMultiBaseStr =
                        String.Join("|", RecursiveDownBaseClientsFind(promo.ClientTreeId, context));
                }

            }
            promo.BaseClientTreeIds = resultMultiBaseStr;
        }

        /// <summary>
        /// Поиск базовых клиентов в дереве в корень
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private int? RecursiveUpBaseClientsFind(int? clientTreeId, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            if (!clientTreeId.HasValue) {
                return null;
            } else {
                ClientTree ctn = context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ctn == null) {
                    return null;
                } else if (ctn.IsBaseClient) {
                    return ctn.ObjectId;
                } else if (ctn.ObjectId == ctn.parentId) {
                    return null;
                } else {
                    return RecursiveUpBaseClientsFind(ctn.parentId, context);
                }

            }
        }


        /// <summary>
        /// Поиск базовых клиентов в дереве назад
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private List<int> RecursiveDownBaseClientsFind(int? clientTreeId, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            if (!clientTreeId.HasValue) {
                return new List<int>();
            } else {
                ClientTree ct = context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ct.IsBaseClient) {
                    return new List<int>() { ct.ObjectId };
                }

                List<ClientTree> ctChilds = context.Set<ClientTree>().Where(
                 x =>
                 //DateTime.Compare(x.StartDate, ct.StartDate) <= 0 && 
                 x.parentId == ct.ObjectId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0)).ToList();

                List<int> res = new List<int>();
                res.AddRange(ctChilds.Where(y => y.IsBaseClient).Select(z => z.ObjectId).ToList());

                foreach (var item in ctChilds.Where(y => !y.IsBaseClient)) {
                    res.AddRange(RecursiveDownBaseClientsFind(item.ObjectId, context));
                }
                return res;
            }
        }



        /// <summary>
        /// Создание отложенной задачи для расчета планового аплифта
        /// </summary>
        /// <param name="promo"></param>
        private void UpdateUplift(Promo promo) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Update uplift value",
                    Name = "Module.Host.TPM.Handlers.UpdateUpliftHandler",
                    ExecutionPeriod = null,
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Добавить продукты из иерархии к промо
        /// </summary>
        /// <param name="objectIds">Список ObjectId продуктов в иерархии</param>
        /// <param name="promo">Промо к которому прикрепляются продукты</param>
        public List<PromoProductTree> AddProductTrees(string objectIds, Promo promo, out bool isSubrangeChanged, DatabaseContext databaseContext = null) {
            var context = databaseContext ?? Context;

            // сформированный список продуктов - приходится использовать из-за отказа SaveChanges
            List<PromoProductTree> currentProducTrees = context.Set<PromoProductTree>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToList();
			List<string> currentProducTreesIds = currentProducTrees.Select(x => x.ProductTreeObjectId.ToString()).ToList();
			List<string> newProductTreesIds = new List<string>();

			if (!String.IsNullOrEmpty(objectIds))
			{
				newProductTreesIds = objectIds.Split(';').ToList();
			}
			List<string> dispatchIds = currentProducTreesIds.Except(newProductTreesIds).ToList();
			dispatchIds.AddRange(newProductTreesIds.Except(currentProducTreesIds));
			if (dispatchIds.Count == 0)
			{
				isSubrangeChanged = false;
			}
			else
			{
				isSubrangeChanged = true;
			}

			// Если Null, значит продукты не менялись
			if (objectIds != null) {
                List<int> productTreeObjectIds = new List<int>();

                if (objectIds.Length > 0) {
                    productTreeObjectIds = objectIds.Split(';').Select(n => Int32.Parse(n)).ToList();
                }

                // находим прежние записи, если они остались то ислючаем их из нового списка
                // иначе удаляем
                //var oldRecords = context.Set<PromoProductTree>().Where(n => n.PromoId == promo.Id && !n.Disabled);
                //PromoProductTree[] oldRecords = new PromoProductTree[currentProducTrees.Count];
                //currentProducTrees.CopyTo(oldRecords);
                foreach (var rec in currentProducTrees) {
                    int index = productTreeObjectIds.IndexOf(rec.ProductTreeObjectId);

                    if (index >= 0) {
                        productTreeObjectIds.RemoveAt(index);
                    } else {
                        rec.DeletedDate = System.DateTime.Now;
                        rec.Disabled = true;
                    }
                }

                // Добавляем новые продукты в промо
                foreach (int objectId in productTreeObjectIds) {
                    PromoProductTree promoProductTree = new PromoProductTree() {
                        Id = Guid.NewGuid(),
                        ProductTreeObjectId = objectId,
                        Promo = promo
                    };

                    currentProducTrees.Add(promoProductTree);
                    context.Set<PromoProductTree>().Add(promoProductTree);
                }
            }

            return currentProducTrees.Where(n => !n.Disabled).ToList();
        }

        /// <summary>
        /// Проверка TI, COGS и наличия продуктов, попадающих под фильтрация
        /// </summary>
        /// <param name="promo">Проверяемое промо</param>
        /// <param name="promoProductTrees">Список узлов продуктового дерева</param>
        /// <exception cref="Exception">Исключение генерируется при отсутсвии одного из проверяемых параметров</exception>
        public void CheckSupportInfo(Promo promo, List<PromoProductTree> promoProductTrees, out List<Product> products, DatabaseContext databaseContext = null) {
            DatabaseContext context = databaseContext ?? Context;

            List<string> messagesError = new List<string>();
            string message = null;
            bool error;
            bool isProductListEmpty = false;
            products = null;

            // проверка на наличие TI
            PlanPromoParametersCalculation.GetTIBasePercent(promo, context, out message, out error);
            if (message != null && error)
            {
                messagesError.Add(message);
                message = null;
            }
            else if (message != null)
            {
                throw new Exception(message);
            }

            // проверка на наличие COGS
            PlanPromoParametersCalculation.GetCOGSPercent(promo, context, out message);
            if (message != null)
            {
                messagesError.Add(message);
                message = null;
            }

            // проверка на наличие продуктов, попадающих под фильтр (для промо не из TLC)
            if (!promo.LoadFromTLC)
            {
                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    products = PlanProductParametersCalculation.GetCheckedProducts(context, promo);
                }
                else
                {
                    isProductListEmpty = PlanProductParametersCalculation.IsProductListEmpty(promo, context, out message, promoProductTrees);
                }
                if (message != null)
                {
                    messagesError.Add(message);
                }

                if (isProductListEmpty)
                {
                    messagesError.Add("Product list is empty. Please, check assortment matrix");
                }
            }

            // если что-то не найдено, то генерируем ошибку
            if (messagesError.Count > 0) {
                string messageError = "";
                for (int i = 0; i < messagesError.Count; i++) {
                    string endString = i == messagesError.Count - 1 ? "" : " ";
                    messageError += messagesError[i] + endString;
                }

                throw new Exception(messageError);
            }
        }


        private IEnumerable<Column> GetPromoROIExportSettings() {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>() {
                new Column { Order = orderNumber++, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column { Order = orderNumber++, Field = "Client1LevelName", Header = "NA/RKA", Quoting = false },
                new Column { Order = orderNumber++, Field = "Client2LevelName", Header = "Client Group", Quoting = false },
                new Column { Order = orderNumber++, Field = "ClientName", Header = "Client", Quoting = false },
                new Column { Order = orderNumber++, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column { Order = orderNumber++, Field = "Technology.Name", Header = "Technology", Quoting = false },
                new Column { Order = orderNumber++, Field = "ProductSubrangesList", Header = "Subrange", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanic.Name", Header = "Mars mechanic", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicType.Name", Header = "Mars mechanic type", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column { Order = orderNumber++, Field = "MechanicComment", Header = "Mechanic comment", Quoting = false },
                new Column { Order = orderNumber++, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "PromoDuration", Header = "Promo duration", Quoting = false , Format = "0"},
                new Column { Order = orderNumber++, Field = "DispatchDuration", Header = "Dispatch Duration", Quoting = false, Format = "0" },
                new Column { Order = orderNumber++, Field = "EventName", Header = "Event", Quoting = false },
                new Column { Order = orderNumber++, Field = "PromoStatus.Name", Header = "Status", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanic.Name", Header = "Plan Instore Mechanic Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanicType.Name", Header = "Plan Instore Mechanic Type Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanicDiscount", Header = "Plan Instore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTIShopper", Header = "Plan Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTIMarketing", Header = "Plan Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoXSites", Header = "Plan Promo X-Sites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCatalogue", Header = "Plan Promo Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPOSMInClient", Header = "Plan Promo POSM In Client", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBranding", Header = "Plan Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBTL", Header = "Plan Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProduction", Header = "Plan Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdXSites", Header = "Plan PromoCostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdCatalogue", Header = "Plan PromoCostProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdPOSMInClient", Header = "Plan PromoCostProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCost", Header = "Plan Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalBaseTI", Header = "Plan Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalCOGS", Header = "Plan Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTotalCost", Header = "Plan Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPostPromoEffectLSVW1", Header = "Plan Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPostPromoEffectLSVW2", Header = "Plan Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPostPromoEffectLSV", Header = "Plan Post Promo Effect LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalLSV", Header = "Plan Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetLSV", Header = "PlanPromo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBaselineBaseTI", Header = "Plan Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBaseTI", Header = "Plan Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetNSV", Header = "Plan Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalNSV", Header = "Plan Promo Total Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalNSV", Header = "Plan Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalMAC", Header = "Plan Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalMAC", Header = "Plan Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalEarnings", Header = "Plan Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalEarnings", Header = "Plan Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoROIPercent", Header = "Plan Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetROIPercent", Header = "Plan Promo Net ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetUpliftPercent", Header = "Plan Promo Net Uplift %", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanInStoreShelfPrice", Header = "Plan Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualInStoreMechanic.Name", Header = "Actual InStore Mechanic Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "ActualInStoreMechanicType.Name", Header = "Actual InStore Mechanic Type Name", Quoting = false  },
                new Column { Order = orderNumber++, Field = "ActualInStoreDiscount", Header = "Actual InStore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualInStoreShelfPrice", Header = "Actual Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "InvoiceNumber", Header = "Invoice number", Quoting = false },
                new Column { Order = orderNumber++, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoLSVByCompensation", Header = "Actual PromoLSV By Compensation", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTIShopper", Header = "Actual Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTIMarketing", Header = "Actual Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoXSites", Header = "Actual Promo Prod XSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCatalogue", Header = "Actual Promo Prod Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPOSMInClient", Header = "Actual Promo Prod POSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBranding", Header = "Actual Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBTL", Header = "Actual Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProduction", Header = "Actual Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdXSites", Header = "Actual Promo CostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdCatalogue", Header = "Actual Promo Cost ProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdPOSMInClient", Header = "Actual Promo Cost ProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCost", Header = "Actual Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalBaseTI", Header = "Actual Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalCOGS", Header = "Actual Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTotalCost", Header = "Actual Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPostPromoEffectLSVW1", Header = "Actual Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPostPromoEffectLSVW2", Header = "Actual Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPostPromoEffectLSV", Header = "Actual Post Promo Effect LSV", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalLSV", Header = "Actual Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetLSV", Header = "Actual Promo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalNSV", Header = "Actual Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalNSV", Header = "Actual Promo Net Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBaselineBaseTI", Header = "Actual Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBaseTI", Header = "Actual Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetNSV", Header = "Actual Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalMAC", Header = "Actual Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalMAC", Header = "Actual Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalEarnings", Header = "Actual Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalEarnings", Header = "Actual Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoROIPercent", Header = "Actual Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetROIPercent", Header = "Actual Promo Net ROI%", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetUpliftPercent", Header = "Actual Promo Net Uplift Percent", Quoting = false,  Format = "0"  }};
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportPromoROIReportXLSX(ODataQueryOptions<Promo> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetPromoROIExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoROIReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool NeedRecalculatePromo(Promo newPromo, Promo oldPromo) {
            bool needReacalculate = false;

            // Если есть различия в этих полях.
            if (oldPromo.ClientTreeId != newPromo.ClientTreeId
                    || oldPromo.MarsMechanicId != newPromo.MarsMechanicId
                    || oldPromo.MarsMechanicTypeId != newPromo.MarsMechanicTypeId
                    || oldPromo.MarsMechanicDiscount != newPromo.MarsMechanicDiscount
                    || oldPromo.StartDate != newPromo.StartDate
                    || oldPromo.EndDate != newPromo.EndDate
                    || oldPromo.DispatchesStart != newPromo.DispatchesStart
                    || oldPromo.DispatchesEnd != newPromo.DispatchesEnd
                    || (oldPromo.ActualPromoBTL != null && newPromo.ActualPromoBTL != null
                        && Math.Round(oldPromo.ActualPromoBTL.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.ActualPromoBTL.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.PlanPromoBTL != null && newPromo.PlanPromoBTL != null
                        && Math.Round(oldPromo.PlanPromoBTL.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.PlanPromoBTL.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.ActualPromoBranding != null && newPromo.ActualPromoBranding != null
                        && Math.Round(oldPromo.ActualPromoBranding.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.ActualPromoBranding.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.PlanPromoBranding != null && newPromo.PlanPromoBranding != null
                        && Math.Round(oldPromo.PlanPromoBranding.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.PlanPromoBranding.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.PlanPromoUpliftPercent != null && newPromo.PlanPromoUpliftPercent != null
                        && Math.Round(oldPromo.PlanPromoUpliftPercent.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.PlanPromoUpliftPercent.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.NeedRecountUplift != null && newPromo.NeedRecountUplift != null && oldPromo.NeedRecountUplift != newPromo.NeedRecountUplift)
                    || oldPromo.PromoStatus.Name.ToLower() == "draft"
                || !String.IsNullOrEmpty(newPromo.AdditionalUserTimestamp))


            {
                needReacalculate = true;
            }

            //if (newPromo.InOut.HasValue && newPromo.InOut.Value)
            //{
                List<Product> oldInOutProducts = PlanProductParametersCalculation.GetCheckedProducts(Context, oldPromo);
                List<Product> newInOutProducts = PlanProductParametersCalculation.GetCheckedProducts(Context, newPromo);

                needReacalculate = needReacalculate || oldInOutProducts.Count != newInOutProducts.Count || !oldInOutProducts.All(x => newInOutProducts.Any(y => y.Id == x.Id));
            //} 
            //else
            //{
            //    needReacalculate = needReacalculate || oldPromo.ProductHierarchy != newPromo.ProductHierarchy;

            //    List<Product> oldRegularProducts = PlanProductParametersCalculation.GetExcludedProducts(Context, oldPromo.RegularExcludedProductIds);
            //    List<Product> newRegularProducts = PlanProductParametersCalculation.GetExcludedProducts(Context, newPromo.RegularExcludedProductIds);

            //    needReacalculate = needReacalculate || oldRegularProducts.Count != newRegularProducts.Count || !oldRegularProducts.All(x => newRegularProducts.Any(y => y.Id == x.Id));
            //}

            return needReacalculate;
        }

        /// <summary>
        /// Проверить не изменился ли набор продуктов, отбираемых по фильтру
        /// </summary>
        /// <param name="promo">Модель промо</param>
        /// <param name="products">Список продуктов, отфильтрованных на данный момент</param>
        /// <returns></returns>
        private bool CheckChangesInProductList(Promo promo, List<Product> products)
        {            
            List<PromoProduct> promoProducts = Context.Set<PromoProduct>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToList();
            bool changed = promoProducts.Count != products.Count;

            if (!changed) {
                foreach (PromoProduct p in promoProducts) {
                    if (!products.Any(n => n.ZREP == p.ZREP)) {
                        changed = true;
                        break;
                    }
                }
            }

            return changed;
        }

        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult GetProducts(ODataActionParameters data)
        {
            try
            {
                var inOutProductIds = data["InOutProductIds"] as IEnumerable<string>;
                var products = new List<Product>();

                inOutProductIds.ToList().ForEach(productId => 
                {
                    Guid productGuidId;
                    if (Guid.TryParse(productId, out productGuidId))
                    {
                        var productForAdding = Context.Set<Product>().FirstOrDefault(product => product.Id == productGuidId);
                        if (productForAdding != null)
                        {
                            products.Add(productForAdding);
                        }
                    }
                });

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = JsonConvert.SerializeObject(products) }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, data = e.Message }));
            }
        }

		[HttpPost]
		[ClaimsAuthorize]
		public IHttpActionResult CheckPromoCreator(string promoId)
		{
			UserInfo user = authorizationManager.GetCurrentUser();
			bool isCreator = false;

			var promo = Context.Set<Promo>().Where(p => p.Id.ToString() == promoId).FirstOrDefault();
			if (promo != null)
			{
				isCreator = promo.CreatorId == user.Id;
			}
			
			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { isCreator = isCreator }));
		}
        public void FixateTempPromoProductsCorrections(Guid promoId, string tempEditUpliftId)
        {
            var queryTemp = Context.Set<PromoProductsCorrection>().Where(x => x.PromoProduct.PromoId == promoId && x.TempId == tempEditUpliftId && x.Disabled != true);
            var query = Context.Set<PromoProductsCorrection>().Where(x => x.PromoProduct.PromoId == promoId && x.TempId == null && x.Disabled != true);
            var promoProductIds = query.Select(x => x.PromoProductId).ToList();
            PromoProductsCorrection promoProductsCorrection;
            UserInfo user = authorizationManager.GetCurrentUser();

            foreach (var tempCorrection in queryTemp)
            {
                if (promoProductIds.Contains(tempCorrection.PromoProductId))
                {
                    promoProductsCorrection = query.Where(x => x.PromoProductId == tempCorrection.PromoProductId).FirstOrDefault();
                    promoProductsCorrection.PlanProductUpliftPercentCorrected = tempCorrection.PlanProductUpliftPercentCorrected;
                    promoProductsCorrection.UserId = user.Id;
                    promoProductsCorrection.UserName = user.Login;
                    promoProductsCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                }
                else
                {
                    promoProductsCorrection = new PromoProductsCorrection()
                    {
                        ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        PromoProduct = tempCorrection.PromoProduct,
                        PlanProductUpliftPercentCorrected = tempCorrection.PlanProductUpliftPercentCorrected,
                        UserId = user.Id,
                        UserName = user.Login
                    };
                    Context.Set<PromoProductsCorrection>().Add(promoProductsCorrection);
                }
                tempCorrection.Disabled = true;
            }
            
        }
    }
}
