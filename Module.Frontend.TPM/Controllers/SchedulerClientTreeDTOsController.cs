using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers {
    /// <summary>
    /// Контроллер для работы с базовыми клиентами для календаря
    /// </summary>
    public class SchedulerClientTreeDTOsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public SchedulerClientTreeDTOsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
            Mapper.CreateMap<ClientTree, SchedulerClientTreeDTO>();
        }

        protected IQueryable<ClientTree> GetConstraintedQuery() {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            DateTime dt = DateTime.Now;
            IQueryable<ClientTree> query = Context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientTree> GetSchedulerClientTreeDTO([FromODataUri] System.Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<SchedulerClientTreeDTO> GetSchedulerClientTreeDTOs() {
            List<SchedulerClientTreeDTO> result = new List<SchedulerClientTreeDTO>();
            foreach (ClientTree client in GetConstraintedQuery()) {
                SchedulerClientTreeDTO clientDef = Mapper.Map<SchedulerClientTreeDTO>(client);
                string stringId = clientDef.Id.ToString();
                clientDef.TypeName = "Regular";
                clientDef.Id = clientDef.Id + 10001; // если Id одинаковый в стор календаря попадает только одна, даже есть idProperty - другое поле (баг?)
                clientDef.InOutId = String.Format("{0}-1", stringId);
                SchedulerClientTreeDTO clientInOut = (SchedulerClientTreeDTO)clientDef.Clone();
                clientInOut.TypeName = "InOut";
                clientInOut.Id = clientInOut.Id + 10002;
                clientInOut.InOutId = String.Format("{0}-2", stringId);
                SchedulerClientTreeDTO clientOtherType = (SchedulerClientTreeDTO)clientInOut.Clone();
                clientOtherType.TypeName = "Other";
                clientOtherType.Id = clientOtherType.Id + 10003;
                clientOtherType.InOutId = String.Format("{0}-3", stringId);
                result.Add(clientDef);
                result.Add(clientInOut);
                result.Add(clientOtherType);
            }
            return result.AsQueryable();
        }
    }
}
