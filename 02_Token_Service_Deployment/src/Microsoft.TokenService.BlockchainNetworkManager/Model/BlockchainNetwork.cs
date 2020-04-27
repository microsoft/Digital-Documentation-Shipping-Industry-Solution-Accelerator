using Microsoft.TokenService.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.TokenService.BlockchainNetworkManager.Model
{
    public class BlockchainNetwork : IEntityModel<Guid>
    {
        public Guid Id { get ; set ; }
        public BlockchainNetwork()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }
        public string Name { get; set; }
        public string BlockchainPlatformName { get; set; }
        public string  BlockchainPlatformType { get; set; }
        public string BlockchainNode { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
