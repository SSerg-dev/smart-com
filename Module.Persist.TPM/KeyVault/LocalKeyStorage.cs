using Core.History;
using Core.Settings;
using Core.KeyVault;

namespace Module.Persist.TPM.KeyVault
{
    public class LocalKeyStorage : IKeyStorage
    {
        public string GetSecret(string secret, string defaultSecret)
        {
            var secretValue = AppSettingsManager.GetSetting<string>(secret, defaultSecret);
            return secretValue;
        }
    }
}
