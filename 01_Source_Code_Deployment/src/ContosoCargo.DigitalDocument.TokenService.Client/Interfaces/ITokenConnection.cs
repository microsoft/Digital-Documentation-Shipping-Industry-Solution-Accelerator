using Microsoft.Azure.TokenService;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public interface ITokenService
    {
        AzureTokenServiceAPI Initialize();
    }
}
