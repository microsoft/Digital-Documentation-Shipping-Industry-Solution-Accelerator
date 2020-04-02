using Microsoft.Azure.TokenService.Management.Interfaces;

namespace Microsoft.Azure.TokenService.Management.Model
{
    public class AccountResponsePropertyBag : IResponsePropertyBag
    {
        public string publicAddress { get; set; }
        public string blockchainNetworkName { get; set; }
        public string description { get; set; }
        public string provisioningState { get; set; }
    }
}
