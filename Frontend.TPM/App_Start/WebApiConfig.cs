using Frontend.TPM.Attributes;
using System.Web.Http;

namespace Frontend.TPM.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AntiXssFilter());
        }
    }
}