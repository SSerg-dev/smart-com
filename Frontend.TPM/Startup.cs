using Microsoft.Owin;
//using Microsoft.Owin.Cors;
using Owin;
using System;

[assembly: OwinStartup(typeof(Frontend.Startup))]
namespace Frontend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AppDomain.CurrentDomain.Load(typeof(Module.Persist.TPM.LogHub).Assembly.FullName);
            AppDomain.CurrentDomain.Load(typeof(Module.Persist.TPM.TasksLogHub).Assembly.FullName);

            //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}