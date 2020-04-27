using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TokenService.Proxy;

namespace TokenClientSample.nFmbtgToken
{
    public class nFmbtgTokenClient
    {

        private NFmbtgTokenClient tokenServiceAPI;


        public nFmbtgTokenClient(string endpointUrl)
        {
            tokenServiceAPI = new NFmbtgTokenClient(endpointUrl);
        }

        public async Task<TransactionReciept> CreateToken(string tokenName, string tokenSymbol, string CallerID)
        {
            Debug.Assert(tokenServiceAPI != null, "Token Service API should be assigned before invoke it");

            return await tokenServiceAPI.DeployNewTokenAsync(CallerID, tokenName, tokenSymbol);

        }

        public async Task<long?> GetBalance(string ContractAddress, string CallerID)
        {
            return await tokenServiceAPI.BalanceOfAsync(ContractAddress, CallerID);
        }

        public async Task<string> GetName(string ContractAddress, string CallerID)
        {

            return await tokenServiceAPI.NameAsync(ContractAddress, CallerID);
        }

        public async Task<string> GetSymbol(string ContractAddress, string CallerID)
        {
            return await tokenServiceAPI.SymbolAsync(ContractAddress, CallerID);
        }

        public async Task<TransactionReciept> MintToken(string ContractAddress, string minter, string mintee, string metaDataString, long? sequence)
        {
            return await tokenServiceAPI.MintTokenAsync(ContractAddress,
                                                                   minter, mintee, sequence, metaDataString);
        }

        public async Task<string> GetTokenMetaData(string ContractAddress, string caller, long sequence)
        {
            tokenServiceAPI.ReadResponseAsString = true;
            return await tokenServiceAPI.TokenURIAsync(ContractAddress, caller, sequence);
        }


        public async Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string caller, string approveeId)
        {
            return await tokenServiceAPI.SetApprovalForAllAsync(ContractAddress, caller, approveeId, true);
        }

        public async Task<TransactionReciept> TranferToken(string ContractAddress, long sequence, string callerID, string from, string to)
        {
            return await tokenServiceAPI.TransferAsync(ContractAddress, from, to, sequence);
        }

        public async Task<TransactionReciept> BurnToken(string ContractAddress, string callerId, long deletedTokensequence)
        {
            return await tokenServiceAPI.BurnAsync(ContractAddress, callerId, deletedTokensequence);
        }

        public async Task<string> WhoisOwner(string ContractAddress, string callerId, long tokenNumber)
        {
            return await tokenServiceAPI.OwnerOfAsync(ContractAddress, callerId, tokenNumber);
        }
    }
}
