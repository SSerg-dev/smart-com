using Core.History;
using Core.Notification;
using Core.Settings;
using Module.Persist.TPM.ElasticSearch;
using Ninject.Modules;
using Persist;
using Persist.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingService
{
    public class ProcessingHostModule : NinjectModule
    {
        public override void Load()
        {
            //var indexName = AppSettingsManager.GetSetting<string>("ElasticHistoryIndexName", "tpm_elastic_index_01");
            //var typeName = AppSettingsManager.GetSetting<string>("ElasticHistoryTypeName", "entry");
            //string uri = AppSettingsManager.GetSetting<string>("ElasticHost", "http://elastic:changeme@localhost:9200");

            //Kernel.Bind<IHistoryWriter<Guid>>().To<LocalElasticHistoryWriter<Guid>>()
            //      .WithConstructorArgument("uri", uri)
            //      .WithConstructorArgument("indexName", indexName);
            //Kernel.Bind<IHistoricalEntityFactory<Guid>>().To<HistoricalEntityFactory<Guid>>().InSingletonScope();
            //Kernel.Bind<IHistoryReader>().To<LocalElasticHistoryReader>()
            //      .WithConstructorArgument("uri", uri)
            //      .WithConstructorArgument("indexName", indexName);
        }
    }
}
