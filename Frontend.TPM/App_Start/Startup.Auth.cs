//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Globalization;
//using System.IdentityModel.Claims;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OpenIdConnect;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using Owin;
//using Microsoft.IdentityModel.Protocols.OpenIdConnect;
//using System.IdentityModel.Tokens;
//using AuthenticationContext = Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext;
//using Persist;
//using Module.Frontend.TPM.Util;

//namespace Frontend
//{
//    public partial class Startup
//    {
//        private static string clientId = Helper.GetSecretSetting("ida:ClientIdKey", "");
//        private static string appKey = Helper.GetSecretSetting("ida:ClientSecretKey", "");
//        private static string aadInstance = Helper.GetSecretSetting("ida:AADInstanceKey", "");
//        private static string tenantId = Helper.GetSecretSetting("ida:TenantIdKey", "");
//        private static string postLogoutRedirectUri = Helper.GetSecretSetting("ida:PostLogoutRedirectUriKey", "");

//        private static string authority = aadInstance + tenantId;

//        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
//        string graphResourceId = "https://graph.windows.net/";

//        public void ConfigureAuth(IAppBuilder app)
//        {
//            DatabaseContext db = new DatabaseContext();

//            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

//            app.UseCookieAuthentication(new CookieAuthenticationOptions());

//            app.UseOpenIdConnectAuthentication(
//                new OpenIdConnectAuthenticationOptions
//                {
//                    ClientId = clientId,
//                    Authority = authority,
//                    PostLogoutRedirectUri = postLogoutRedirectUri,

//                    Notifications = new OpenIdConnectAuthenticationNotifications()
//                    {
//                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
//                        AuthorizationCodeReceived = (context) =>
//                        {
//                            var code = context.Code;
//                            ClientCredential credential = new ClientCredential(clientId, appKey);
//                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
//                            AuthenticationContext authContext = new AuthenticationContext(authority, new ADALTokenCache(signedInUserID));
//                            return authContext.AcquireTokenByAuthorizationCodeAsync(
//                               code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceId);
//                        }
//                    }
//                });
//        }
//    }
//}
