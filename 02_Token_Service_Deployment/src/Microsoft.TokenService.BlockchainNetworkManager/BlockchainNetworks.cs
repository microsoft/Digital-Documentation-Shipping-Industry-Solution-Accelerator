using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.Storage;
using Microsoft.TokenService.Storage.Mongo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.BlockchainNetworkManager
{
    public class BlockchainNetworks : RepositoryBase<BlockchainNetwork, Guid>, IBlockchainNetworkManager
    {
        public BlockchainNetworks(string DataConnectionString, string CollectionName) : base(DataConnectionString, CollectionName)
        {
        }

        public async Task<BlockchainNetwork> RegisterBlockchainNetwork(string BlockchainNetworkName, string TransactionNodeURL, string Description)
        {
            var blockchainNetwork = new BlockchainNetwork()
            {
                Name = BlockchainNetworkName,
                BlockchainNode = TransactionNodeURL,
                BlockchainPlatformName = "Quorum",
                BlockchainPlatformType = "Ethereum",
                Description = Description
            };

            await this.ObjectCollection.SaveAsync(blockchainNetwork);
            return blockchainNetwork;
        }

        public IEnumerable<BlockchainNetwork> GetAllBlockchainNetworks()
        {
            return this.ObjectCollection.GetAll();
        }

        public BlockchainNetwork GetBlockchainNetwork(Guid Id)
        {
            return this.ObjectCollection.Find(new GenericSpecification<BlockchainNetwork>(x => x.Id == Id));
        }

        public BlockchainNetwork GetBlockchainNetwork(String blockchainName)
        {
            return this.ObjectCollection.Find(new GenericSpecification<BlockchainNetwork>(x => x.Name == blockchainName));
        }

        public void UnRegisterBlockchainNetwork(Guid Id)
        {
            this.ObjectCollection.Delete(Id);
        }
    }
}
