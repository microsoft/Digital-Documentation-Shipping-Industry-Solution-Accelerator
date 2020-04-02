namespace Microsoft.Azure.TokenService.Management.Model
{
    public class AccountRequestPropertyBag : RequestPropertyBagBase
    {
        [IgnoreToList]
        public string AccountName { get; set; }
        private string _blockchainNetworkId;

        public string blockchainNetworkId
        {
            get => $"/subscriptions/1e5f5d29-1b9b-4330-bacb-e6a00e4e8a66/resourceGroups/ABTTest/providers/Microsoft.BlockchainTokens/tokenServices/echopreview/blockchainNetworks/{_blockchainNetworkId}";
            set { _blockchainNetworkId = value; }
        } 
    }
}
