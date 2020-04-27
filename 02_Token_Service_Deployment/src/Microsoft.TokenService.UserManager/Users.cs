using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.Storage;
using Microsoft.TokenService.Storage.Mongo;
using Microsoft.TokenService.UserManager.Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microsoft.TokenService.UserManager
{
    public class Users : RepositoryBase<User, Guid>, IUserManager
    {
        Parties _parties;
        BlockchainNetworks _blockchainNetworks;
        //IConfiguration _configuration;
        KeyVaultClient keyVaultClient;

        public Users(string DataConnectionString, string CollectionName, IConfiguration Configuration) : base(DataConnectionString, CollectionName)
        {
            _parties = new Parties(DataConnectionString, CollectionName);
            _blockchainNetworks = new BlockchainNetworks(DataConnectionString, CollectionName);

            keyVaultClient = new KeyVaultClient(Configuration["KeyVault:KeyVaultUrl"],
                                                Configuration["KeyVault:ClientID"],
                                                Configuration["KeyVault:ClientSecret"]);


        }

        public async Task<User> RegisterUser(string Name, string Description, Guid PartyID, Guid BlockchainNetworkID)
        {
            var newUser = new User()
            {
                Name = Name,
                PublicAddress = "",
                Description = Description,
                Party = _parties.GetParty(PartyID),
                BlockchainNetwork = _blockchainNetworks.GetBlockchainNetwork(BlockchainNetworkID)
            };

            var userIdentifier = newUser.Id.ToString();
            var result = await keyVaultClient.SetKey(userIdentifier);
            var publicKey = await keyVaultClient.GetPublicKey(userIdentifier);

            newUser.PublicAddress = publicKey;

            await this.ObjectCollection.SaveAsync(newUser);
            return newUser;
        }

        public User GetUser(Guid Id)
        {
            return this.ObjectCollection.Find(new GenericSpecification<User>(x => x.Id == Id));
        }

        public void UnRegistUser(Guid id)
        {
            _ = keyVaultClient.DeleteKey(id.ToString()).Result;
            this.ObjectCollection.Delete(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.ObjectCollection.GetAll();
        }
    }
}
