using Microsoft.Azure.TokenService.Management.Interfaces;

namespace Microsoft.Azure.TokenService.Management.Model
{
    public class BlockchainNetworkResponsePropertyBag : IResponsePropertyBag
    {
        public string blockchainPlatformType { get; set; }
        public string blockchainPlatformName { get; set; }
        public string blockchainNode { get; set; }
        public string description { get; set; }
    }
}
