namespace Microsoft.Azure.TokenService.Management.Model
{
    public class BlockchainNetworkRequestPropertyBag : RequestPropertyBagBase
    {
        [IgnoreToList]
        public string BlockchainNetworkId { get; set; }
        public string blockchainNode { get; set; }
        public string blockchainPlatformType { get => "EthereumFamily"; }
        public string blockchainPlatformName { get => "Quorum"; }
    }


}
