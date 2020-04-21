using Microsoft.Azure.TokenService;
using Microsoft.Azure.TokenService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class TokenServiceWrapper : TokenServiceWrapperBase, IERC20Compatible
    {
        public TokenServiceWrapper(AzureTokenServiceAPI api) : base(api)
        {

        }

        /// <summary>
        /// Create Token by Non Fungible Burnable, Delegateable, Transferable token template
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="tokenSymbol"></param>
        /// <param name="CallerID"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> CreateToken(string tokenName, string tokenSymbol, string CallerID, string CallerGroupName = "Contoso")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            //Generating TokeName
            if (this.tokenServiceAPI.TokenName == null) this.tokenServiceAPI.TokenName = tokenName; //Guid.NewGuid().ToString();

            var request =
                new ConstructorNFMBRGRequest
                {
                    CallerGroupName = CallerGroupName,
                    CallerAccountName = CallerID,
                    FunctionParams = new ConstructorNFMBRGRequestFunctionParams
                    {
                        Name = tokenName,
                        Symbol = tokenSymbol
                    },
                    AdditionalMetaData = null,
                    RequestId = Guid.NewGuid().ToString()
                };

            return await this.tokenServiceAPI.ConstructorNFMBRGAsync(request);
        }

        #region "ERC 20 Compatibles"

        /// <summary>
        /// Check User's Token Balance
        /// </summary>
        /// <param name="TokenID"></param>
        /// <param name="CallerID"></param>
        /// <returns></returns>
        public async Task<long?> GetBalance(string TokenID, string CallerID, string CallerGroupName = "Contoso")
        {
            var request = new BalanceOfNFMBRGRequest()
            {
                CallerGroupName = CallerGroupName,
                CallerAccountName = CallerID,
                FunctionParams = new BalanceOfNFMBRGRequestFunctionParams()
                {
                    Owner = new AccountPartyType("Account", $"{CallerGroupName}.{CallerID}")
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = TokenID;

            return (await tokenServiceAPI.BalanceOfNFMBRGAsync(request)).Output;
        }

        /// <summary>
        /// Returns Token's Name
        /// </summary>
        /// <param name="TokenID"></param>
        /// <param name="CallerID"></param>
        /// <returns></returns>
        public async Task<string> GetName(string TokenID, string CallerID, string CallerGroupName = "Contoso")
        {
            var request = new NameNFMBRGRequest()
            {
                CallerAccountName = CallerID,
                CallerGroupName = CallerGroupName,
                FunctionParams = null,
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = TokenID;
            var response = await tokenServiceAPI.NameNFMBRGAsync(request);

            return response.Output;
        }

        /// <summary>
        /// Returns Token's Symbol
        /// </summary>
        /// <param name="TokenID"></param>
        /// <param name="CallerID"></param>
        /// <returns></returns>
        public async Task<string> GetSymbol(string TokenID, string CallerID, string CallerGroupName = "Contoso")
        {
            var request = new SymbolNFMBRGRequest()
            {
                CallerGroupName = CallerGroupName,
                CallerAccountName = CallerID,
                FunctionParams = new Dictionary<string, string>(),
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = TokenID;
            var response = await tokenServiceAPI.SymbolNFMBRGAsync(request);

            return response.Output;
        }

        public Task<long?> GetTotalSupply(string TokenID, string CallerID, string CallerGroupName = "Contoso")
        {
            //Doesn't make sense to Nonfungible token
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Mint token with Datadata and TokenNumber
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="tokenCreator"></param>
        /// <param name="mintee"></param>
        /// <param name="metaDataString"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> MintToken(string tokenID, string tokenCreator, string mintee, string metaDataString, long? sequence, string tokenCreatorGroupName = "Contoso")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            //AccountResource ar = new AccountResource();
            //ar.GroupName = tokenCreatorGroupName;
            //var response = await ar.GetAsync(mintee);

            var request = new MintWithTokenURINFMBRGRequest
            {
                CallerGroupName = tokenCreatorGroupName,
                CallerAccountName = tokenCreator,
                FunctionParams = new MintWithTokenURINFMBRGRequestFunctionParams()
                {
                    To = new AccountPartyType("Account", $"{tokenCreatorGroupName}.{mintee}"),
                    TokenId = sequence,
                    TokenURI = metaDataString,
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.MintWithTokenURINFMBRGAsync(request);
        }

        /// <summary>
        /// Check Specific user has minter role
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="callerID"></param>
        /// <param name="ownerID"></param>
        /// <returns></returns>
        public async Task<IsMinterNFMBRGResponse> IsMinter(string tokenID, string callerID, string ownerID, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new IsMinterNFMBRGRequest
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = callerID,
                FunctionParams = new IsMinterNFMBRGRequestFunctionParams
                {
                    Account = new AccountPartyType
                    {
                        Descriptor = "Account",
                        Value = $"{callerGroupName}.{ownerID}"
                    }
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.IsMinterNFMBRGAsync(request);

        }

        /// <summary>
        /// Query Meta which was stored in minted token
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="caller"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public async Task<TokenURINFMBRGResponse> GetTokenMetaData(string tokenID, string caller, long? sequence, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new TokenURINFMBRGRequest
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = caller,
                FunctionParams = new TokenURINFMBRGRequestFunctionParams()
                {
                    TokenId = sequence
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.TokenURINFMBRGAsync(request);
        }

        /// <summary>
        /// Adding Minter role by Token owner
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="caller"></param>
        /// <param name="newMinter"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> AddMinter(string tokenID, string caller, string newMinter, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new AddMinterNFMBRGRequest
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = caller,
                FunctionParams = new AddMinterNFMBRGRequestFunctionParams()
                {
                    Account = new AccountPartyType
                    {
                        Descriptor = "Account",
                        Value = $"{callerGroupName}.{newMinter}"
                    }
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.AddMinterNFMBRGAsync(request);
        }

        /// <summary>
        /// Giving Delegation
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="caller"></param>
        /// <param name="allowerID"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> SetApprovalForAll(string tokenID, string caller, string allowerID, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new SetApprovalForAllNFMBRGRequest()
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = caller,
                FunctionParams = new SetApprovalForAllNFMBRGRequestFunctionParams()
                {
                    To = new AccountPartyType()
                    {
                        Descriptor = "Account",
                        Value = $"{callerGroupName}.{allowerID}"
                    },
                    Approved = true
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.SetApprovalForAllNFMBRGAsync(request);
        }

        /// <summary>
        /// Transfer Token from User A to User B
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="mintedTokenNumber"></param>
        /// <param name="callerID"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> TranferToken(string tokenID, long? mintedTokenNumber, string callerID, string from, string to, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");
            //TODO: uncomment this code
            var request = new TransferFromNFMBRGRequest()
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = callerID,
                FunctionParams = new TransferFromNFMBRGRequestFunctionParams()
                {
                    TokenId = mintedTokenNumber,
                    FromProperty = new AccountPartyType("Account", $"{callerGroupName}.{from}"),
                    To = new AccountPartyType("Account", $"{callerGroupName}.{to}")
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.TransferFromNFMBRGAsync(request);
        }

        /// <summary>
        /// Burn minted Token
        /// </summary>
        /// <param name="tokenID"></param>
        /// <param name="callerID"></param>
        /// <param name="mintedTokenNum"></param>
        /// <returns></returns>
        public async Task<AsyncResponse> DeleteToken(string tokenID, string callerID, long? mintedTokenNum, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new BurnNFMBRGRequest()
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = callerID,
                FunctionParams = new BurnNFMBRGRequestFunctionParams()
                {
                    TokenId = mintedTokenNum
                },
                RequestId = Guid.NewGuid().ToString()
            };

            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.BurnNFMBRGAsync(request);
        }

        public async Task<OwnerOfNFMBRGResponse> IsItMyToken(string tokenID, string callerID, long? mintedTokenNum, string callerGroupName = "nike")
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            var request = new OwnerOfNFMBRGRequest()
            {
                CallerGroupName = callerGroupName,
                CallerAccountName = callerID,
                FunctionParams = new OwnerOfNFMBRGRequestFunctionParams()
                {
                    TokenId = mintedTokenNum
                },
                RequestId = Guid.NewGuid().ToString()
            };
            tokenServiceAPI.TokenName = tokenID;
            return await tokenServiceAPI.OwnerOfNFMBRGAsync(request);
        }
    }
}
