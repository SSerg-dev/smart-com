using Core.Dependency;
using Core.ModuleRegistrator;
using Ninject;
using System.Web.Optimization;

namespace Frontend
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            Frontend.Core.App_Start.BundleRegistrator.RegisterBundles(bundles);

            foreach (var registrator in IoC.Kernel.GetAll<IFrontendModuleRegistrator>())
            {
                registrator.RegisterBundles(bundles);
            }
        }

    }
}