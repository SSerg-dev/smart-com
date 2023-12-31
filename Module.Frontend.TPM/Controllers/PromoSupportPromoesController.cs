﻿using AutoMapper;
using Core.Dependency;
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
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
    public class PromoSupportPromoesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoSupportPromoesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoSupportPromo> GetConstraintedQuery(TPMmode TPMmode = TPMmode.Current)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoSupportPromo> query = Context.Set<PromoSupportPromo>();

            switch (TPMmode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.Promo.Number }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    break;
                case TPMmode.RA:
                    query = query.GroupBy(x => new { x.Promo.Number }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    break;
            }
            return query.Where(q => !q.Disabled);

        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoSupportPromo> GetPromoSupportPromo([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoSupportPromo> GetPromoSupportPromoes(TPMmode TPMmode = TPMmode.Current)
        {
            return GetConstraintedQuery(TPMmode);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoSupportPromo> GetPromoSupportPromoes([FromODataUri] string PromoId, TPMmode TPMmode = TPMmode.Current)
        {
            var promosupports = GetConstraintedQuery(TPMmode);
            var promoNumber = int.Parse(PromoId);
            var promo = Context.Set<Promo>().Where(x => x.Number == promoNumber).FirstOrDefault();
            var result = promosupports.Where(x => x.PromoId == promo.Id);
            return result;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoSupportPromo> GetFilteredData(ODataQueryOptions<PromoSupportPromo> options)
        {
            var bodyText = HttpContext.Current.Request.GetRequestBody();
            var query = JsonHelper.IsValueExists(bodyText, "TPMmode")
                 ? GetConstraintedQuery(JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode"))
                 : GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoSupportPromo>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoSupportPromo>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<PromoSupportPromo> patch)
        {
            var model = Context.Set<PromoSupportPromo>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
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
        public async Task<IHttpActionResult> Post(PromoSupportPromo model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<PromoSupportPromo>().Create<PromoSupportPromo>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<PromoSupportPromo>().Add(result);

            try
            {
                // разница между промо в подстатье должно быть меньше 2 периодов (8 недель)
                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                var diffBetweenPromoInDays = settingsManager.GetSetting<int>("DIFF_BETWEEN_PROMO_IN_DAYS", 7 * 12);
                Promo promo = Context.Set<Promo>().Find(result.PromoId);
                bool bigDifference = Context.Set<PromoSupportPromo>().Any(n => n.PromoSupportId == result.PromoSupportId
                        && DbFunctions.DiffDays(n.Promo.StartDate.Value, promo.EndDate.Value).Value > diffBetweenPromoInDays && !n.Disabled);

                if (bigDifference)
                    throw new Exception("The difference between the dates of the promo should be less than 3 periods");

                await Context.SaveChangesAsync();

                CalculateBudgetsCreateTask(new List<Guid>() { result.Id });
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> PromoSuportPromoPost(Guid promoSupportId, TPMmode TPMmode)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    var diffBetweenPromoInDays = settingsManager.GetSetting<int>("DIFF_BETWEEN_PROMO_IN_DAYS", 7 * 12);
                    List<string> promoIdsList = new List<string>();
                    string promoIds = Request.Content.ReadAsStringAsync().Result;
                    if (promoIds != null)
                    {
                        promoIdsList = JsonConvert.DeserializeObject<List<string>>(promoIds);
                    }

                    List<Guid> guidPromoIds = promoIdsList.Select(f => Guid.Parse(f)).ToList();

                    List<PromoSupportPromo> promoSupportPromoes = Context.Set<PromoSupportPromo>()
                        .Include(g => g.PromoSupport)
                        .Include(g => g.Promo.BTLPromoes)
                        .Include(g => g.Promo.IncrementalPromoes)
                        .Include(g => g.Promo.PromoProductTrees)
                        .Include(x => x.Promo.PromoProducts.Select(y => y.PromoProductsCorrections))
                        .Where(psp => psp.PromoSupportId == promoSupportId && !psp.Disabled)
                        .ToList();
                    bool isAllCurrent = promoSupportPromoes.All(g => g.TPMmode == TPMmode.Current);
                    bool isAllRS = promoSupportPromoes.All(g => g.TPMmode == TPMmode.RS);
                    bool isAllRA = promoSupportPromoes.All(g => g.TPMmode == TPMmode.RA);
                    List<PromoSupportPromo> promoSupportPromoesRS = new List<PromoSupportPromo>();
                    List<PromoSupportPromo> promoSupportPromoesRA = new List<PromoSupportPromo>();
                    if (TPMmode == TPMmode.RS)
                    {
                        if (TPMmode == TPMmode.RS && isAllCurrent)
                        {
                            promoSupportPromoes = RSmodeHelper.EditToPromoSupportPromoRS(Context, promoSupportPromoes);
                        }
                        if (!isAllCurrent && !isAllRS && TPMmode == TPMmode.Current)
                        {
                            promoSupportPromoesRS = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.RS).ToList();
                            // убираем RSы
                            promoSupportPromoes = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.Current).ToList();
                        }
                        else
                        {
                            // убираем currentы
                            promoSupportPromoes = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.RS).ToList();
                        }
                    }
                    if (TPMmode == TPMmode.RA)
                    {
                        if (TPMmode == TPMmode.RA && isAllCurrent)
                        {
                            promoSupportPromoes = RAmodeHelper.EditToPromoSupportPromoRA(Context, promoSupportPromoes);
                        }
                        if (!isAllCurrent && !isAllRA && TPMmode == TPMmode.Current)
                        {
                            promoSupportPromoesRA = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.RA).ToList();
                            // убираем RSы
                            promoSupportPromoes = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.Current).ToList();
                        }
                        else
                        {
                            // убираем currentы
                            promoSupportPromoes = promoSupportPromoes.Where(g => g.TPMmode == TPMmode.RA).ToList();
                        }
                    }
                    foreach (var id in guidPromoIds)
                    {
                        Promo promo = Context.Set<Promo>().Find(id);
                        // закрытые не берем
                        if (promo.PromoStatus.SystemName != "Closed")
                        {
                            // разница между промо в подстатье должно быть меньше 2 периодов (8 недель)
                            bool bigDifference = promoSupportPromoes.Any(n => (promo.EndDate.Value - n.Promo.StartDate.Value).Days > diffBetweenPromoInDays);

                            if (bigDifference)
                                throw new Exception("The difference between the dates of the promo should be less than 3 periods");
                            if (TPMmode == TPMmode.RS)
                            {
                                if (promo.TPMmode == TPMmode.Current)
                                {
                                    promo = RSmodeHelper.EditToPromoRS(Context, promo);
                                    PromoSupportPromo psp = new PromoSupportPromo
                                    {
                                        PromoSupportId = promoSupportId,
                                        PromoId = promo.Id,
                                        TPMmode = promo.TPMmode
                                    };
                                    promo.PromoSupportPromoes.Add(psp);
                                }
                                else
                                {
                                    PromoSupportPromo psp = new PromoSupportPromo
                                    {
                                        PromoSupportId = promoSupportId,
                                        PromoId = id,
                                        TPMmode = promo.TPMmode
                                    };
                                    Context.Set<PromoSupportPromo>().Add(psp);
                                }
                            }
                            else if (TPMmode == TPMmode.RA)
                            {
                                if (promo.TPMmode == TPMmode.Current)
                                {
                                    promo = RAmodeHelper.EditToPromoRA(Context, promo);
                                    PromoSupportPromo psp = new PromoSupportPromo
                                    {
                                        PromoSupportId = promoSupportId,
                                        PromoId = promo.Id,
                                        TPMmode = promo.TPMmode
                                    };
                                    promo.PromoSupportPromoes.Add(psp);
                                }
                                else
                                {
                                    PromoSupportPromo psp = new PromoSupportPromo
                                    {
                                        PromoSupportId = promoSupportId,
                                        PromoId = id,
                                        TPMmode = promo.TPMmode
                                    };
                                    Context.Set<PromoSupportPromo>().Add(psp);
                                }
                            }
                            else
                            {
                                Promo promoModeRS = Context.Set<Promo>()
                                    .Include(g => g.PromoSupportPromoes)
                                    .FirstOrDefault(g => g.Number == promo.Number && g.TPMmode == TPMmode.RS);
                                PromoSupportPromo psp = new PromoSupportPromo
                                {
                                    PromoSupportId = promoSupportId,
                                    PromoId = id,
                                    TPMmode = promo.TPMmode
                                };
                                Context.Set<PromoSupportPromo>().Add(psp);
                                if (promoModeRS != null)
                                {
                                    if (promoModeRS.PromoSupportPromoes.Any(g => g.PromoSupportId == promoSupportId && !g.Disabled))
                                    {
                                        //по идее надо - CalculateBudgetsCreateTask но это тормознет сохранение в Current
                                    }
                                    else
                                    {
                                        PromoSupportPromo psp1 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promoModeRS.Id,
                                            TPMmode = promo.TPMmode
                                        };
                                        Context.Set<PromoSupportPromo>().Add(psp1);
                                    }
                                }
                                else if (promoSupportPromoesRS.Count > 0)
                                {
                                    Promo promoRS = Context.Set<Promo>().FirstOrDefault(g => g.Number == promo.Number && g.TPMmode == TPMmode.RS);
                                    if (promoRS != null)
                                    {
                                        PromoSupportPromo psp1 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promoRS.Id,
                                            TPMmode = TPMmode.RS
                                        };
                                        Context.Set<PromoSupportPromo>().Add(psp1);
                                    }
                                    else
                                    {
                                        promo = RSmodeHelper.EditToPromoRS(Context, promo);
                                        PromoSupportPromo psp2 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promo.Id,
                                            TPMmode = promo.TPMmode
                                        };
                                        promo.PromoSupportPromoes.Add(psp2);
                                    }

                                }
                                Promo promoModeRA = Context.Set<Promo>()
                                    .Include(g => g.PromoSupportPromoes)
                                    .FirstOrDefault(g => g.Number == promo.Number && g.TPMmode == TPMmode.RA);
                                if (promoModeRA != null)
                                {
                                    if (promoModeRA.PromoSupportPromoes.Any(g => g.PromoSupportId == promoSupportId && !g.Disabled))
                                    {
                                        //по идее надо - CalculateBudgetsCreateTask но это тормознет сохранение в Current
                                    }
                                    else
                                    {
                                        PromoSupportPromo psp1 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promoModeRA.Id,
                                            TPMmode = promo.TPMmode
                                        };
                                        Context.Set<PromoSupportPromo>().Add(psp1);
                                    }
                                }
                                else if (promoSupportPromoesRS.Count > 0)
                                {
                                    Promo promoRA = Context.Set<Promo>().FirstOrDefault(g => g.Number == promo.Number && g.TPMmode == TPMmode.RA);
                                    if (promoRA != null)
                                    {
                                        PromoSupportPromo psp1 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promoRA.Id,
                                            TPMmode = TPMmode.RS
                                        };
                                        Context.Set<PromoSupportPromo>().Add(psp1);
                                    }
                                    else
                                    {
                                        promo = RSmodeHelper.EditToPromoRS(Context, promo);
                                        PromoSupportPromo psp2 = new PromoSupportPromo
                                        {
                                            PromoSupportId = promoSupportId,
                                            PromoId = promo.Id,
                                            TPMmode = promo.TPMmode
                                        };
                                        promo.PromoSupportPromoes.Add(psp2);
                                    }

                                }
                            }
                        }
                    }
                    await Context.SaveChangesAsync();

                    CalculateBudgetsCreateTask(new List<Guid>() { promoSupportId }, null, TPMmode);

                    transaction.Commit();
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    // обработка при создании дублирующей записи
                    SqlException exc = e.GetBaseException() as SqlException;
                    if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo has already associated." }));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
                    }
                }
            }
        }

        /// <summary>
        /// Управление списком PromoSupportPromo из формы Promo Support
        /// </summary>
        /// <param name="psp">JSON строка выбранных промо</param>
        /// <param name="promoSupportId">ID Promo Support</param>
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> ChangeListPSP(Guid promoSupportId)
        {
            try
            {
                string pspJSON = Request.Content.ReadAsStringAsync().Result;
                List<PromoSupportPromo> newList = JsonConvert.DeserializeObject<List<PromoSupportPromo>>(pspJSON);
                List<PromoSupportPromo> oldList = Context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == promoSupportId && !n.Disabled).ToList();
                // список Id подстатей/промо, которые необходимо пересчитать
                string changedPromoSupportPromoIds = "";

                // список Id промо, которые были откреплены
                List<Guid> deletedPromoIds = new List<Guid>();

                for (int i = 0; i < newList.Count; i++)
                {
                    if (newList[i].Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        // если новая запись - добавляем
                        newList[i].Id = Guid.NewGuid();
                        newList[i].Promo = Context.Set<Promo>().Find(newList[i].PromoId);
                        newList[i].PromoSupport = Context.Set<PromoSupport>().Find(newList[i].PromoSupportId);

                        ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                        var diffBetweenPromoInDays = settingsManager.GetSetting<int>("DIFF_BETWEEN_PROMO_IN_DAYS", 7 * 12);
                        DateTimeOffset endPromoDate = newList[i].Promo.EndDate.Value;
                        bool bigDifference = Context.Set<PromoSupportPromo>().Any(n => n.PromoSupportId == promoSupportId
                                        && DbFunctions.DiffDays(n.Promo.StartDate.Value, endPromoDate).Value > diffBetweenPromoInDays && !n.Disabled);

                        if (bigDifference)
                            throw new Exception("The difference between the dates of the promo should be less than 3 periods");

                        Context.Set<PromoSupportPromo>().Add(newList[i]);
                    }
                    else
                    {
                        // если промо было уже прикреплено, то обновляем
                        PromoSupportPromo pspForUpdate = oldList.First(n => n.Id == newList[i].Id);
                        pspForUpdate.FactCalculation = newList[i].FactCalculation;
                        pspForUpdate.FactCostProd = newList[i].FactCostProd;
                        pspForUpdate.PlanCalculation = newList[i].PlanCalculation;
                        pspForUpdate.PlanCostProd = newList[i].PlanCostProd;

                        Context.Entry(pspForUpdate).State = EntityState.Modified;

                        // удаляем из списка
                        oldList.Remove(pspForUpdate);
                        // сразу формируем новый список, с обновленными данными
                        newList[i] = pspForUpdate;
                    }
                }

                // оставшиеся удаляем из БД
                foreach (PromoSupportPromo p in oldList)
                {
                    p.Disabled = true;
                    p.DeletedDate = DateTime.Now;

                    deletedPromoIds.Add(p.PromoId);
                }

                await Context.SaveChangesAsync();

                // Id могли обновится, поэтому записываем после сохранения
                foreach (PromoSupportPromo p in newList)
                {
                    changedPromoSupportPromoIds += p.Id + ";";
                }

                CalculateBudgetsCreateTask(new List<Guid>() { promoSupportId }, deletedPromoIds);

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, list = newList }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<PromoSupportPromo> patch)
        {
            try
            {
                var model = Context.Set<PromoSupportPromo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                await Context.SaveChangesAsync();

                CalculateBudgetsCreateTask(new List<Guid>() { key });

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
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            //считаем что в этот метод только из Current попадает
            try
            {
                var model = Context.Set<PromoSupportPromo>()
                    .Include(g => g.Promo)
                    .FirstOrDefault(g => g.Id == key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                await Context.SaveChangesAsync();

                CalculateBudgetsCreateTask(new List<Guid>() { model.PromoSupportId }, new List<Guid>() { model.PromoId });
                //CalculateBudgetsCreateTask(key.ToString(), true, true, true, true);

                PromoSupportPromo promoSupportPromoRS = Context.Set<PromoSupportPromo>()
                    .Include(g => g.Promo)
                    .FirstOrDefault(g => g.Promo.Number == model.Promo.Number && g.TPMmode == TPMmode.RS);
                if (promoSupportPromoRS != null)
                {
                    promoSupportPromoRS.DeletedDate = System.DateTime.Now;
                    promoSupportPromoRS.Disabled = true;
                    await Context.SaveChangesAsync();
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }
        }

        /// <summary>
        /// Получить привязанные подстатьи к промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetLinkedSubItems(Guid promoId)
        {
            try
            {
                // IDs привязанных подстатей к промо
                Guid[] existsPromoSupportsId = Context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled)
                    .Select(n => n.PromoSupport.Id).Distinct().ToArray();

                return Json(new
                {
                    success = true,
                    data = existsPromoSupportsId
                });
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        /// <summary>
        /// Изменение привязанных подстатей из формы PROMO
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="subItemsIds">список ID подстатей</param>
        /// <param name="budgetName">имя бюджета</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> ManageSubItems(Guid promoId, string budgetName)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    List<Guid> subItemsIdsList = new List<Guid>();
                    List<string> tmp = new List<string>();
                    Promo promo = Context.Set<Promo>().Find(promoId);

                    string subItemsIds = Request.Content.ReadAsStringAsync().Result;
                    if (subItemsIds != null)
                    {
                        tmp = JsonConvert.DeserializeObject<List<string>>(subItemsIds);
                    }
                    tmp.ForEach(id =>
                    {
                        Guid itemId = Guid.Parse(id);
                        subItemsIdsList.Add(itemId);
                    });

                    // находим прежние записи, если они остались то ислючаем их из нового списка
                    // иначе удаляем
                    var oldRecords = Context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId
                        && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf(budgetName.ToLower()) >= 0 && !n.Disabled);

                    // список Id подстатей/промо, которые были откреплены
                    List<Guid> deletedPromoIds = new List<Guid>();
                    // список Id подстатей, которые нужно пересчитать
                    List<Guid> promoSupportForRecalc = new List<Guid>();

                    foreach (PromoSupportPromo rec in oldRecords)
                    {
                        int index = subItemsIdsList.IndexOf(rec.PromoSupportId);

                        if (index >= 0)
                        {
                            subItemsIdsList.RemoveAt(index);
                        }
                        else
                        {
                            rec.DeletedDate = System.DateTime.Now;
                            rec.Disabled = true;
                            await Context.SaveChangesAsync();

                            deletedPromoIds.Add(rec.PromoId);
                            promoSupportForRecalc.Add(rec.PromoSupportId);
                        }
                    }

                    // привязываем новые подстатьи к промо
                    foreach (Guid promoSupportId in subItemsIdsList)
                    {
                        // разница между промо в подстатье должно быть меньше 2 периодов (8 недель)
                        ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                        var diffBetweenPromoInDays = settingsManager.GetSetting<int>("DIFF_BETWEEN_PROMO_IN_DAYS", 7 * 12);
                        bool bigDifference = Context.Set<PromoSupportPromo>().Any(n => n.PromoSupportId == promoSupportId
                            && (DbFunctions.DiffDays(n.Promo.StartDate.Value, promo.EndDate.Value).Value > diffBetweenPromoInDays
                                || DbFunctions.DiffDays(n.Promo.StartDate.Value, promo.EndDate.Value).Value < -diffBetweenPromoInDays)
                            && !n.Disabled);

                        if (bigDifference)
                            throw new Exception("The difference between the dates of the promo in the promo support should be less than 3 periods");
                        else
                        {
                            PromoSupportPromo psp = new PromoSupportPromo()
                            {
                                PromoId = promoId,
                                PromoSupportId = promoSupportId,
                                FactCalculation = 0,
                                PlanCalculation = 0
                            };

                            Context.Set<PromoSupportPromo>().Add(psp);
                            await Context.SaveChangesAsync();

                            promoSupportForRecalc.Add(promoSupportId);
                        }
                    }

                    await Context.SaveChangesAsync();
                    CalculateBudgetsCreateTask(promoSupportForRecalc, deletedPromoIds);
                    transaction.Commit();

                    return Json(new { success = true });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return GetErorrRequest(e);
                }
            }
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов
        /// </summary>
        /// <param name="promoSupportPromoIds">список ID подстатей</param>
        /// <param name="calculatePlanCostTE">Необходимо ли пересчитывать значения плановые Cost TE</param>
        /// <param name="calculateFactCostTE">Необходимо ли пересчитывать значения фактические Cost TE</param>
        /// <param name="calculatePlanCostProd">Необходимо ли пересчитывать значения плановые Cost Production</param>
        /// <param name="calculateFactCostProd">Необходимо ли пересчитывать значения фактические Cost Production</param>
        private void CalculateBudgetsCreateTask(List<Guid> promoSupportIds, List<Guid> unlinkedPromoIds = null, TPMmode tPMmode = TPMmode.Current)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            string promoSupportIdsString = FromListToString(promoSupportIds);
            string unlinkedPromoIdsString = FromListToString(unlinkedPromoIds);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoSupportIds", promoSupportIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TPMmode", tPMmode, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Budgets, data, Context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        private string FromListToString(List<Guid> list)
        {
            string result = "";

            if (list != null)
                foreach (Guid el in list.Distinct())
                    result += el + ";";

            return result;
        }

        /// <summary>
        /// Получить суммы по статьям
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="fact">True если фактические значения, иначе плановые</param>
        /// <param name="bugdetName">Наименование бюджета</param>
        /// <param name="itemsName">Список статей (через ';')</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetValuesForItems(Guid promoId, bool fact, string budgetName, string itemsName = null, bool costProd = false)
        {
            // суммы для статей
            List<object> sums = new List<object>();
            // общая сумма (бюджет)
            double sumBudget = 0;

            IQueryable<IGrouping<string, PromoSupportPromo>> groups;

            // если точно указаны статьи, то выбераем только их
            if (itemsName != null && itemsName.Length > 0)
            {
                List<string> itemsIdsList = new List<string>();

                itemsIdsList = itemsName.Split(';').Select(n => n.ToLower()).ToList();

                groups = Context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled
                    && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf(budgetName.ToLower()) >= 0
                    && itemsIdsList.Any(s => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf(s) >= 0))
                .GroupBy(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name);
            }
            else
            {
                groups = Context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled
                    && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf(budgetName.ToLower()) >= 0)
                .GroupBy(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name);
            }

            // группа - это 1 статья
            foreach (var g in groups)
            {
                double sumItem;

                if (costProd)
                    sumItem = fact ? g.Sum(n => n.FactCostProd) : g.Sum(n => n.PlanCostProd);
                else
                    sumItem = fact ? g.Sum(n => n.FactCalculation) : g.Sum(n => n.PlanCalculation);

                sums.Add(new { key = g.Key, value = sumItem });
                sumBudget += sumItem;
            }

            return Json(new
            {
                success = true,
                data = sums
            });
        }

        public static IEnumerable<Column> GetExportSettingsTICosts()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Promo.Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 1, Field = "Promo.Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 2, Field = "Promo.BrandTech.BrandsegTechsub", Header = "Brandtech", Quoting = false },
                new Column() { Order = 2, Field = "PlanCalculation", Header = "Plan Cost TE Total", Quoting = false },
                new Column() { Order = 2, Field = "FactCalculation", Header = "Actual Cost TE Total", Quoting = false },
                new Column() { Order = 3, Field = "Promo.EventName", Header = "Event", Quoting = false },
                new Column() { Order = 4, Field = "Promo.StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 5, Field = "Promo.EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 6, Field = "Promo.PromoStatus.Name", Header = "Status", Quoting = false },
            };
            return columns;
        }

        public static IEnumerable<Column> GetExportSettingsCostProd()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Promo.Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 1, Field = "Promo.Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 2, Field = "Promo.BrandTech.BrandsegTechsub", Header = "Brandtech", Quoting = false },
                new Column() { Order = 2, Field = "PlanCostProd", Header = "Plan Cost Production", Quoting = false },
                new Column() { Order = 2, Field = "FactCostProd", Header = "Actual Cost Production", Quoting = false },
                new Column() { Order = 3, Field = "Promo.EventName", Header = "Event", Quoting = false },
                new Column() { Order = 4, Field = "Promo.StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 5, Field = "Promo.EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 6, Field = "Promo.PromoStatus.Name", Header = "Status", Quoting = false },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoSupportPromo> options, string section = "")
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            string getColumnMethod = section == "ticosts"
                                        ? nameof(PromoSupportPromoesController.GetExportSettingsTICosts)
                                        : nameof(PromoSupportPromoesController.GetExportSettingsCostProd);
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoSupportPromo), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoSupportPromoesController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", getColumnMethod, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PromoSupportPromo)} dictionary",
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

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoSupportPromo>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoSupportPromo has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> PromoSupportPromoDelete(Guid key, TPMmode TPMmode)
        {
            try
            {
                var supportPromos = Context.Set<PromoSupportPromo>()
                    .Where(x => x.Id == key && !x.Disabled)
                    .ToList();

                if (supportPromos == null)
                {
                    return NotFound();
                }

                if (TPMmode == TPMmode.RS && supportPromos[0].TPMmode == TPMmode.Current) //фильтр промо
                {
                    RSmodeHelper.EditToPromoSupportPromoRS(Context, supportPromos, true, System.DateTime.Now);
                }
                else if (TPMmode == TPMmode.RA && supportPromos[0].TPMmode == TPMmode.Current)
                {
                    RAmodeHelper.EditToPromoSupportPromoRA(Context, supportPromos, true, System.DateTime.Now);
                }
                else
                {
                    supportPromos[0].DeletedDate = System.DateTime.Now;
                    supportPromos[0].Disabled = true;
                }
                await Context.SaveChangesAsync();

                CalculateBudgetsCreateTask(new List<Guid>() { supportPromos[0].PromoSupportId }, new List<Guid>() { supportPromos[0].PromoId });
                await Context.SaveChangesAsync();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }
    }
}