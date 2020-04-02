using ContosoCargo.DigitalDocument.TokenService.Client.Messages;
using Microsoft.Azure.TokenService;
using Microsoft.Azure.TokenService.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    /// <summary>
    /// Provisioning new User for ABT
    /// {
    //    "id": "ab8be042-cfae-4e6e-ab08-73b8557f330a",
    //    "name": "CryptoGoods ledger",
    //    "description": "Quorum",
    //    "blockchainPlatformType": "EthereumFamily",
    //    "blockchainPlatformName": "Ethereum Quorum",
    //    "blockchainNode": "https://Contosomember01.blockchain.azure.com:3200/f24gVpuHVS8npNjWbIgUfQ1P"
    //  }
    //    {
    //    "id": "8d0c4ff8-398c-4b78-a1fc-a9afe5ec9401",
    //    "name": "Contoso"
    //  }
    /// </summary>
    public class AccountServiceWrapper : TokenServiceWrapperBase, IContosoTokenServiceAccount
    {
        public string GroupName { get; set; }
        public string BlockchainNetworkName { get; set; }
        public AccountServiceWrapper(AzureTokenServiceAPI api, string GroupNamne = "nike", string BlockchainNetworkID = "cryptokickchain") : base(api)
        {
            this.GroupName = GroupNamne;
            this.BlockchainNetworkName = BlockchainNetworkID;
        }

        /// <summary>
        /// PartyID and BlockchainNetworkId value should move in configuration
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async Task<Account> RegisterAccount(string ContosoUserIdentifier)
        {
            var accountResource = new AccountResource();

            accountResource.GroupName = this.GroupName;
            var abtUserID = Guid.NewGuid().ToString();

            var response =
                await accountResource.RegisterOrUpdateAsync(new Microsoft.Azure.TokenService.Management.Model.AccountRequestPropertyBag()
                {
                    AccountName = abtUserID,
                    blockchainNetworkId = this.BlockchainNetworkName,
                    description = $"Nike user - {ContosoUserIdentifier}"
                });

            var jObjectResponse = (JObject)response.Properties;

            return new Account()
            {
                Id = abtUserID,
                Name = abtUserID,
                //BlockchainNetworkName = jObjectResponse.GetValue("blockchainNetworkName").ToString(),
                PublicAddress = jObjectResponse.GetValue("publicAddress").ToString(),
                Description = jObjectResponse.GetValue("description").ToString()
            };
        }

        public async Task<Account> GetAccountAsync(string ABTUserID)
        {
            var accountResource = new AccountResource();
            accountResource.GroupName = this.GroupName;

            var response = await accountResource.GetAsync(ABTUserID);

            if (response.value.Length == 0) return null;

            return new Account()
            {
                Id = response.value[0].name,
                Name = response.value[0].name,
                BlockchainNetworkName = response.value[0].properties.blockchainNetworkName,
                Description = response.value[0].properties.description,
                PublicAddress = response.value[0].properties.publicAddress
            };
        }

        public async Task<Account[]> GetAllAccountAsync()
        {
            var accountResource = new AccountResource();
            accountResource.GroupName = this.GroupName;

            var response = await accountResource.GetAllAsync();

            if (response.value.Length == 0) return null;

            var accountList = new List<Account>();

            foreach (var account in response.value)
            {
                accountList.Add(new Account()
                {
                    Id = account.name,
                    Name = account.name,
                    BlockchainNetworkName = account.properties.blockchainNetworkName,
                    Description = account.properties.description,
                    PublicAddress = account.properties.publicAddress
                });
            }

            return accountList.ToArray();
        }

        public async Task DeleteUser(string ABTUserID)
        {
            var accountResource = new AccountResource();
            accountResource.GroupName = this.GroupName;
            await accountResource.UnRegisterAsync(ABTUserID);
        }
    }
}
