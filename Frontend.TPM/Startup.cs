using Core.Settings;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Module.Persist.TPM.Session;
//using Microsoft.Owin.Cors;
using Owin;
using System;

[assembly: OwinStartup(typeof(Frontend.Startup))]
namespace Frontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AppDomain.CurrentDomain.Load(typeof(Module.Persist.TPM.LogHub).Assembly.FullName);
            AppDomain.CurrentDomain.Load(typeof(Module.Persist.TPM.TasksLogHub).Assembly.FullName);
            AppDomain.CurrentDomain.Load(typeof(Module.Persist.TPM.Session.SessionHub).Assembly.FullName);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(AppSettingsManager.GetSetting<int>("SIGNALR_DISCONNECT_TIMEOUT_SECONDS", 30));
            app.Use(typeof(SessionSignalRMiddleware));
            app.MapSignalR();
        }
    }
}