using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.FunctionalHelpers.ClientTreeFunction;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.Authorization.WebApi;

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

                if (model != null)
                {
                    return Content(HttpStatusCode.OK, model.Value);
                }
                else
                {
                    ArrayList promotypes = new ArrayList { "Regular Promo", "InOut Promo", "Loyalty Promo", "Dynamic Promo", "Competitor Promo" };
                    ArrayList competitors = new ArrayList { };
                    DateTime dt = DateTime.Now;
                    List<ClientTree> clients = context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)).ToList();
                    ClientTree clientTreeNA = clients.FirstOrDefault(g => g.ObjectId == 5000002);

                    List<ClientResult> clientArray = ClientTreeHelper.GetChildrenBaseClient(clientTreeNA, clients);
                    ArrayList data = new ArrayList { clientArray, promotypes, competitors };
                    string dataStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                    return Content(HttpStatusCode.OK, dataStr);
                    // [[{"id":5000004,"name":"Magnit MM"},{"id":5000005,"name":"Magnit HM"}],["Regular Promo","InOut Promo","Loyalty Promo","Dynamic Promo","Competitor Promo"],[]]
                }
            }
        }
        
    }
}
