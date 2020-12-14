using Core.Dependency;
using Core.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.KeyVault;

namespace Core.Settings
{
    public static class localKeyStorageManager
    {
        public static IKeyStorage GetKeyVault()
        {
            var keyVault = AppSettingsManager.GetSetting<bool>("UseKeyVault", false);

            if (keyVault)
                return new Module.Persist.TPM.KeyVault.KeyStorage();
            else
                return new LocalKeyStorage();
        }
    }
}
