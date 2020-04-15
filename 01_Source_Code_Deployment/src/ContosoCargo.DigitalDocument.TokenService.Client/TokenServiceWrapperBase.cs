using Microsoft.Azure.TokenService;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class TokenServiceWrapperBase
    {
        protected AzureTokenServiceAPI tokenServiceAPI;

        public TokenServiceWrapperBase(AzureTokenServiceAPI api)
        {
            tokenServiceAPI = api;
        }
    }
}
