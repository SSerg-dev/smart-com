﻿using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
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

namespace Module.Frontend.TPM.Controllers
{
    public class BudgetSubItemClientTreesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BudgetSubItemClientTreesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<BudgetSubItemClientTree> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<BudgetSubItemClientTree> query = Context.Set<BudgetSubItemClientTree>();

            return query;
        }

        // [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<BudgetSubItemClientTree> GetBudgetSubItemClientTree([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        // [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<BudgetSubItemClientTree> GetBudgetSubItemClientTrees()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<BudgetSubItemClientTree> GetFilteredData(ODataQueryOptions<BudgetSubItemClientTree> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<BudgetSubItemClientTree>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<BudgetSubItemClientTree>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<BudgetSubItemClientTree> patch)
        {
            var model = Context.Set<BudgetSubItemClientTree>().Find(key);
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
        [HttpPost]
        public async Task<IHttpActionResult> Post(System.Guid selectedSubItemId)
        {
            try
            {
                // Старый список выбранных клиентов
                IQueryable<int> oldClientTreeIds = Context.Set<BudgetSubItemClientTree>().Where(x => x.BudgetSubItemId == selectedSubItemId).Select(y => y.ClientTreeId);

                // Новый список выбранных клиентов BudgetSubItems
                string clientsToAdd = Request.Content.ReadAsStringAsync().Result;
                IEnumerable<int> newClientTreeIds = clientsToAdd.Split(';').Select(Int32.Parse);

                // Выборка списоков Id по ClientTree для создания и удаления связей в БД
                var addExceptResult = newClientTreeIds.Except(oldClientTreeIds).ToArray();
                var removeExceptResult = oldClientTreeIds.Except(newClientTreeIds).ToArray();

                // Удаление записей
                string deleteScript = String.Empty;         // скрипт для удаления всех связей не выбранных ClientTree с текущим Event
                foreach (int clientId in removeExceptResult)
                {
                    // добавление строк в скрипт
                    deleteScript += String.Format("DELETE FROM [DefaultSchemaSetting].[BudgetSubItemClientTree] WHERE [BudgetSubItemId] = '{0}' and [ClientTreeId] = {1}", selectedSubItemId, clientId);
                }
                // выполнить скрипт, если он не пустой
                if (!String.IsNullOrEmpty(deleteScript))
                {
                    Context.ExecuteSqlCommand(deleteScript);
                }

                // Добавление записей
                foreach (int clientId in addExceptResult)
                {
                    BudgetSubItemClientTree budgetSubItemClientTree = new BudgetSubItemClientTree()
                    {
                        ClientTreeId = clientId,
                        BudgetSubItemId = selectedSubItemId
                    };
                    Context.Set<BudgetSubItemClientTree>().Add(budgetSubItemClientTree);
                }

                await Context.SaveChangesAsync();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<BudgetSubItemClientTree> patch)
        {
            try
            {
                Context.Set<BudgetSubItemClientTree>().AsNoTracking();
                var model = Context.Set<BudgetSubItemClientTree>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                await Context.SaveChangesAsync();

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
            try
            {
                var model = Context.Set<BudgetSubItemClientTree>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                Context.Set<BudgetSubItemClientTree>().Remove(model);
                await Context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<BudgetSubItemClientTree>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This BudgetSubItemClientTree has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }

}
