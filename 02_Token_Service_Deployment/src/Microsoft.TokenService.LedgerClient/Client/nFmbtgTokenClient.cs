using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.LedgerClient.Model;
using Microsoft.TokenService.LedgerClient.nFmbtgToken;
using Microsoft.TokenService.LedgerClient.nFmbtgToken.ContractDefinition;
using Microsoft.TokenService.UserManager;
using Microsoft.TokenService.UserManager.Model;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Web3;
using Nethereum.Web3;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TokenService.LedgerClient.Client
{
    public class nFmbtgTokenClient : LedgerClientBase, InFmbtgTokenClient
    {
        private Users users;
        private MapperConfiguration mapConfig;
        public nFmbtgTokenClient(IConfiguration config) : base(config)
        {
            var connString = config["App:Offchain_Connectionstring"];
            var collectionName = config["App:ManagementCollection"];

            users = new Users(connString, collectionName, config);

            mapConfig = 
                new MapperConfiguration(cfg => 
                                                cfg.CreateMap<TransactionReceipt, 
                                                              Model.TransactionReciept>()
                                                              .ForMember(dest => dest.TransactionHash, opt => opt.MapFrom(src => src.TransactionHash))
                                                              .ForMember(dest => dest.TransactionIndex, opt => opt.MapFrom(src => src.TransactionIndex.ToLong()))
                                                              .ForMember(dest => dest.BlockHash, opt => opt.MapFrom(src => src.BlockHash))
                                                              .ForMember(dest => dest.BlockNumber, opt => opt.MapFrom(src => src.BlockNumber.ToLong()))
                                                              .ForMember(dest => dest.CumulativeGasUsed, opt => opt.MapFrom(src => src.CumulativeGasUsed.ToLong()))
                                                              .ForMember(dest => dest.GasUsed, opt => opt.MapFrom(src => src.GasUsed.ToLong()))
                                                              .ForMember(dest => dest.ContractAddress, opt => opt.MapFrom(src => src.ContractAddress))
                                                              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToLong()))
                                                              );
        }

        private async Task<Web3> getWeb3WithUserID(string UserID)
        {
            var user = users.GetUser(Guid.Parse(UserID));
            return await GetWeb3(user.Id.ToString(),
                                        user.BlockchainNetwork.BlockchainNode);
        }

        private async Task<NFmbtgTokenService> getTokenService(string ContractAddress, string UserID)
        {
            return new NFmbtgTokenService(await getWeb3WithUserID(UserID),
                                                ContractAddress);
        }

        public async Task<TransactionReciept> DeployNewToken(string TokenOwnerId, string TokenName, string TokenSymbol)
        {
            var receipt = await NFmbtgTokenService.DeployContractAndWaitForReceiptAsync(
                                                await getWeb3WithUserID(TokenOwnerId),
                                                new NFmbtgTokenDeployment()
                                                {
                                                    Name = TokenName,
                                                    Symbol = TokenSymbol
                                                });
            
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
           
        }

        public async Task<string> Name(string ContractAddress, string CallerId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.NameQueryAsync();
        }

        public async Task<string> Symbol(string ContractAddress, string CallerId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.SymbolQueryAsync();
        }

        public async Task<string> OwnerOf(string ContractAddress, string CallerId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.OwnerOfQueryAsync(TokenId);
        }

        public async Task<long> BalanceOf(string ContractAddress, string CallerId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var retBalance = await tokenSvc.BalanceOfQueryAsync(
                                                    users.GetUser(Guid.Parse(CallerId)).PublicAddress);
            return (long)retBalance;
        }

        public async Task<string> TokenURI(string ContractAddress, string CallerId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.TokenURIQueryAsync(TokenId);
        }

        public async Task<bool> IsApprovedForAll(string ContractAddress, string CallerId, string TokenOwner, string OperatorId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);

            return await tokenSvc.IsApprovedForAllQueryAsync(users.GetUser(Guid.Parse(TokenOwner)).PublicAddress,
                                                             users.GetUser(Guid.Parse(OperatorId)).PublicAddress);

        }

        public async Task<string> GetApproved(string ContractAddress, string CallerId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.GetApprovedQueryAsync(
                                                    TokenId);
        }

        public async Task<TransactionReciept> Approve(string ContractAddress, string CallerId, string ApproverId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.ApproveRequestAndWaitForReceiptAsync(
                                                                        users.GetUser(Guid.Parse(ApproverId)).PublicAddress,
                                                                        TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string CallerId, string ApproveeId, bool Approved)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.SetApprovalForAllRequestAndWaitForReceiptAsync(
                                                                        users.GetUser(Guid.Parse(ApproveeId)).PublicAddress,
                                                                        Approved);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }

        public async Task<TransactionReciept> TransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.TransferFromRequestAndWaitForReceiptAsync(
                                                                        users.GetUser(Guid.Parse(SenderId)).PublicAddress,
                                                                        users.GetUser(Guid.Parse(RecipientId)).PublicAddress,
                                                                        TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }

        public async Task<TransactionReciept> Transfer(string ContractAddress, string SenderId, string RecipientId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, SenderId);
            var receipt = await tokenSvc.TransferRequestAndWaitForReceiptAsync(
                                                                        users.GetUser(Guid.Parse(RecipientId)).PublicAddress,
                                                                        TokenId);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> SafeTransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.SafeTransferFromRequestAndWaitForReceiptAsync(
                                                                        users.GetUser(Guid.Parse(SenderId)).PublicAddress,
                                                                        users.GetUser(Guid.Parse(RecipientId)).PublicAddress,
                                                                        TokenId);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);

        }

        public async Task<TransactionReciept> Burn(string ContractAddress, string CallerId, long TokenId)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.BurnRequestAndWaitForReceiptAsync(TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> MintToken(string ContractAddress, string TokenMinterId, string TokenMinteeId, long TokenId, string TokenURI)
        {
            NFmbtgTokenService tokenSvc = await getTokenService(ContractAddress, TokenMinterId);
            var receipt = await tokenSvc.MintWithTokenURIRequestAndWaitForReceiptAsync(
                                                                    users.GetUser(Guid.Parse(TokenMinteeId)).PublicAddress,
                                                                    TokenId,
                                                                    TokenURI);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


    }
}
