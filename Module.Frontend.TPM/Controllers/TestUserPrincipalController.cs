﻿using Core.Settings;
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
                UserPrincipal currUP = UserPrincipal.Current;
                var currPC = currUP.Context;

                var pc = new PrincipalContext(
                    ContextType.Domain,
                    AppSettingsManager.GetSetting<string>("DOMAIN_NAME", ""),
                    AppSettingsManager.GetSetting<string>("AD_CONNECTION_USERNAME", ""),
                    AppSettingsManager.GetSetting<string>("AD_CONNECTION_PASSWORD", "")
                );

                UserPrincipal up = UserPrincipal.FindByIdentity(currPC, IdentityType.SamAccountName, "test");
                UserPrincipal _up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, "test");

                List<string> upData = new List<string>(){
                    currPC.Name,
                    currPC.UserName,
                    currPC.ConnectedServer,
                    currPC.Options.ToString(),
                    currPC.ContextType.ToString(),

                    pc.Name,
                    pc.UserName,
                    pc.ConnectedServer,
                    pc.Options.ToString(),
                    pc.ContextType.ToString(),

                    up != null ? up.Name : "UP is empty",
                    _up != null ? _up.Name : "_UP is empty",
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
                return Json(new { success = false, result = e.Message });
            }
        }
    }
}