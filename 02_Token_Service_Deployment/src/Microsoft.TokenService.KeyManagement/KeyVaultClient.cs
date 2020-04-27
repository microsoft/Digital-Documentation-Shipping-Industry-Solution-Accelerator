using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Nethereum.RPC.Accounts;
using Nethereum.Signer.AzureKeyVault;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TokenService.KeyManagement
{
    public class KeyVaultClient
    {
        private string clientId;
        private string clientSecret;
        private string keyvaultUrl;

        private Microsoft.Azure.KeyVault.KeyVaultClient kvClient;

        public KeyVaultClient(string KeyVaultBaseURL, string ClientID, string ClientSecret)
        {
            kvClient = new Azure.KeyVault.KeyVaultClient(GetToken);
            clientId = ClientID;
            clientSecret = ClientSecret;
            keyvaultUrl = KeyVaultBaseURL;
        }

        #region KeyVault Secret

        public async Task<SecretBundle> ReadSecret(string SecretIdentifier)
        {
            kvClient = new Microsoft.Azure.KeyVault.KeyVaultClient(GetToken);
            return await kvClient.GetSecretAsync(keyvaultUrl, SecretIdentifier);
        }

        public async Task<SecretBundle> SetSecret(string SecretIdentifier, string Secret)
        {
            return await kvClient.SetSecretAsync(keyvaultUrl, SecretIdentifier, Secret);
        }
        public async Task<IAccount> SetUpExternalAccountFromKeyVaultBySecret(string SecretIentifier)
        {
            var privateKeySecret = await ReadSecret(SecretIentifier);
            return new Account(privateKeySecret.Value.Trim());
        }

        public async Task<DeletedSecretBundle> DeleteSecret(string SecretIdentifier)
        {
            return await kvClient.DeleteSecretAsync(keyvaultUrl, SecretIdentifier);
        }

        #endregion



        public async Task<KeyBundle> ReadKey(string keyIdentifier)
        {
            return await kvClient.GetKeyAsync(string.Concat(keyvaultUrl, "keys/", keyIdentifier));
        }

        public async Task<DeletedKeyBundle> DeleteKey(string KeyIdentifier)
        {
            return await kvClient.DeleteKeyAsync(keyvaultUrl, KeyIdentifier);
        }

        public async Task<KeyBundle> SetKey(string keyIdentifier)
        {
            return await kvClient.CreateKeyAsync(keyvaultUrl, keyIdentifier,
                  new NewKeyParameters()
                  {
                      Kty = "EC",
                      CurveName = "SECP256K1",
                      KeySize = 2048,
                      Tags = null
                  });
        }

        public async Task<string> GetPublicKey(string keyIdentifier)
        {
            var signer = new AzureKeyVaultExternalSigner(kvClient, string.Concat(keyvaultUrl, "keys/", keyIdentifier));
            return await signer.GetAddressAsync();
        }

        public async Task<IAccount> SetUpExternalAccountFromKeyVaultByKey(string keyIdentifier)
        {
            var signer = new AzureKeyVaultExternalSigner(kvClient, string.Concat(keyvaultUrl, "keys/", keyIdentifier));
            var externalAccount = new ExternalAccount(signer);
            await externalAccount.InitialiseAsync();
            return externalAccount;
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(
                clientId,
                clientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }

    }

}
