using Core.History;
using Core.Settings;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Core.KeyVault;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Module.Persist.TPM.KeyVault
{
    public class KeyStorage : IKeyStorage
    {
        public string GetSecret(string secret, string defaultSecret)
        {
            KeyVaultClient keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(GetToken), new HttpClient());

            SecretBundle secretValue = null;

            try
            {
                var url = AppSettingsManager.GetSetting("KeyVaultUrl", "");
                secretValue = keyVaultClient
                    .GetSecretAsync($"https://{url}.vault.azure.net/secrets/{secret}")
                    .Result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return !string.IsNullOrEmpty(secretValue.Value) ? secretValue.Value : defaultSecret;
        }

        private static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var appId = AppSettingsManager.GetSetting("KeyVaultId", "");
            var appPass = AppSettingsManager.GetSetting("KeyVaultPass", "");
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(appId, appPass);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
