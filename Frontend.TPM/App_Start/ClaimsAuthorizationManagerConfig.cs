using Frontend.App_Start;
using System;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Security.Claims;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ClaimsAuthorizationManagerConfig), "FederationConfigurationCreated")]

namespace Frontend.App_Start
{
    public static class ClaimsAuthorizationManagerConfig
    {
        public static void FederationConfigurationCreated()
        {
            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;
        }

        private static void FederatedAuthentication_FederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
        {
            IdentityConfigurationElement element = SystemIdentityModelSection.Current
                .IdentityConfigurationElements
                .GetElement(String.Empty);

            Type type = element != null && element.ClaimsAuthorizationManager.IsConfigured
                ? element.ClaimsAuthorizationManager.Type
                : null;

            if (type != null)
            {
                e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager =
                    (ClaimsAuthorizationManager)DependencyResolver.Current.GetService(type);
            }
        }
    }
}