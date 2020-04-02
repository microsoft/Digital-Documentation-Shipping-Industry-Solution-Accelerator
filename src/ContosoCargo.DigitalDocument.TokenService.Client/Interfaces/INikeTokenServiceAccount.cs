using ContosoCargo.DigitalDocument.TokenService.Client.Messages;
using System.Threading.Tasks;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    interface IContosoTokenServiceAccount
    {
        Task<Account> RegisterAccount(string ContosoUserIdentifier);
    }
}
