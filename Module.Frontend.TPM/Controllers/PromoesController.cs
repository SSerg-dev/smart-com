﻿using AutoMapper;
using Core.Data;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.RA;
using Module.Frontend.TPM.FunctionalHelpers.RSmode;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Model;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoesController : EFContextController
    {

        private readonly IAuthorizationManager authorizationManager;

        public PromoesController() { }

        public PromoesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Promo> GetConstraintedQuery(bool canChangeStateOnly = false, TPMmode tPMmode = TPMmode.Current, bool withDeleted = false)
        {
            PerformanceLogger logger = new PerformanceLogger();
            logger.Start();
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<Promo> query = Context.Set<Promo>();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            if (withDeleted)
            {
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, tPMmode, filters, FilterQueryModes.None, canChangeStateOnly ? role : String.Empty);
            }
            else
            {
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, tPMmode, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);
            }

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            logger.Stop();
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<Promo> GetPromo([FromODataUri] Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetPromoes(bool canChangeStateOnly = false, TPMmode tPMmode = TPMmode.Current)
        {
            string filter = HttpContext.Current.Request.QueryString["$filter"];
            bool oneRecord = filter == null ? false : filter.Contains("Id eq guid");
            if (oneRecord)
            {
                return Context.Set<Promo>();
            }
            return GetConstraintedQuery(canChangeStateOnly, tPMmode);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetCanChangeStatePromoes(bool canChangeStateOnly = false, TPMmode tPMmode = TPMmode.Current)
        {
            return GetConstraintedQuery(canChangeStateOnly, tPMmode);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<Promo> GetFilteredData(ODataQueryOptions<Promo> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            string filter = HttpContext.Current.Request.QueryString["$filter"];
            bool IsMasterFiltered = filter == null ? false : filter.Contains("MasterPromoId");
            var query = GetConstraintedQuery(Helper.GetValueIfExists<bool>(bodyText, "canChangeStateOnly"), JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode"), IsMasterFiltered);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<Promo>(options.Context, Request, HttpContext.Current.Request);

            return optionsPost.ApplyTo(query, querySettings) as IQueryable<Promo>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Promo> patch)
        {
            var model = Context.Set<Promo>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            model.DeviationCoefficient /= 100;
            try
            {
                List<ClientTree> clientTrees = Context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
                //Установка полей по дереву ProductTree
                //SetPromoByProductTree(model);
                //Установка дат в Mars формате
                PromoHelper.SetPromoMarsDates(model);
                //Установка полей по дереву ClientTree
                PromoHelper.SetPromoByClientTree(model, clientTrees);

                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(model);
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Post(Promo model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (model.IsSplittable)//Split subranges
                {
                    Promo promo = SplitSubranges(model);

                    return Created(promo);
                }
                UserInfo user = authorizationManager.GetCurrentUser();
                RoleInfo role = authorizationManager.GetCurrentRole();
                Promo result = PromoHelper.SavePromo(model, Context, user, role);

                return Created(result);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private Promo SplitSubranges(Promo model)
        {
            Promo promo = new Promo();
            List<string> productTreeObjectIds = model.ProductTreeObjectIds.Split(';').ToList();
            List<string> inOutProductIds = model.InOutProductIds.Split(';').ToList();
            List<string> productSubranges = model.ProductSubrangesList?.Split(';').ToList();
            List<string> productSubrangesRU = model.ProductSubrangesListRU?.Split(';').ToList();

            UserInfo user = authorizationManager.GetCurrentUser();
            RoleInfo role = authorizationManager.GetCurrentRole();

            inOutProductIds.RemoveAt(inOutProductIds.Count - 1);//remove last element, because it contains empty
            for (int i = 0; i < productTreeObjectIds.Count(); i++)
            {
                IEnumerable<int> productTreeObjectIdsArray = new int[] { Convert.ToInt32(productTreeObjectIds[i]) };
                IQueryable<ProductTree> productTreeNodes = Context.Set<ProductTree>().Where(x => productTreeObjectIdsArray.Any(y => x.ObjectId == y) && !x.EndDate.HasValue);
                List<Func<Product, bool>> expressionList = new List<Func<Product, bool>>();
                try
                {
                    expressionList = ProductsController.GetExpressionList(productTreeNodes);
                }
                catch { }
                List<Product> filteredProducts = Context.Set<Product>().Where(x => !x.Disabled).ToList();
                List<Product> listProducts = filteredProducts.Where(x => expressionList.Any(y => y.Invoke(x))).ToList();
                string inOutProductIdsForProductTree = "";
                foreach (string iopi in inOutProductIds)
                {
                    if (listProducts.Any(a => a.Id == new Guid(iopi)))
                    {
                        inOutProductIdsForProductTree = inOutProductIdsForProductTree + iopi + ';';
                    }
                }
                model.ProductTreeObjectIds = productTreeObjectIds[i];
                model.InOutProductIds = inOutProductIdsForProductTree;
                model.ProductSubrangesList = productSubranges == null ? "" : productSubranges[i];
                model.ProductSubrangesListRU = productSubrangesRU == null ? "" : productSubrangesRU[i];
                promo = PromoHelper.SavePromo(model, Context, user, role);
            }

            return promo;
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Promo> patch)
        {
            try
            {
                Promo model = Context.Set<Promo>()
                    .Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(y => y.ProductCorrectionPriceIncreases))
                    .FirstOrDefault(g => g.Id == key);
                if (model == null)
                {
                    return NotFound();
                }
                Promo promo = patch.GetEntity();
                if (promo.IsSplittable)
                {
                    patch.Patch(model);
                    model.Name = promo.Name ?? model.Name;
                    model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(s => s.SystemName == StateNames.DRAFT_PUBLISHED && !s.Disabled).Id;
                    model.ProductTreeObjectIds = promo.ProductTreeObjectIds;
                    SplitSubranges(model);
                    await DeletePromo(key);
                    return Updated(model);
                }
                var isSubrangeChanged = false;
                DateTimeOffset? ChangedDate = DateTimeOffset.UtcNow;

                Promo ChangePromo = new Promo();
                patch.CopyChangedValues(ChangePromo);
                IEnumerable<string> ChangedList = patch.GetChangedPropertyNames();

                //Promo promoCopy = new Promo(model);
                Promo promoCopy = AutomapperProfiles.PromoCopy(model);
                if (model.TPMmode == TPMmode.Current)
                {
                    if (model.TPMmode == TPMmode.RS || ChangePromo.TPMmode == TPMmode.RS)
                    {
                        if (model.TPMmode != ChangePromo.TPMmode)
                        {
                            model = RSmodeHelper.EditToPromoRS(Context, model);
                        }
                        else
                        {
                            Promo promoRS = Context.Set<Promo>().FirstOrDefault(f => f.Number == model.Number && f.TPMmode == TPMmode.RS);
                            if (promoRS != null)
                            {
                                Context.Set<Promo>().Remove(promoRS);
                                await Context.SaveChangesAsync();
                                patch.Patch(model);
                                RSmodeHelper.EditToPromoRS(Context, model);
                            }
                        }
                    }
                    if (model.TPMmode == TPMmode.RA || ChangePromo.TPMmode == TPMmode.RA)
                    {
                        if (model.TPMmode != ChangePromo.TPMmode)
                        {
                            model = RAmodeHelper.EditToPromoRA(Context, model);
                        }
                        else
                        {
                            Promo promoRA = Context.Set<Promo>().FirstOrDefault(f => f.Number == model.Number && f.TPMmode == TPMmode.RA);
                            if (promoRA != null)
                            {
                                Context.Set<Promo>().Remove(promoRA);
                                await Context.SaveChangesAsync();
                                patch.Patch(model);
                                RAmodeHelper.EditToPromoRA(Context, model);
                            }
                        }
                    }
                }
                patch.Patch(model);

                model.DeviationCoefficient /= 100;
                if (!String.IsNullOrEmpty(model.AdditionalUserTimestamp))
                    FixateTempPromoProductsCorrections(model.Id, model.AdditionalUserTimestamp);

                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
                model.DispatchesStart = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesStart);
                model.DispatchesEnd = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesEnd);

                UserInfo user = authorizationManager.GetCurrentUser();
                RoleInfo role = authorizationManager.GetCurrentRole();
                string userRole = role.SystemName;
                bool needToCreateDemandIncident = false;
                // при изменении StartDate скидываем в Event в Standard promo
                if (ChangePromo.StartDate != null && userRole != "SupportAdministrator")
                {
                    DateTimeOffset StartDateCopy = (DateTimeOffset)promoCopy.StartDate;
                    DateTimeOffset StartDateChange = (DateTimeOffset)ChangePromo.StartDate;
                    if (StartDateCopy.Date != StartDateChange.Date)
                    {
                        if (ChangePromo.EventId == null)
                        {
                            Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == "Standard promo");
                            if (promoEvent == null)
                            {
                                return InternalServerError(new Exception("Event 'Standard promo' not found"));
                            }

                            ChangePromo.EventId = promoEvent.Id;
                            model.EventId = promoEvent.Id;
                            model.EventName = promoEvent.Name;
                        }
                    }
                }


                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out string message);
                if (!status)
                {
                    return InternalServerError(new Exception(message));
                }

                String statusName = Context.Set<PromoStatus>().FirstOrDefault(s => s.Id == model.PromoStatusId && !s.Disabled).SystemName;
                String prevStatusName = Context.Set<PromoStatus>().FirstOrDefault(s => s.Id == promoCopy.PromoStatusId && !s.Disabled).SystemName;
                bool needDetachPromoSupport = statusName.ToLower() == "draft" && prevStatusName.ToLower() == "draftpublished";

                if (statusName.ToLower() == "onapproval" && userRole == "SupportAdministrator")
                {
                    PromoStatusHelper.StableOnApprovalStatus(model, userRole, Context);
                }

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
                    || promoCopy.IsDemandPlanningApproved != model.IsDemandPlanningApproved
                    || promoCopy.IsGAManagerApproved != model.IsGAManagerApproved)
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
                    // если approved переводим в cancelled дочерние
                    if (statusName.ToLower() == "approved" && userRole != "SupportAdministrator")
                    {
                        string childmessage = await DeleteChildPromoes(model.Id, user);
                        if (!string.IsNullOrEmpty(childmessage))
                        {
                            return InternalServerError(new Exception(childmessage));
                        }
                    }
                }

                SetBrandTechIdPromo(model);

                if (statusName.ToLower() != "cancelled" && (statusName.ToLower() != "finished" || statusName.ToLower() == "finished" && userRole == "SupportAdministrator"))
                {
                    List<PromoProductTree> promoProductTrees = new List<PromoProductTree>();
                    if (!model.LoadFromTLC)
                    {
                        // Добавление продуктов
                        promoProductTrees = PromoHelper.AddProductTrees(model.ProductTreeObjectIds, model, out isSubrangeChanged);
                    }

                    List<Product> products = Context.Set<Product>().Where(x => !x.Disabled).ToList();
                    bool needRecalculatePromo = NeedRecalculatePromo(model, promoCopy, products);
                    bool needResetUpliftCorrections = false;
                    bool needResetUpliftCorrectionsPI = false;
                    if (model.NeedRecountUplift != null && promoCopy.NeedRecountUplift != null && model.NeedRecountUplift != promoCopy.NeedRecountUplift)
                    {
                        needResetUpliftCorrections = true;
                    }

                    //if (model.NeedRecountUpliftPI != promoCopy.NeedRecountUpliftPI ||
                    //    (promoCopy.PlanPromoUpliftPercentPI == null && model.PlanPromoUpliftPercentPI != null &&
                    //    Math.Round((double)model.PromoPriceIncrease.PlanPromoUpliftPercent, 2, MidpointRounding.AwayFromZero) != model.PlanPromoUpliftPercentPI))
                    //{
                    //    needResetUpliftCorrectionsPI = true;

                    //}
                    // PriceIncrease
                    if (model.PromoPriceIncrease != null && model.PlanPromoUpliftPercentPI != null)
                    {
                        model.PromoPriceIncrease.PlanPromoUpliftPercent = model.PlanPromoUpliftPercentPI;
                    }
                    model.AdditionalUserTimestamp = null;

                    //для ускорения перехода в следующий статус (если нет изменений параметров промо, то пропускаем следующие действия)
                    if (needRecalculatePromo || statusName.ToLower() == "draft")
                    {
                        OneLoadModel oneLoad = new OneLoadModel
                        {
                            Mechanics = Context.Set<Mechanic>().Where(g => !g.Disabled).ToList(),
                            MechanicTypes = Context.Set<MechanicType>().Where(g => !g.Disabled).ToList(),
                            ClientTrees = Context.Set<ClientTree>().Where(g => g.EndDate == null).ToList(),
                            ProductTrees = Context.Set<ProductTree>().Where(g => g.EndDate == null).ToList(),
                            Brands = Context.Set<Brand>().Where(g => !g.Disabled).ToList(),
                            Technologies = Context.Set<Technology>().Where(g => !g.Disabled).ToList(),
                            BrandTeches = Context.Set<BrandTech>().Where(g => !g.Disabled).ToList(),
                            Colors = Context.Set<Color>().Where(g => !g.Disabled).ToList()
                        };
                        //Установка полей по дереву ProductTree
                        PromoHelper.SetPromoByProductTree(model, promoProductTrees, oneLoad);
                        //Установка дат в Mars формате
                        PromoHelper.SetPromoMarsDates(model);
                        //Установка полей по дереву ClientTree
                        PromoHelper.SetPromoByClientTree(model, oneLoad.ClientTrees);
                        //Установка механик
                        PromoHelper.SetMechanic(model, oneLoad.Mechanics, oneLoad.MechanicTypes);
                        PromoHelper.SetMechanicIA(model, oneLoad.Mechanics, oneLoad.MechanicTypes);

                        if (statusName.ToLower() == "draft")
                        {
                            string error;
                            // Прикрепление продуктов
                            PlanProductParametersCalculation.SetPromoProduct(model.Id, Context, out error, true, promoProductTrees);
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
                        if (promoEvent == null)
                        {
                            return InternalServerError(new Exception("Event not found"));
                        }
                        model.EventName = promoEvent.Name;
                    }

                    // для draft не проверяем и не считаем
                    if (statusName.ToLower() != "draft")
                    {
                        OneLoadModel oneLoad = new OneLoadModel
                        {
                            ClientTrees = await Context.Set<ClientTree>().Where(g => g.EndDate == null).ToListAsync(),
                            BrandTeches = await Context.Set<BrandTech>().Where(g => !g.Disabled).ToListAsync(),
                            COGSs = await Context.Set<COGS>().Where(x => !x.Disabled).ToListAsync(),
                            PlanCOGSTns = await Context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToListAsync(),
                            ProductTrees = await Context.Set<ProductTree>().Where(g => g.EndDate == null).ToListAsync(),
                            TradeInvestments = await Context.Set<TradeInvestment>().Where(x => !x.Disabled).ToListAsync(),
                            Products = await Context.Set<Product>().Where(g => !g.Disabled).ToListAsync()
                        };
                        // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)

                        List<Product> filteredProducts = PromoHelper.CheckSupportInfo(model, promoProductTrees, oneLoad, Context); // продукты, подобранные по фильтрам

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
                                filteredProducts = PlanProductParametersCalculation.GetCheckedProducts(model, products);
                                resultProductList = PlanProductParametersCalculation.GetResultProducts(filteredProducts, eanPCs, model, products);
                            }
                            else
                            {
                                resultProductList = PlanProductParametersCalculation.GetCheckedProducts(model, products);
                            }

                            changedProducts = CheckChangesInProductList(model, resultProductList);
                        }

                        //создание отложенной задачи, выполняющей переподбор аплифта и перерассчет параметров                    
                        //если у промо есть признак InOut, то Uplift считать не нужно.
                        if ((changedProducts || needRecalculatePromo) && userRole != "SupportAdministrator")
                        {
                            // если меняем длительность промо, то пересчитываем Marketing TI
                            bool needCalculatePlanMarketingTI = promoCopy.StartDate != model.StartDate || promoCopy.EndDate != model.EndDate;
                            needToCreateDemandIncident = PromoHelper.CheckCreateIncidentCondition(promoCopy, model, patch, isSubrangeChanged);

                            PromoHelper.CalculatePromo(model, Context, (Guid)user.Id, (Guid)role.Id, needCalculatePlanMarketingTI, needResetUpliftCorrections, needResetUpliftCorrectionsPI, false, needToCreateDemandIncident, promoCopy.MarsMechanic.Name, promoCopy.MarsMechanicDiscount, promoCopy.DispatchesStart, promoCopy.PlanPromoUpliftPercent, promoCopy.PlanPromoIncrementalLSV); //TODO: Задача создаётся раньше чем сохраняются изменения промо.
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
                        PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);
                    }
                }
                else if (statusName.ToLower() != "finished")
                {
                    //при отмене промо необходимо отвязать бюджеты от промо и пересчитать эти бюджеты
                    PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                    PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);

                    //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                    if (model.InOut.HasValue && model.InOut.Value)
                    {
                        PromoHelper.DisableIncrementalPromo(Context, model);
                    }
                }
                if (ChangePromo.EventId != null)
                {
                    var btlPromo = Context.Set<BTLPromo>().FirstOrDefault(x => x.PromoId == key && !x.Disabled);
                    if (btlPromo != null)
                    {
                        btlPromo.Disabled = true;
                        btlPromo.DeletedDate = DateTimeOffset.UtcNow;
                        PromoCalculateHelper.CalculateBTLBudgetsCreateTask(btlPromo.BTLId.ToString(), null, null, Context, new List<Guid> { key }, safe: true);
                    }
                }

                if (promoCopy.PromoStatus.SystemName.ToLower() == "cancelled")
                {
                    model.PlanPromoXSites = null;
                    model.PlanPromoCatalogue = null;
                    model.PlanPromoPOSMInClient = null;
                    model.PlanPromoCostProdXSites = null;
                    model.PlanPromoCostProdCatalogue = null;
                    model.PlanPromoCostProdPOSMInClient = null;
                    model.ActualPromoXSites = null;
                    model.ActualPromoCatalogue = null;
                    model.ActualPromoPOSMInClient = null;
                    model.ActualPromoCostProdXSites = null;
                    model.ActualPromoCostProdCatalogue = null;
                    model.ActualPromoCostProdPOSMInClient = null;
                    model.PlanPromoTIShopper = null;
                    model.PlanPromoTIMarketing = null;
                    model.PlanPromoBranding = null;
                    model.PlanPromoCost = null;
                    model.PlanPromoBTL = null;
                    model.PlanPromoCostProduction = null;
                    model.ActualPromoTIShopper = null;
                    model.ActualPromoTIMarketing = null;
                    model.ActualPromoBranding = null;
                    model.ActualPromoBTL = null;
                    model.ActualPromoCostProduction = null;
                    model.ActualPromoCost = null;
                }
                if (model.PromoStatus.SystemName.ToLower() == "cancelled")
                {
                    List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
                    foreach (PromoProduct promoProduct in promoProductToDeleteList)
                    {
                        promoProduct.DeletedDate = System.DateTime.Now;
                        promoProduct.Disabled = true;
                    }
                    //model.NeedRecountUplift = true;
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
                if (model.InOutProductIds != null && promoCopy.InOutProductIds != null &&
                    model.InOutProductIds != promoCopy.InOutProductIds && prevStatusName.ToLower() != "draftpublished")
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

                if (model.PromoStatus.SystemName.ToLower() != "finished" && userRole == "SupportAdministrator")
                {
                    PromoHelper.CalculateSumInvoiceProduct(Context, model);
                }

                if (model.PromoStatus.SystemName.ToLower() == "finished")
                {
                    //Сбрасываем значение SumInvoice при снятии галочки
                    if (promoCopy.ManualInputSumInvoice == true && model.ManualInputSumInvoice == false)
                    {
                        model.SumInvoice = null;
                    }
                    PromoHelper.CalculateSumInvoiceProduct(Context, model);
                    CreateTaskCalculateActual(model.Id, safe: true);
                }
                if (model.IsInExchange == false && promoCopy.IsInExchange == true)
                {
                    // unlink дочерние промо
                    var PromoesUnlink = Context.Set<Promo>().Where(p => p.MasterPromoId == model.Id && !p.Disabled).ToList();
                    foreach (var childpromo in PromoesUnlink)
                    {
                        childpromo.MasterPromoId = null;
                    }
                }
                await Context.SaveChangesAsync();

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

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(ex));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult RecalculatePromo(Guid promoId)
        {
            Promo promo = Context.Set<Promo>().Find(promoId);
            if (promo == null)
                return NotFound();

            UserInfo user = authorizationManager.GetCurrentUser();
            RoleInfo role = authorizationManager.GetCurrentRole();

            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    PromoHelper.CalculatePromo(promo, Context, (Guid)user.Id, (Guid)role.Id, true, false, false);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return InternalServerError(GetExceptionMessage.GetInnerException(e));
                }
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> MassApprove()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "MassApproveHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("promoNumbers", promoNumbers, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Mass promo approving",
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> SendForApproval()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "MassSendForApprovalHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("promoNumbers", promoNumbers, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Mass promo send for approval",
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

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
        public IHttpActionResult ChangeResponsible(Guid promoId, string userName)
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

        private void SetBrandTechIdPromo(Promo model)
        {
            if (model.ProductTreeObjectIds != null)
            {
                List<int> productTreeObjectIds = new List<int>();
                if (model.ProductTreeObjectIds.Length > 0)
                {
                    productTreeObjectIds = model.ProductTreeObjectIds.Split(';').Select(n => Int32.Parse(n)).ToList();
                }

                int objectId = productTreeObjectIds.FirstOrDefault();

                DateTime dt = DateTime.Now;
                ProductTree productTree = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == objectId);
                if (productTree != null)
                {
                    Guid? brandId = null;
                    Guid? technologyId = null;
                    Brand brand = null;
                    Technology technology = null;
                    bool end = false;
                    do
                    {
                        if (productTree.Type == "Brand")
                        {
                            brand = Context.Set<Brand>().FirstOrDefault(x => x.Name == productTree.Name);
                            if (brand != null)
                            {
                                brandId = brand.Id;
                            }
                        }
                        if (productTree.Type == "Technology")
                        {
                            technology = Context.Set<Technology>().FirstOrDefault(x => (x.Name + " " + x.SubBrand).Trim() == productTree.Name.Trim());
                            if (technology != null)
                            {
                                technologyId = technology.Id;
                            }
                        }
                        if (productTree.parentId != 1000000)
                        {
                            productTree = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == productTree.parentId);
                        }
                        else
                        {
                            end = true;
                        }
                    } while (!end && productTree != null);

                    BrandTech brandTech = Context.Set<BrandTech>().FirstOrDefault(x => !x.Disabled && x.TechnologyId == technologyId && x.BrandId == brandId);
                    if (brandTech != null)
                    {
                        model.BrandTechId = brandTech.Id;
                    }
                    else
                    {
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
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            try
            {
                var model = Context.Set<Promo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                Promo promoCopy = AutomapperProfiles.PromoCopy(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status)
                {
                    return InternalServerError(new Exception(message));
                }

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
                // удалить дочерние промо
                var PromoesUnlink = Context.Set<Promo>().Where(p => p.MasterPromoId == model.Id && !p.Disabled).ToList();
                foreach (var childpromo in PromoesUnlink)
                {
                    childpromo.MasterPromoId = null;
                }
                await Context.SaveChangesAsync();

                PromoHelper.WritePromoDemandChangeIncident(Context, model, true);
                PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);

                //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                if (model.InOut.HasValue && model.InOut.Value)
                {
                    PromoHelper.DisableIncrementalPromo(Context, model);
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult PromoRSDelete(Guid key, TPMmode TPMmode)
        {
            try
            {
                Promo model = Context.Set<Promo>()
                    .Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                    .FirstOrDefault(g => g.Id == key);
                if (model == null)
                {
                    return NotFound();
                }
                if (TPMmode == TPMmode.RS)
                {
                    StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(Context);
                    if ((((DateTimeOffset)model.StartDate).AddDays(15) < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)model.EndDate) && model.BudgetYear == startEndModel.BudgetYear)
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo is not in the RS period" }));
                    }
                    if (TPMmode == TPMmode.RS && model.TPMmode == TPMmode.Current) //фильтр промо
                    {
                        List<string> blockStatuses = "Draft,Planned,Closed,Deleted,Finished,Started,Cancelled".Split(',').ToList();
                        if (blockStatuses.Contains(model.PromoStatus.SystemName))
                        {
                            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo in status: " + model.PromoStatus.Name + " cannot be deleted in the RS mode" }));
                        }
                        Promo presentRsPromo = Context.Set<Promo>().FirstOrDefault(g => g.Disabled && g.TPMmode == TPMmode.RS && g.Number == model.Number);
                        if (presentRsPromo is null)
                        {
                            model = RSmodeHelper.EditToPromoRS(Context, model, true, System.DateTime.Now);
                        }
                        //создавать удаленную копию PromoRS c сущностями, если ее нет
                    }
                    else if (TPMmode == TPMmode.RS && model.TPMmode == TPMmode.RS)
                    {
                        // удалить PromoRS  c сущностями
                        model = RSmodeHelper.DeleteToPromoRS(Context, model);
                    }
                }
                if (TPMmode == TPMmode.RA)
                {
                    StartEndModel startEndModel = RAmodeHelper.GetRAPeriod();
                    if (((DateTimeOffset)model.DispatchesStart) < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)model.DispatchesStart || model.BudgetYear != startEndModel.BudgetYear)
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo is not in the RA period" }));
                    }
                    if (TPMmode == TPMmode.RA && model.TPMmode == TPMmode.Current) //фильтр промо
                    {
                        List<string> blockStatuses = "Draft,Planned,Closed,Deleted,Finished,Started,Cancelled".Split(',').ToList();
                        if (blockStatuses.Contains(model.PromoStatus.SystemName))
                        {
                            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo in status: " + model.PromoStatus.Name + " cannot be deleted in the RA mode" }));
                        }
                        Promo presentRaPromo = Context.Set<Promo>().FirstOrDefault(g => g.Disabled && g.TPMmode == TPMmode.RA && g.Number == model.Number);
                        if (presentRaPromo is null)
                        {
                            model = RAmodeHelper.EditToPromoRA(Context, model, true, System.DateTime.Now);
                        }
                        //создавать удаленную копию PromoRS c сущностями, если ее нет
                    }
                    else if (TPMmode == TPMmode.RA && model.TPMmode == TPMmode.RA)
                    {
                        // удалить PromoRS  c сущностями
                        model = RAmodeHelper.DeleteToPromoRA(Context, model);
                    }
                }

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }
        private async Task DeletePromo(Guid key)
        {
            try
            {
                var model = Context.Set<Promo>().Find(key);
                if (model == null)
                {
                    throw new Exception("NotFound");
                }

                Promo promoCopy = AutomapperProfiles.PromoCopy(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status)
                {
                    throw new Exception(message);
                }

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
                await Context.SaveChangesAsync();

                PromoHelper.WritePromoDemandChangeIncident(Context, model, true);
                PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);

                //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                if (model.InOut.HasValue && model.InOut.Value)
                {
                    PromoHelper.DisableIncrementalPromo(Context, model);
                }
                //throw new Exception("NoContent");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
        public async Task<IHttpActionResult> ChangeStatus(Guid id, Guid promoNewStatusId)
        {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Delta<Promo> patch = new Delta<Promo>((new Promo()).GetType(), new string[] { "PromoStatusId" });
                    patch.TrySetPropertyValue("PromoStatusId", promoNewStatusId);

                    // если возвращается Update, то всё прошло без ошибок
                    var result = await Patch(id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>)
                    {
                        transaction.Commit();
                        return Json(new { success = true });
                    }
                    else
                    {
                        if (result is ExceptionResult exc)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return InternalServerError(GetExceptionMessage.GetInnerException(e));
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
        public async Task<IHttpActionResult> DeclinePromo([FromODataUri] Guid rejectPromoId, [FromODataUri] Guid rejectReasonId, [FromODataUri] string rejectComment)
        {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Promo promo = Context.Set<Promo>().Find(rejectPromoId);
                    RejectReason rejectreason = Context.Set<RejectReason>().Find(rejectReasonId);

                    if (promo == null)
                    {
                        throw new Exception("Promo not found");
                    }
                    else if (rejectreason == null)
                    {
                        throw new Exception("Reject reason not found");
                    }

                    PromoStatus draftPublishedStatus = Context.Set<PromoStatus>().First(n => n.SystemName == "DraftPublished");
                    Delta<Promo> patch = new Delta<Promo>(typeof(Promo), new string[] { "PromoStatusId", "RejectReasonId" });
                    patch.TrySetPropertyValue("PromoStatusId", draftPublishedStatus.Id);
                    patch.TrySetPropertyValue("RejectReasonId", rejectReasonId);

                    //Убираем Linked Promoes и убираем ссылки у дочерних промо
                    patch.TrySetPropertyValue("LinkedPromoes", string.Empty);
                    var PromoesUnlink = Context.Set<Promo>().Where(p => p.MasterPromoId == promo.Id && !p.Disabled).ToList();
                    foreach (var childpromo in PromoesUnlink)
                    {
                        childpromo.MasterPromoId = null;
                    }
                    // Для сохранения корректного значения после Patch
                    promo.DeviationCoefficient *= 100;
                    // если возвращается Update, то всё прошло без ошибок
                    var result = await Patch(promo.Id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>)
                    {
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

                        await Context.SaveChangesAsync();
                        transaction.Commit();

                        var jsonPromo = JsonConvert.SerializeObject(new { success = true, data = promo }, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                        return Content(HttpStatusCode.OK, jsonPromo);
                    }
                    else
                    {
                        ExceptionResult exc = result as ExceptionResult;
                        if (exc != null)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return InternalServerError(GetExceptionMessage.GetInnerException(e));
                }
            }
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetUserDashboardsCount([FromODataUri] String userrole)
        {
            switch (userrole)
            {
                case "KeyAccountManager":
                    return Content(HttpStatusCode.OK, UserDashboard.GetKeyAccountManagerCount(authorizationManager, Context));
                case "DemandPlanning":
                    return Content(HttpStatusCode.OK, UserDashboard.GetDemandPlanningCount(authorizationManager, Context));
                case "DemandFinance":
                    return Content(HttpStatusCode.OK, UserDashboard.GetDemandFinanceCount(authorizationManager, Context));
                case "CMManager":
                    return Content(HttpStatusCode.OK, UserDashboard.GetCMManagerCount(authorizationManager, Context));
                case "CustomerMarketing":
                    return Content(HttpStatusCode.OK, UserDashboard.GetCustomerMarketingCount(authorizationManager, Context));
                case "GAManager":
                    return Content(HttpStatusCode.OK, UserDashboard.GetGAManagerCount(authorizationManager, Context));
            }
            return Content(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(new { Error = "Fail to role" }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetLiveMetricsDashboard(ODataQueryOptions<Promo> options, int clientTreeId, string period)
        {

            return Content(HttpStatusCode.OK, LiveMetricsDashboard.GetLiveMetricsDashboard(authorizationManager, Context, clientTreeId, period));
        }

        private FilterContainer GetFilter(Delta<Promo> patch, string fieldName)
        {
            object fieldValue;
            if (patch.TryGetPropertyValue(fieldName, out fieldValue) && fieldValue != null)
            {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>((string)fieldValue);
                return result;
            }
            else
            {
                return null;
            }
        }

        private FilterContainer GetFilter(string fieldValue)
        {
            if (fieldValue != null)
            {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>(fieldValue);
                return result;
            }
            else
            {
                return null;
            }
        }

        private bool EntityExists(Guid key)
        {
            return Context.Set<Promo>().Count(e => e.Id == key) > 0;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<Promo> options)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(Promo), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(Promo)} dictionary",
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

            return Content(HttpStatusCode.OK, "success");
        }

        /// <summary>
        /// Экспорт календаря в эксель
        /// </summary>
        /// <param name="options"></param>
        /// <param name="data">clients - список id клиентов соответствующих фильтру на клиенте, year - год</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> ExportSchedule(ODataQueryOptions<Promo> options, ODataActionParameters data)
        {
            try
            {
                // TODO: Передавать фильтр в параметры задачи
                //var tsts = options.RawValues.Filter;
                //var tsts = JsonConvert.SerializeObject(options, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                UserInfo user = authorizationManager.GetCurrentUser();
                Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
                RoleInfo role = authorizationManager.GetCurrentRole();
                Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

                IEnumerable<int> clients = (IEnumerable<int>)data["clients"];

                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("clients", clients.ToList(), handlerData, visible: false, throwIfNotExists: false);

                //IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                //List<Promo> promoes = CastQueryToPromo(results);
                if (data.Count() > 1)
                {
                    HandlerDataHelper.SaveIncomingArgument("year", (int)data["year"], handlerData, visible: false, throwIfNotExists: false);
                }
                LoopHandler handler = new LoopHandler()
                {
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
                Context.LoopHandlers.Add(handler);
                await Context.SaveChangesAsync();
                return Content<string>(HttpStatusCode.OK, "Export task successfully created");
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private string GetUserName(string userName)
        {
            string[] userParts = userName.Split(new char[] { '/', '\\' });
            return userParts[userParts.Length - 1];
        }

        /// <summary>
        /// Преобразование записей в модель Promo
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private List<Promo> CastQueryToPromo(IQueryable records)
        {
            List<Promo> castedPromoes = new List<Promo>();
            Promo proxy = Context.Set<Promo>().Create<Promo>();
            foreach (var item in records)
            {
                if (item is IEntity<Guid>)
                {
                    var configuration = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Promo, Promo>().ReverseMap());
                    var mapper = configuration.CreateMapper();
                    Promo result = mapper.Map(item, proxy);
                    castedPromoes.Add(result);
                }
                else if (item is ISelectExpandWrapper)
                {
                    var property = item.GetType().GetProperty("Instance");
                    var instance = property.GetValue(item);
                    Promo val = null;
                    if (instance is Promo)
                    {
                        val = (Promo)instance;
                        castedPromoes.Add(val);
                    }
                }
            }
            return castedPromoes;
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                await CreateImportTask(fileName, "FullXLSXUpdateImportHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Чтение лога задачи 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult ReadPromoCalculatingLog(String promoId)
        {
            Guid promoGuid = Guid.Parse(promoId);
            String respond = null;
            int codeTo = 0;
            String opDataTo = null;
            string description = null;
            string status = null;
            try
            {
                Guid? handlerGuid = null;
                BlockedPromo bp = Context.Set<BlockedPromo>().FirstOrDefault(n => n.PromoBlockedStatusId == promoGuid && !n.Disabled);

                Promo promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoGuid && !x.Disabled);
                if (promo != null)
                {
                    handlerGuid = Guid.Parse(promo.BlockInformation.Split('_')[0]);
                    if (handlerGuid != null)
                    {
                        LoopHandler handler = Context.Set<LoopHandler>().FirstOrDefault(x => x.Id == handlerGuid);
                        if (handler != null)
                        {
                            description = handler.Description;
                            status = handler.Status;

                            string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                            string logFileName = String.Format("{0}.txt", handlerGuid);
                            string filePath = System.IO.Path.Combine(logDir, logFileName);
                            if (File.Exists(filePath))
                            {
                                respond = File.ReadAllText(filePath);
                            }
                            else
                            {
                                respond = "";
                            }
                        }
                    }
                }

                if (bp == null)
                {
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
            }
            catch (Exception e)
            {
                respond = e.Message;
                codeTo = 1;
            }

            return Json(new
            {
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
        public IHttpActionResult GetHandlerIdForBlockedPromo(string promoId)
        {
            var guidPromoId = Guid.Parse(promoId);
            var blockedPromo = Context.Set<BlockedPromo>().FirstOrDefault(n => n.PromoBlockedStatusId == guidPromoId && !n.Disabled);

            if (blockedPromo != null)
            {
                return Json(new
                {
                    success = true,
                    handlerId = blockedPromo.HandlerId
                });
            }
            else
            {
                return Json(new
                {
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
            BlockedPromo calculatingInfo = Context.Set<BlockedPromo>().Where(n => n.PromoBlockedStatusId == guidPromoId).OrderByDescending(n => n.CreateDate).FirstOrDefault();
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

        private async Task CreateImportTask(string fileName, string importHandler)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            ImportResultFilesModel resiltfile = new ImportResultFilesModel();
            ImportResultModel resultmodel = new ImportResultModel();

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
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

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ImportPromo).Name,
                //Name = "ProcessingHost.Handlers.Import." + importHandler,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                RunGroup = typeof(ImportPromo).Name,
                ExecutionPeriod = null,
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculateActual(Guid promoId, bool safe)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("needRedistributeLSV", true, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Actual, data, Context, promoId, safe);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        /// <summary>
        /// Создание отложенной задачи для расчета планового аплифта
        /// </summary>
        /// <param name="promo"></param>
        private async Task UpdateUplift(Promo promo)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);


            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Update uplift value",
                Name = "Module.Host.TPM.Handlers.UpdateUpliftHandler",
                ExecutionPeriod = null,
                RunGroup = "UpdateUplift",
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }

        private IEnumerable<Column> GetPromoROIExportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>() {
                new Column { Order = orderNumber++, Field = "Number", Header = "Promo ID", Quoting = false },
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
                new Column { Order = orderNumber++, Field = "ActualPromoNetUpliftPercent", Header = "Actual Promo Net Uplift Percent", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "SumInvoice", Header = "Invoice Total", Quoting = false,  Format = "0.00"  } };
            return columns;
        }

        //Out of date, Export in ROI controller
        [ClaimsAuthorize]
        public IHttpActionResult ExportPromoROIReportXLSX(ODataQueryOptions<Promo> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetPromoROIExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoROIReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool NeedRecalculatePromo(Promo newPromo, Promo oldPromo, List<Product> products)
        {
            bool needReacalculate = false;

            // Если есть различия в этих полях.
            if (oldPromo.ClientTreeId != newPromo.ClientTreeId
                    || oldPromo.MarsMechanicId != newPromo.MarsMechanicId
                    || oldPromo.MarsMechanicTypeId != newPromo.MarsMechanicTypeId
                    || oldPromo.SumInvoice != newPromo.SumInvoice
                    || oldPromo.ManualInputSumInvoice != newPromo.ManualInputSumInvoice
                    || oldPromo.MarsMechanicDiscount != newPromo.MarsMechanicDiscount
                    || oldPromo.StartDate != newPromo.StartDate
                    || oldPromo.EndDate != newPromo.EndDate
                    || oldPromo.DispatchesStart != newPromo.DispatchesStart
                    || oldPromo.DispatchesEnd != newPromo.DispatchesEnd
                    || oldPromo.BudgetYear != newPromo.BudgetYear
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
                    || (newPromo.PromoPriceIncrease != null && newPromo.PlanPromoUpliftPercentPI != null && Math.Round(newPromo.PromoPriceIncrease.PlanPromoUpliftPercent.Value, 2, MidpointRounding.AwayFromZero) != Math.Round(newPromo.PlanPromoUpliftPercentPI.Value, 2, MidpointRounding.AwayFromZero))
                    || (oldPromo.NeedRecountUplift != null && newPromo.NeedRecountUplift != null && oldPromo.NeedRecountUplift != newPromo.NeedRecountUplift)
                    || (oldPromo.PlanPromoUpliftPercentPI == null && newPromo.PlanPromoUpliftPercentPI != null)
                    || (oldPromo.NeedRecountUpliftPI != newPromo.NeedRecountUpliftPI)
                    || oldPromo.IsOnInvoice != newPromo.IsOnInvoice
                    || oldPromo.PlanAddTIMarketingApproved != newPromo.PlanAddTIMarketingApproved
                    || oldPromo.PromoStatus.Name.ToLower() == "draft"
                || !String.IsNullOrEmpty(newPromo.AdditionalUserTimestamp))
            {
                needReacalculate = true;
            }

            //if (newPromo.InOut.HasValue && newPromo.InOut.Value)
            //{
            List<Product> oldInOutProducts = PlanProductParametersCalculation.GetCheckedProducts(oldPromo, products);
            List<Product> newInOutProducts = PlanProductParametersCalculation.GetCheckedProducts(newPromo, products);

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

            if (!changed)
            {
                foreach (PromoProduct p in promoProducts)
                {
                    if (!products.Any(n => n.ZREP == p.ZREP))
                    {
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
                var productIds = new List<Guid>();
                var products = new List<Product>();

                inOutProductIds.ToList().ForEach(productId =>
                {
                    if (Guid.TryParse(productId, out Guid productGuidId))
                    {
                        productIds.Add(productGuidId);
                    }
                });

                products = Context.Set<Product>().Where(g => productIds.Contains(g.Id)).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, Product>()
                        .ForMember(pTo => pTo.AssortmentMatrices, opt => opt.Ignore())
                        .ForMember(pTo => pTo.BaseLines, opt => opt.Ignore())
                        .ForMember(pTo => pTo.IncreaseBaseLines, opt => opt.Ignore())
                        .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Ignore())
                        .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                        .ForMember(pTo => pTo.PriceLists, opt => opt.Ignore())
                        .ForMember(pTo => pTo.ProductChangeIncidents, opt => opt.Ignore())
                        .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                        .ForMember(pTo => pTo.RollingVolumes, opt => opt.Ignore());
                });
                var mapper = config.CreateMapper();
                var productsMap = mapper.Map<List<Product>>(products);
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = productsMap }, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
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
        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult GetRSPeriod(TPMmode TPMmode)
        {
            StartEndModel startEndModel = new StartEndModel();
            if (TPMmode == TPMmode.RS)
            {
                startEndModel = RSPeriodHelper.GetRSPeriod(Context);
            }
            if (TPMmode == TPMmode.RA)
            {
                startEndModel = RAmodeHelper.GetRAPeriod();
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, startEndModel }));
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
        private async Task<string> DeleteChildPromoes(Guid modelId, UserInfo user)
        {
            string childmessage = string.Empty;
            var ChildPromoes = Context.Set<Promo>().Where(p => p.MasterPromoId == modelId && !p.Disabled).ToList();
            var statuses = Context.Set<PromoStatus>().ToList();
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
                    using (PromoStateContext childStateContext = new PromoStateContext(Context, ChildPromo))
                    {
                        if (childStateContext.ChangeState(ChildPromo, PromoStates.Deleted, "System", out childmessage))
                        {
                            ChildPromo.DeletedDate = DateTime.Now;
                            ChildPromo.Disabled = true;
                            ChildPromo.PromoStatusId = statuses.FirstOrDefault(e => e.SystemName == "Deleted").Id;

                            List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == ChildPromo.Id && !x.Disabled).ToList();
                            foreach (PromoProduct promoProduct in promoProductToDeleteList)
                            {
                                promoProduct.DeletedDate = System.DateTime.Now;
                                promoProduct.Disabled = true;
                            }
                            ChildPromo.NeedRecountUplift = true;
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
                            // удалить дочерние промо
                            var PromoesUnlink = Context.Set<Promo>().Where(p => p.MasterPromoId == ChildPromo.Id && !p.Disabled).ToList();
                            foreach (var childpromo in PromoesUnlink)
                            {
                                childpromo.MasterPromoId = null;
                            }
                            await Context.SaveChangesAsync();

                            PromoHelper.WritePromoDemandChangeIncident(Context, ChildPromo, true);
                            PromoCalculateHelper.RecalculateBudgets(ChildPromo, user, Context);
                            PromoCalculateHelper.RecalculateBTLBudgets(ChildPromo, user, Context, safe: true);

                            //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                            if (ChildPromo.InOut.HasValue && ChildPromo.InOut.Value)
                            {
                                PromoHelper.DisableIncrementalPromo(Context, ChildPromo);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            return childmessage;
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов
        /// </summary>
        /// <param name="promoSupportPromoIds">список ID подстатей</param>
        /// <param name="calculatePlanCostTE">Необходимо ли пересчитывать значения плановые Cost TE</param>
        /// <param name="calculateFactCostTE">Необходимо ли пересчитывать значения фактические Cost TE</param>
        /// <param name="calculatePlanCostProd">Необходимо ли пересчитывать значения плановые Cost Production</param>
        /// <param name="calculateFactCostProd">Необходимо ли пересчитывать значения фактические Cost Production</param>
        public void CalculateBTLBudgetsCreateTask(string btlId, List<Guid> unlinkedPromoIds = null)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("BTLId", btlId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            if (unlinkedPromoIds != null)
            {
                HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIds, data, visible: false, throwIfNotExists: false);
            }

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.BTL, data, Context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }
    }
}
