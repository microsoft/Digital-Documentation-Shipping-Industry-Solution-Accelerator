using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.UserManager.Model;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TokenService.LedgerClient.Client
{
    public class LedgerClientBase
    {
        protected Web3 web3;
        private KeyVaultClient kvClient;

        private string clientId;
        private string clientSecret;
        private string kvUrl;

        protected LedgerClientBase(IConfiguration config)
        {
            clientId = config["KeyVault:ClientID"];
            clientSecret = config["KeyVault:ClientSecret"];
            kvUrl = config["KeyVault:KeyVaultUrl"];

            kvClient = new KeyVaultClient(kvUrl, clientId, clientSecret);

        }

        protected async Task<Web3> GetWeb3(string KeyIdentifier, string RPCEndpoint)
        {
            var _account = await kvClient.SetUpExternalAccountFromKeyVaultByKey(KeyIdentifier);
            var rpcClient = new RpcClient(baseUrl: new Uri(RPCEndpoint));
            ((ExternalAccount)_account).InitialiseDefaultTransactionManager(rpcClient);
            var web3 = new Web3(account: _account, client: rpcClient);

            web3.TransactionManager.DefaultGasPrice = 0;
            
            return web3;
        }

    }
}
