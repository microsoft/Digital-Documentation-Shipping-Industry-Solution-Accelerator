using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TokenService.LedgerClient.Client;
using Microsoft.TokenService.LedgerClient.Model;
using Nethereum.RPC.Eth.DTOs;

namespace Microsoft.TokenService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class nFmbtgTokenController : ControllerBase
    {
        InFmbtgTokenClient nFmbtgTokenClient;
        public nFmbtgTokenController(InFmbtgTokenClient TokenClient)
        {
            nFmbtgTokenClient = TokenClient;
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<TransactionReciept> Approve(string ContractAddress,string CallerId, string ApproverId, long TokenId) 
            => await nFmbtgTokenClient.Approve(ContractAddress, CallerId, ApproverId, TokenId);

        [HttpPost]
        [Route("BalanceOf")]
        public async Task<long> BalanceOf(string ContractAddress, string CallerId) => await nFmbtgTokenClient.BalanceOf(ContractAddress, CallerId);

        [HttpPost]
        [Route("Burn")]
        public async Task<TransactionReciept> Burn(string ContractAddress, string CallerId, long TokenId) 
            => await nFmbtgTokenClient.Burn(ContractAddress, CallerId, TokenId);

        [HttpPost]
        [Route("DeployNewToken")]
        public async Task<TransactionReciept> DeployNewToken(string TokenOwnerId, string TokenName, string TokenSymbol) 
            => await nFmbtgTokenClient.DeployNewToken(TokenOwnerId, TokenName, TokenSymbol);


        [HttpPost]
        [Route("GetApproved")]
        public async Task<string> GetApproved(string ContractAddress, string CallerId, long TokenId) 
            => await nFmbtgTokenClient.GetApproved(ContractAddress, CallerId, TokenId);

        [HttpPost]
        [Route("IsApprovedForAll")]
        public async Task<bool> IsApprovedForAll(string ContractAddress, string CallerId, string TokenOwner, string OperatorId) 
            => await nFmbtgTokenClient.IsApprovedForAll(ContractAddress, CallerId, TokenOwner, OperatorId);

        [HttpPost]
        [Route("MintToken")]
        public async Task<TransactionReciept> MintToken(string ContractAddress, string TokenMinterId, string TokenMinteeId, long TokenId, string TokenURI) 
            => await nFmbtgTokenClient.MintToken(ContractAddress, TokenMinterId, TokenMinteeId, TokenId, TokenURI);

        [HttpPost]
        [Route("Name")]
        public async Task<string> Name(string ContractAddress, string CallerId) => await nFmbtgTokenClient.Name(ContractAddress, CallerId);


        [HttpPost]
        [Route("OwnerOf")]
        public async Task<string> OwnerOf(string ContractAddress, string CallerId, long TokenId) => await nFmbtgTokenClient.OwnerOf(ContractAddress, CallerId, TokenId);


        [HttpPost]
        [Route("SafeTransferFrom")]
        public async Task<TransactionReciept> SafeTransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
            => await nFmbtgTokenClient.SafeTransferFrom(ContractAddress, CallerId, SenderId, RecipientId, TokenId);

        [HttpPost]
        [Route("SetApprovalForAll")]
        public async Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string CallerId, string ApproveeId, bool Approved)
            => await nFmbtgTokenClient.SetApprovalForAll(ContractAddress, CallerId, ApproveeId, Approved);

        [HttpPost]
        [Route("Symbol")]
        public async Task<string> Symbol(string ContractAddress, string CallerId) => await nFmbtgTokenClient.Symbol(ContractAddress, CallerId);

        [HttpPost]
        [Route("TokenURI")]
        public async Task<string> TokenURI(string ContractAddress, string CallerId, long TokenId) => await nFmbtgTokenClient.TokenURI(ContractAddress, CallerId, TokenId);

        [HttpPost]
        [Route("Transfer")]
        public async Task<TransactionReciept> Transfer(string ContractAddress, string SenderId, string RecipientId, long TokenId) 
            => await nFmbtgTokenClient.Transfer(ContractAddress, SenderId, RecipientId, TokenId);

        [HttpPost]
        [Route("TransferFrom")]
        public async Task<TransactionReciept> TransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
            => await nFmbtgTokenClient.TransferFrom(ContractAddress, CallerId, SenderId, RecipientId, TokenId);

    }
}