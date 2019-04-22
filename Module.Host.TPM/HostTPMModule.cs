using Core.ModuleRegistrator;
using Ninject.Modules;

namespace Module.Host.TPM {
    public class HostTPMModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IHostModuleRegistrator>().To<HostTPMModuleRegistrator>().InSingletonScope();
        }
    }
}
