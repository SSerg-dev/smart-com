using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.FunctionalHelpers.ClientTreeFunction;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class SavedSettingsController : ApiController
    {
        private readonly IAuthorizationManager authorizationManager;
        public SavedSettingsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        [ClaimsAuthorize]
        [Route("api/SavedSettings/SaveSettings")]
        [HttpPost]
        public IHttpActionResult SaveSettings([FromBody] SavedSettingDTO dto)
        {
            UserInfo userInfo = authorizationManager.GetCurrentUser();
            RoleInfo roleInfo = userInfo.GetCurrentRole();
            using (DatabaseContext context = new DatabaseContext())
            {
                var model = context.Set<SavedSetting>().FirstOrDefault(gs =>
                gs.UserRole.UserId == userInfo.Id && gs.UserRole.RoleId == roleInfo.Id && gs.Key == dto.Key);

                if (model == null)
                {
                    model = context.Set<SavedSetting>().Create();
                    model.Key = dto.Key;
                    model.UserRoleId = context.UserRoles.FirstOrDefault(ur => ur.UserId == userInfo.Id && ur.RoleId == roleInfo.Id).Id;
                    context.Set<SavedSetting>().Add(model);
                }

                model.Value = dto.Value;
                context.SaveChanges();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        [ClaimsAuthorize]
        [Route("api/SavedSettings/LoadSettings")]
        [HttpPost]
        public IHttpActionResult LoadSettings([FromBody] SavedSettingDTO dto)
        {
            UserInfo userInfo = authorizationManager.GetCurrentUser();
            RoleInfo roleInfo = userInfo.GetCurrentRole();
            using (DatabaseContext context = new DatabaseContext())
            {
                var model = context.Set<SavedSetting>().FirstOrDefault(gs =>
                gs.UserRole.UserId == userInfo.Id && gs.UserRole.RoleId == roleInfo.Id && gs.Key == dto.Key);
                UserInfo user = authorizationManager.GetCurrentUser();
                string role = authorizationManager.GetCurrentRoleName();
                IList<Constraint> constraints = user.Id.HasValue ? context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                    .ToList() : new List<Constraint>();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                List<int> clientsIds = ApplyFilter(hierarchy, filters);
                ArrayList promotypes = new ArrayList { "Regular Promo", "InOut Promo", "Loyalty Promo", "Dynamic Promo", "Competitor Promo" };
                ArrayList competitors = new ArrayList { };
                DateTime dt = DateTime.Now;
                List<ClientTree> clients = context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)).ToList();
                ClientTree clientTreeNA = clients.FirstOrDefault(g => g.ObjectId == 5000002);
                if (model != null)
                {
                    var obj = JsonConvert.DeserializeObject<List<List<object>>>(model.Value);
                    var oooo = obj[0];
                    List<IdName> idNames = new List<IdName>();
                    if (clientsIds.Count > 0)
                    {
                        foreach (var item in oooo)
                        {
                            IdName idName = JsonConvert.DeserializeObject<IdName>(item.ToString());
                            if (!clientsIds.Contains(idName.id))
                            {
                                List<ClientResult> clientArray = ClientTreeHelper.GetChildrenBaseClient(clientTreeNA, clients);
                                if (constraints.Count == 0)
                                {

                                    ArrayList data = new ArrayList { clientArray, promotypes, competitors };
                                    string dataStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                                    return Content(HttpStatusCode.OK, dataStr);
                                    // [[{"id":5000004,"name":"Magnit MM"},{"id":5000005,"name":"Magnit HM"}],["Regular Promo","InOut Promo","Loyalty Promo","Dynamic Promo","Competitor Promo"],[]]
                                }
                                else
                                {
                                    clientArray = context.Set<ClientTree>().Where(g => clientsIds.Contains(g.ObjectId) && g.EndDate == null && g.IsBaseClient).Select(g => new ClientResult { Id = g.ObjectId, Name = g.Name }).ToList();
                                    ArrayList data = new ArrayList { clientArray, promotypes, competitors };
                                    string dataStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                                    return Content(HttpStatusCode.OK, dataStr);
                                }
                            }
                        }
                    }
                    return Content(HttpStatusCode.OK, model.Value);
                }
                else
                {
                    List<ClientResult> clientArray = ClientTreeHelper.GetChildrenBaseClient(clientTreeNA, clients);
                    if (constraints.Count == 0)
                    {

                        ArrayList data = new ArrayList { clientArray, promotypes, competitors };
                        string dataStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                        return Content(HttpStatusCode.OK, dataStr);
                        // [[{"id":5000004,"name":"Magnit MM"},{"id":5000005,"name":"Magnit HM"}],["Regular Promo","InOut Promo","Loyalty Promo","Dynamic Promo","Competitor Promo"],[]]
                    }
                    else
                    {
                        clientArray = context.Set<ClientTree>().Where(g => clientsIds.Contains(g.ObjectId) && g.EndDate == null && g.IsBaseClient).Select(g => new ClientResult { Id = g.ObjectId, Name = g.Name }).ToList();
                        ArrayList data = new ArrayList { clientArray, promotypes, competitors };
                        string dataStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                        return Content(HttpStatusCode.OK, dataStr);
                    }

                }
            }
        }
        public static List<int> ApplyFilter(IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            List<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client).ToList();
            List<int> filteredId = new List<int>();
            if (clientFilter.Any())
            {
                List<ClientTreeHierarchyView> hierarchyList = getFilteredHierarchy(hierarchy, clientFilter).ToList();
                filteredId = hierarchyList.Select(n => n.Id).ToList();

            }
            return filteredId;
        }
        private static IQueryable<ClientTreeHierarchyView> getFilteredHierarchy(IQueryable<ClientTreeHierarchyView> hierarchy, IEnumerable<string> clientFilter)
        {
            return hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
        }
        class IdName
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}
