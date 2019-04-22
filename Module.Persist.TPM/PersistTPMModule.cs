using Core.ModuleRegistrator;
using Ninject.Modules;

namespace Module.Persist.TPM {
    public class PersistTPMModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IPersistModuleRegistrator>().To<PersistTPMModuleRegistrator>().InSingletonScope();
        }
    }
}
