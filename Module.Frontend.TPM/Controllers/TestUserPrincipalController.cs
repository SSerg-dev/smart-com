using Core.Settings;
using System;
using NLog;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Security;
using System.Web;

namespace Module.Frontend.TPM.Controllers
{
    [AllowAnonymous]
    public class TestUserPrincipalController : ApiController
    {
        [AllowAnonymous]
        [Route("api/TestUserPrincipal/CheckUserPrincipal")]
        [HttpGet]
        public IHttpActionResult CheckUserPrincipal()
        {
            try
            {
                var pc = new PrincipalContext(
                    ContextType.Domain,
                    AppSettingsManager.GetSetting<string>("DOMAIN_NAME", ""),
                    AppSettingsManager.GetSetting<string>("AD_CONNECTION_USERNAME", ""),
                    AppSettingsManager.GetSetting<string>("AD_CONNECTION_PASSWORD", "")
                );

                var _up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, "test");

                var d_pc = new PrincipalContext(ContextType.Domain);

                //var currUP = UserPrincipal.Current;
                //var currPC = currUP.Context;
                var up = UserPrincipal.FindByIdentity(d_pc, IdentityType.SamAccountName, "test");

                List<string> upData = new List<string>(){
                    d_pc.Name,
                    d_pc.UserName,
                    d_pc.ConnectedServer,
                    d_pc.Options.ToString(),
                    d_pc.ContextType.ToString(),
                    d_pc.Container,

                    pc.Name,
                    pc.UserName,
                    pc.ConnectedServer,
                    pc.Options.ToString(),
                    pc.ContextType.ToString(),

                    up != null ? up.Name : "currUP is empty",
                    _up != null ? _up.Name : "_UP is empty",
                    _up != null ? _up.DistinguishedName : "_UP is empty",
                };

                return Json(new
                {
                    success = true,
                    message = "",
                    data = upData
                });
            }
            catch (Exception e)
            {
                return Json(new { success = false, result = e });
            }
        }
    }
}