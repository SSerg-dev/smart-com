using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {
    public class PromoStatusChangesController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoStatusChangesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoStatusChange> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PromoStatusChange> query = Context.Set<PromoStatusChange>();
            return query;
        }

        protected IQueryable<PromoStatusChange> GetConstraintedByPromoQuery(Guid? promoKey) {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PromoStatusChange> query = Context.Set<PromoStatusChange>().Where(e => e.PromoId == promoKey);
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult PromoStatusChangesByPromo(String promoKey) {
            try {
                IQueryable<PromoStatusChange> pscs = GetConstraintedByPromoQuery(Guid.Parse(promoKey));
                var pscsList = pscs.OrderByDescending(y=>y.Date).ToList();
                foreach (var item in pscsList) {
                    var user = Context.Set<User>().FirstOrDefault(x => x.Id == item.UserId);
                    if (user != null) {
                        item.UserName = user.Name;
                    }
                    var role = Context.Set<Role>().FirstOrDefault(x => x.Id == item.RoleId);
                    if (role != null) {
                        item.RoleName = role.DisplayName;
                    }
                    var status = item.PromoStatus;
                    if (status != null) {
                        item.StatusColor = status.Color;
                        item.StatusName = status.Name;
                    }
                }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = pscsList }));
        } catch (DbUpdateException e) {
                return InternalServerError(new Exception(e.Message));
            }
    }

        private bool EntityExists(System.Guid key) {
            return Context.Set<PromoStatusChange>().Count(e => e.Id == key) > 0;
        }     
    }
}
