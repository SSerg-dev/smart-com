using AutoMapper;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class NonPromoSupportBrandTechesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public NonPromoSupportBrandTechesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<NonPromoSupportBrandTech> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<NonPromoSupportBrandTech> query = Context.Set<NonPromoSupportBrandTech>().Where(e => !e.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<NonPromoSupportBrandTech> GetNonPromoSupportBrandTech([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<NonPromoSupportBrandTech> GetNonPromoSupportBrandTeches()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<NonPromoSupportBrandTech> patch)
        {
            var model = Context.Set<NonPromoSupportBrandTech>().Find(key);
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
        public IHttpActionResult Post(NonPromoSupportBrandTech model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<NonPromoSupportBrandTech>().Create<NonPromoSupportBrandTech>();
            var result = (NonPromoSupportBrandTech)Mapper.Map(model, proxy, typeof(NonPromoSupportBrandTech), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<NonPromoSupportBrandTech>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<NonPromoSupportBrandTech>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<NonPromoSupportBrandTech>().Count(e => e.Id == key) > 0;
        }

        /// <summary>
        /// Управление списком NonPromoSupportBrandTech
        /// </summary>
        /// <param name="nonPromoSupportId">ID Non Promo Support</param>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ModifyNonPromoSupportBrandTechList(Guid nonPromoSupportId)
        {
            try
            {
                string pspJSON = Request.Content.ReadAsStringAsync().Result;
                List<string> brandTechGuids = JsonConvert.DeserializeObject<List<string>>(pspJSON);
                List<NonPromoSupportBrandTech> oldList = Context.Set<NonPromoSupportBrandTech>().Where(n => n.NonPromoSupportId == nonPromoSupportId && !n.Disabled).ToList();
                List<NonPromoSupportBrandTech> newList = new List<NonPromoSupportBrandTech>();

                foreach (var brandTechGuid in brandTechGuids)
                {
                    var btGuid = Guid.Parse(brandTechGuid);
                    if (btGuid != null)
                    {
                        var nonPromoSupportBrandTech = Context.Set<NonPromoSupportBrandTech>().Where(x => x.BrandTechId == btGuid && x.NonPromoSupportId == nonPromoSupportId).FirstOrDefault();
                        if (nonPromoSupportBrandTech != null)
                        {
                            if (nonPromoSupportBrandTech.Disabled)
                            {
                                nonPromoSupportBrandTech.Disabled = false;
                                nonPromoSupportBrandTech.DeletedDate = null;
                                nonPromoSupportBrandTech.NonPromoSupportId = nonPromoSupportId;
                                nonPromoSupportBrandTech.BrandTechId = btGuid;
                                nonPromoSupportBrandTech.NonPromoSupport = Context.Set<NonPromoSupport>().Find(nonPromoSupportId);
                                nonPromoSupportBrandTech.BrandTech = Context.Set<BrandTech>().Find(btGuid);
                            }
                            else
                            {
                                oldList.Remove(nonPromoSupportBrandTech);
                            }
                        }
                        else
                        {
                            var newNonPromoSupportBrandTech = new NonPromoSupportBrandTech
                            {
                                Id = Guid.NewGuid(),
                                Disabled = false,
                                DeletedDate = null,
                                NonPromoSupportId = nonPromoSupportId,
                                BrandTechId = btGuid,
                                NonPromoSupport = Context.Set<NonPromoSupport>().Find(nonPromoSupportId),
                                BrandTech = Context.Set<BrandTech>().Find(btGuid)
                            };
                            Context.Set<NonPromoSupportBrandTech>().Add(newNonPromoSupportBrandTech);
                        }
                    }
                }

                foreach (var item in oldList)
                {
                    item.Disabled = true;
                    item.DeletedDate = DateTimeOffset.Now;
                }

                Context.SaveChanges();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }
    }
}
