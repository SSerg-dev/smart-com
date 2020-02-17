using Core.History;
using Core.Notification;
using Core.Security;
using Core.Settings;
using Frontend.Core.Security;
using Frontend.Core.Security.AuthorizationManager;
using Frontend.Core.Security.SecurityManager;
using History.ElasticSearch;
using History.RavenDB;
using Ninject.Activation;
using Ninject.Modules;
using Persist;
using Persist.Utils;
using System;
using System.Web;
using Utility.Security;
using Module.Persist.TPM.ElasticSearch;

namespace Frontend {
    public class FrontendModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IAuthorizationManager>().ToMethod(SelectAuthorizationManager);
            Kernel.Bind<ISecurityManager>().ToMethod(SelectSecurityManager);

            Kernel.Bind<ISettingsManager>().To<SettingsManager>();

            Kernel.Bind<IMailNotificationSettingsService>().To<DbMailNotificationSettingsService>();
            Kernel.Bind<IMailNotificationService>().To<NLogMailNotificationService>();

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

            //Kernel.Bind<IHistoryWriter<Guid>>().To<RavenDbHistoryWriter<Guid>>();
            ////Kernel.Bind<IHistoryWriter<Guid>>().To<ElasticHistoryWriter<Guid>>();
            //Kernel.Bind<IHistoricalEntityFactory<Guid>>().To<HistoricalEntityFactory<Guid>>().InSingletonScope();
            //Kernel.Bind<IHistoryReader>().To<RavenDbHistoryReader>();
            ////Kernel.Bind<IHistoryReader>().To<ElasticHistoryReader>();
        }

        private static IAuthorizationManager SelectAuthorizationManager(IContext context) {
            if (HttpContext.Current == null) {
                return new SystemAuthorizationManager();
            }

            var authSource = AppSettingsManager.GetSetting<String>(Consts.AUTH_SOURCE, Consts.DB_AUTH_SOURCE);

            switch (authSource) {
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

        private static ISecurityManager SelectSecurityManager(IContext context) {
            var authSource = AppSettingsManager.GetSetting<String>(Consts.AUTH_SOURCE, Consts.DB_AUTH_SOURCE);

            switch (authSource) {
                case Consts.DB_AUTH_SOURCE:
                    return new DbSecurityManager();
                case Consts.AD_AUTH_SOURCE:
                    return new TPM.TestAdSecurityManager();
                case Consts.MIXED_AUTH_SOURCE:
                    return new AdAndDbSecurityManager();
                default:
                    return new DbSecurityManager();
            }
        }

    }
}