using Core.History;
using Core.Notification;
using Core.Security;
using Core.Settings;
using Frontend.Core.Security;
using Frontend.Core.Security.AuthorizationManager;
using Frontend.Core.Security.SecurityManager;
using History.ElasticSearch;
using History.RavenDB;
using History.Mongo;
using Ninject.Activation;
using Ninject.Modules;
using Persist;
using Persist.Utils;
using System;
using System.Web;
using Utility.Security;
using Module.Persist.TPM.ElasticSearch;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using Core.KeyVault;
using System.Linq;
using System.Web.Http.Results;
using Frontend.Core.NotificationServices;

namespace Frontend
{
    public class FrontendModule : NinjectModule
    {
        public override void Load()
        {
            if (!Kernel.GetBindings(typeof(IAuthorizationManager)).Any())
            {
                Kernel.Bind<IAuthorizationManager>().ToMethod(SelectAuthorizationManager);
            }
            if (!Kernel.GetBindings(typeof(ISecurityManager)).Any())
            {
                Kernel.Bind<ISecurityManager>().ToMethod(SelectSecurityManager);
            }

            if (!Kernel.GetBindings(typeof(ISettingsManager)).Any())
            {
                Kernel.Bind<ISettingsManager>().To<Persist.Utils.ConfigSettingsManager>();
            }
            if (!Kernel.GetBindings(typeof(IKeyStorage)).Any())
            {
                Kernel.Bind<IKeyStorage>().ToMethod(SelectKeyVault);
            }

            if (!Kernel.GetBindings(typeof(IMailNotificationSettingsService)).Any())
            {
                Kernel.Bind<IMailNotificationSettingsService>().To<DbMailNotificationSettingsService>();
            }
            if (!Kernel.GetBindings(typeof(IMailNotificationService)).Any())
            {
                SelectNotificationService(Kernel);
            }

            var historyModule = AppSettingsManager.GetSetting<string>("HistoryModule", "ElasticSearch");
            switch (historyModule)
            {
                case "ElasticSearch":
                    LoadElastic();
                    break;
                case "MongoDB":
                    LoadMongoDB();
                    break;
                default:
                    LoadElastic();
                    break;
            }

            //Kernel.Bind<IHistoryWriter<Guid>>().To<RavenDbHistoryWriter<Guid>>();
            ////Kernel.Bind<IHistoryWriter<Guid>>().To<ElasticHistoryWriter<Guid>>();
            //Kernel.Bind<IHistoricalEntityFactory<Guid>>().To<HistoricalEntityFactory<Guid>>().InSingletonScope();
            //Kernel.Bind<IHistoryReader>().To<RavenDbHistoryReader>();
            ////Kernel.Bind<IHistoryReader>().To<ElasticHistoryReader>();
        }

        private static void SelectNotificationService(Ninject.IKernel Kernel)
        {
            var NotificationService = AppSettingsManager.GetSetting<String>("NotificationService", "NLog");

            switch (NotificationService)
            {
                case "NLog":
                    Kernel.Bind<IMailNotificationService>().To<NLogMailNotificationService>();
                    break;
                case "LogicApp":
                    Kernel.Bind<IMailNotificationService>().To<LogicAppMailNotificationService>();
                    break;
                default:
                    Kernel.Bind<IMailNotificationService>().To<NLogMailNotificationService>();
                    break;
            }
        }

        private static IAuthorizationManager SelectAuthorizationManager(IContext context)
        {
            if (HttpContext.Current == null)
            {
                return new SystemAuthorizationManager();
            }

            var authSource = AppSettingsManager.GetSetting<String>(Consts.AUTH_SOURCE, Consts.AD_AUTH_SOURCE);

            switch (authSource)
            {
                case Consts.DB_AUTH_SOURCE:
                    return new DbAuthorizationManager();
                case Consts.AD_AUTH_SOURCE:
                    return new AdAuthorizationManager();
                case Consts.MIXED_AUTH_SOURCE:
                    return new AdAndDbAuthorizationManager();
                default:
                    return new DbAuthorizationManager();
            }
        }

        private static ISecurityManager SelectSecurityManager(IContext context)
        {
            var authSource = AppSettingsManager.GetSetting<String>(Consts.AUTH_SOURCE, Consts.AD_AUTH_SOURCE);

            switch (authSource)
            {
                case Consts.DB_AUTH_SOURCE:
                    return new DbSecurityManager();
                case Consts.AD_AUTH_SOURCE:
                    return new AdSecurityManager();
                case Consts.MIXED_AUTH_SOURCE:
                    return new AdAndDbSecurityManager();
                default:
                    return new DbSecurityManager();
            }
        }

        private void LoadElastic()
        {
            var indexName = AppSettingsManager.GetSetting<string>("ElasticHistoryIndexName", "tpm_elastic_index_01");
            var typeName = AppSettingsManager.GetSetting<string>("ElasticHistoryTypeName", "entry");
            string uri = AppSettingsManager.GetSetting<string>("ElasticHost", "http://elastic:changeme@localhost:9200");

            Kernel.Bind<IHistoryWriter<Guid>>().To<ElasticHistoryWriter<Guid>>()
                  .WithConstructorArgument("uri", uri)
                  .WithConstructorArgument("indexName", indexName);
            Kernel.Bind<IHistoricalEntityFactory<Guid>>().To<HistoricalEntityFactory<Guid>>().InSingletonScope();
            Kernel.Bind<IHistoryReader>().To<ElasticHistoryReader>()
                  .WithConstructorArgument("uri", uri)
                  .WithConstructorArgument("indexName", indexName);
        }

        private void LoadMongoDB()
        {
            var dbName = KeyStorageManager.GetKeyVault().GetSecret("MongoDBName", "");
            var uri = KeyStorageManager.GetKeyVault().GetSecret("MongoUrl", "");

            // 2 years
            double ttlSec = AppSettingsManager.GetSetting<double>("MongoTTLSec", 63113904);
            string colName = AppSettingsManager.GetSetting<string>("MongoColName", "historicals");

            Kernel.Bind<IHistoryWriter<Guid>>().To<MongoDBHistoryWriter<Guid>>()
                  .WithConstructorArgument("uri", uri)
                  .WithConstructorArgument("ttlSec", ttlSec)
                  .WithConstructorArgument("colName", colName)
                  .WithConstructorArgument("dbName", dbName);
            Kernel.Bind<IHistoricalEntityFactory<Guid>>().To<HistoricalEntityFactory<Guid>>().InSingletonScope();
            Kernel.Bind<IHistoryReader>().To<MongoDBHistoryReader>()
                  .WithConstructorArgument("uri", uri)
                  .WithConstructorArgument("ttlSec", ttlSec)
                  .WithConstructorArgument("colName", colName)
                  .WithConstructorArgument("dbName", dbName);
        }

        private static IKeyStorage SelectKeyVault(IContext context)
        {
            var keyVault = AppSettingsManager.GetSetting<bool>("UseKeyVault", false);

            if (keyVault)
                return new KeyStorage();
            else
                return new LocalKeyStorage();
        }
    }
}