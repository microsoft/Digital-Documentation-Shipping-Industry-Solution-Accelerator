using Microsoft.Azure.TokenService.Models;
using System.Threading.Tasks;
namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    interface IContosoTokenService
    {
        Task<AsyncResponse> CreateToken(TokenCreationRequest tokenCreationRequest);
        Task<long?> GetTokenBalance(TokenInfomationRequest tokenInfomationRequest);

        Task<string> GetTokenName(TokenInfomationRequest tokenInfomationRequest);

        Task<string> GetTokenSymbol(TokenInfomationRequest tokenInfomationRequest);

        Task<AsyncResponse> MintToken(MintTokenRequest mintTokenRequest);

        Task<IsMinterNFMBRGResponse> IsMinter(IsMinterRequest isMinterRequest);

        Task<TokenURINFMBRGResponse> GetTokenMetaData(GetTokenMetaDataRequest getTokenMetaDataRequest);

        Task<AsyncResponse> SetApprovalForAll(SetApprovalAllRequest setApprovalAllRequest);

        Task<AsyncResponse> TransferToken(TransferTokenRequest transferTokenRequest);

        Task<AsyncResponse> DeleteToken(DeleteTokenRequest deleteTokenRequest);

        Task<OwnerOfNFMBRGResponse> OwnnerOfToken(OwnerOfTokenRequest ownerOfNFMBRGRequest);

    }
}
