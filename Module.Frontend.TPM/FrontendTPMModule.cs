using Core.ModuleRegistrator;
using Ninject.Modules;

namespace Module.Frontend.TPM
{
    public class FrontendTPMModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IFrontendModuleRegistrator>().To<FrontendTPMModuleRegistrator>().InSingletonScope();
        }
    }
}
