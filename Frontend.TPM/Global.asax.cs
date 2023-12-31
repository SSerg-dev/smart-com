﻿using Core.Dependency;
using Core.Notification;
using Core.Settings;
using Frontend.Core;
using Frontend.Core.Security;
using Newtonsoft.Json;
using Ninject;
using NLog;
using Persist;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Utility.EmailGetter;

namespace Frontend {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {

            AreaRegistration.RegisterAllAreas();

            var authSection = (AuthenticationSection) ConfigurationManager.GetSection("system.web/authentication");
            if (authSection.Mode == AuthenticationMode.None) {
                GlobalConfiguration.Configuration.MessageHandlers.Add(IoC.Kernel.Get<BasicAuthenticationMessageHandler>());
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);

            string useXssFilter = WebConfigurationManager.AppSettings["UseXssFilter"];
            if (useXssFilter != "false")
            {
                GlobalConfiguration.Configure(TPM.App_Start.WebApiConfig.Register);
            }

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Newtonsoft Json default settings
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
            // Json.net default settings
            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // Инициализация функций получения списка получателей оповещений
            NotificationEmailGetterLocator.Instance.RegisterGetter(HardcodeInterestedUserEmailGetter.FunctionName, HardcodeInterestedUserEmailGetter.Function);

            try {
                IMailNotificationService notifier = (IMailNotificationService) IoC.Kernel.GetService(typeof(IMailNotificationService));
                using (DatabaseContext context = new DatabaseContext()) {
                    string procedureName = context.Settings.Where(x => x.Name == "FRONTEND_URL").FirstOrDefault().Value;

                    IDictionary<string, string> parameters = new Dictionary<string, string>() {
                        { "NAME", procedureName},
                        { "TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") }
                    };
                    EmailGetterArgument eventArgument = new EmailGetterArgument();
                    eventArgument.Set("Filter", null);

                    bool isAllowNotificationsSending = AppSettingsManager.GetSetting<bool>("AllowNotificationsSending", false);
                    if (isAllowNotificationsSending)
                    {
                        notifier.Notify("APP_FRONTEND_START", parameters, eventArgument);
                    }
                }
            } catch (Exception e) {
                logger.Error(e, "Error during sending notification about starting the system");
            }

            logger.Debug("Frontend is started");
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            if (e.ExceptionObject is Exception) {
                logger.Error(e.ExceptionObject as Exception);
            } else {
                logger.Error(e.ExceptionObject);
            }
        }
    }
}
