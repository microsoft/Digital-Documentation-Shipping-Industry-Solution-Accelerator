using Microsoft.TokenService.BlockchainNetworkManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.BlockchainNetworkManager
{
    public interface IBlockchainNetworkManager
    {
        BlockchainNetwork GetBlockchainNetwork(Guid Id);
        BlockchainNetwork GetBlockchainNetwork(string blockchainName);
        IEnumerable<BlockchainNetwork> GetAllBlockchainNetworks();
        Task<BlockchainNetwork> RegisterBlockchainNetwork(string BlockchainNetworkName, string TransactionNodeURL, string Description);
        void UnRegisterBlockchainNetwork(Guid Id);
    }
}