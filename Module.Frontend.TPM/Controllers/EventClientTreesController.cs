using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class EventClientTreesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public EventClientTreesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<EventClientTree> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<EventClientTree> query = Context.Set<EventClientTree>();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<EventClientTree> GetEventClientTree([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<EventClientTree> GetEventClientTrees()
        {
            return GetConstraintedQuery();
        }


        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<EventClientTree> patch)
        {
            var model = Context.Set<EventClientTree>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                Context.SaveChanges();
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
        public IHttpActionResult Post(EventClientTree model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<EventClientTree>().Create<EventClientTree>();
            var result = (EventClientTree)Mapper.Map(model, proxy, typeof(EventClientTree), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<EventClientTree>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Post(System.Guid eventId)
        {
            try
            {
                // Старый список выбранных клиентов
                IQueryable<int> oldClientTreeIds = Context.Set<EventClientTree>().Where(x => x.EventId == eventId).Select(y => y.ClientTreeId);

                // Новый список выбранных клиентов Event
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
                    deleteScript += String.Format("DELETE FROM [dbo].[EventClientTree] WHERE [EventId] = '{0}' and [ClientTreeId] = {1}", eventId, clientId);
                }
                // выполнить скрипт, если он не пустой
                if (!String.IsNullOrEmpty(deleteScript))
                {
                    Context.Database.ExecuteSqlCommand(deleteScript);
                }

                // Добавление записей
                foreach (int clientId in addExceptResult)
                {
                    EventClientTree eventClientTree = new EventClientTree()
                    {
                        ClientTreeId = clientId,
                        EventId = eventId
                    };
                    Context.Set<EventClientTree>().Add(eventClientTree);
                }

                Context.SaveChanges();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<EventClientTree> patch)
        {
            try
            {
                var model = Context.Set<EventClientTree>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                Context.SaveChanges();

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
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                EventClientTree model = Context.Set<EventClientTree>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                string deleteScript = String.Format("DELETE FROM [dbo].[EventClientTree] WHERE [Id] = '{0}'", model.Id.ToString());
                Context.Database.ExecuteSqlCommand(deleteScript);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<EventClientTree>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Event has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}
