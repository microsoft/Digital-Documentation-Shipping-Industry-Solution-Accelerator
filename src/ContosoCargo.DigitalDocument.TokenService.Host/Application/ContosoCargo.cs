// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using ContosoCargo.DigitalDocument.TokenService.Models;
using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase;
using ContosoCargo.DigitalDocument.TokenService.Host.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.Extensions.Configuration;
using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo;
using MongoDB.Driver;

namespace ContosoCargo.DigitalDocument.TokenService.Host.Application
{
    public class ContosoCargo : IContosoCargoApplication
    {
        private readonly IRepository<CargoTokenShipment, Guid> ContosoCargoIncidentRepository;
        private Client.TokenServiceWrapper TokenServiceClient;

        public ContosoCargo(CosmosConnectionStrings Connections, IConfiguration Config, HttpClient httpClient)
        {
            ContosoCargoIncidentRepository = new BusinessTransactionRepository<CargoTokenShipment, Guid>(new MongoClient(Connections.PrimaryReadWriteKey), "CargoTokenRepository");
            TokenServiceClient = new Client.TokenServiceWrapper(Config["App:ServiceEndpoint"], httpClient);
        }

        public async Task<CargoTokenShipment> CreateQuoteRequest(QuoteRequestMessage quoteRequest)
        {
            //Create New Token
            var TokenName = $"ContosoCargoToken:{Guid.NewGuid().ToString()}";
            var CallerID = quoteRequest.CallerID;
            var TokenSymbol = $"CS-{Guid.NewGuid().ToString()}-{quoteRequest.CustomerID}";

            var creationResponse = await TokenServiceClient.CreateToken(TokenName, TokenSymbol, CallerID);

            //Mint Token
            var Minter = quoteRequest.CallerID;
            var Mintee = quoteRequest.CustomerID;
            var TokenID = creationResponse.ContractAddress;
            var TokenSequence = 0; //define QuoteRequest TokenID will be 0
            var SerializedBusinessMetaData = JsonConvert.SerializeObject(quoteRequest.QuoteInfo);

            var mintedResponse = await TokenServiceClient.MintToken(TokenID, Minter, Mintee, SerializedBusinessMetaData, TokenSequence);

            CargoTokenShipment newToken = new CargoTokenShipment()
            {
                TokenMetaInfo = new TokenMeta()
                {
                    TokenID = creationResponse.ContractAddress,
                    TokenName = TokenName,
                    TokenSymbol = TokenSymbol,
                    TokenTemplateID = "8465231b-4919-4498-b6e3-ae5975b7eab2",
                    TokenCreator = Minter,
                    TokenCreatedDate = DateTime.Now
                },
                CurrentStatus = ShipmentStage.OnQuoteRequest,
                TokenId = creationResponse.ContractAddress,
                Contracter = Minter,
                Contractee = Mintee,
                QuoteRequestTokenInfo = new TokenMintedInfo()
                {
                    TokenSequence = TokenSequence,
                    BusinessMetaData = SerializedBusinessMetaData,
                    TokenMintedDate = System.DateTime.Now,
                    TokenMintee = Mintee,
                    TokenMinter = Minter,
                    TokenID = creationResponse.ContractAddress,
                    MintedTokenTitle = quoteRequest.QuoteTitle,
                    MintedTokenDescription = quoteRequest.QuoteDescription
                }
            };

            //Save TokenInfo into Repository

            await ContosoCargoIncidentRepository.SaveAsync(newToken);
            return newToken;
        }




        public async Task<CargoTokenShipment> CreateQuote(QuoteRequestMessage quoteRequest)
        {
            CargoTokenShipment Shipment = ContosoCargoIncidentRepository.Get(Guid.Parse(quoteRequest.ShipmentID));

            //Mint Token
            var Minter = quoteRequest.CallerID;
            var Mintee = quoteRequest.CustomerID;
            var TokenID = Shipment.TokenId;
            var TokenSequence = 1;//define Quote TokenID will be 1
            var SerializedBusinessMetaData = JsonConvert.SerializeObject(quoteRequest.QuoteInfo);

            var mintedResponse = await TokenServiceClient.MintToken(TokenID, Minter, Mintee, SerializedBusinessMetaData, TokenSequence);

            Shipment.QuotedTokenInfo = new TokenMintedInfo()
            {
                TokenID = Shipment.TokenId,
                TokenSequence = TokenSequence, 
                BusinessMetaData = SerializedBusinessMetaData, 
                TokenMintedDate = System.DateTime.Now,
                TokenMintee = Mintee,
                TokenMinter = Minter,
                MintedTokenTitle = quoteRequest.QuoteTitle,
                MintedTokenDescription = quoteRequest.QuoteDescription
            };

            //Save TokenInfo into Repository
            Shipment.CurrentStatus = ShipmentStage.Quoted;
            await ContosoCargoIncidentRepository.SaveAsync(Shipment);
            return Shipment;
        }

        public IEnumerable<CargoTokenShipment> GetMyShipments(GetTokenShipmentsRequest getTokenShipmentsRequest)
        {
            string callee = getTokenShipmentsRequest.CallerID;

            if (getTokenShipmentsRequest.IsContracter)
            {
                return ContosoCargoIncidentRepository.FindAll(
                                                                new GenericSpecification<CargoTokenShipment>(x =>
                                                                x.QuoteRequestTokenInfo.TokenMinter.Equals(callee) ||
                                                                x.BookedTokenInfo.TokenMinter.Equals(callee) ||
                                                                x.QuotedTokenInfo.TokenMinter.Equals(callee) ||
                                                                x.BookingRequestTokenInfo.TokenMinter.Equals(callee)));
            }
            else
            {
                return ContosoCargoIncidentRepository.FindAll(
                                                        new GenericSpecification<CargoTokenShipment>(x =>
                                                        x.QuoteRequestTokenInfo.TokenMintee.Equals(callee) ||
                                                        x.BookedTokenInfo.TokenMintee.Equals(callee) ||
                                                        x.QuotedTokenInfo.TokenMintee.Equals(callee) ||
                                                        x.BookingRequestTokenInfo.TokenMintee.Equals(callee)));
            }
        }

        public async Task<bool> DeleteMyShipment(Guid id, DeleteTokenRequest deleteTokenRequest)
        {
            var Shipment = ContosoCargoIncidentRepository.Find(new GenericSpecification<CargoTokenShipment>(x =>
            x.Id == id));

            if ((Shipment != null) && (Shipment.Contracter == deleteTokenRequest.CallerID))
            {
                //required asyc process burn tokens as parallel.             
                deleteTokenRequest.TokenSequence = 0;
                await TokenServiceClient.DeleteToken(deleteTokenRequest.TokenID, deleteTokenRequest.CallerID, deleteTokenRequest.TokenSequence);

                deleteTokenRequest.TokenSequence = 1;
                await TokenServiceClient.DeleteToken(deleteTokenRequest.TokenID, deleteTokenRequest.CallerID, deleteTokenRequest.TokenSequence);

                deleteTokenRequest.TokenSequence = 2;
                await TokenServiceClient.DeleteToken(deleteTokenRequest.TokenID, deleteTokenRequest.CallerID, deleteTokenRequest.TokenSequence);

                deleteTokenRequest.TokenSequence = 3;
                await TokenServiceClient.DeleteToken(deleteTokenRequest.TokenID, deleteTokenRequest.CallerID, deleteTokenRequest.TokenSequence);

                ContosoCargoIncidentRepository.Delete(id);

                return true;

            }
            else
            {
                return false;
            }
        }

        public CargoTokenShipment GetShipment(Guid id)
        {
            CargoTokenShipment returnedShipment =
          ContosoCargoIncidentRepository.Find(new GenericSpecification<CargoTokenShipment>(x => x.Id.Equals(id)));

            return returnedShipment;
        }

        public async Task<CargoTokenShipment> BookingRequest(BookingRequestMessage bookingRequest)
        {
            CargoTokenShipment Shipment = ContosoCargoIncidentRepository.Get(Guid.Parse(bookingRequest.ShipmentID));

            //Mint Token
            var Minter = bookingRequest.CallerID;
            var Mintee = bookingRequest.CustomerID;
            var TokenID = Shipment.TokenId;
            var TokenSequence = 2; //BookingRequest sequence is "2"
            var SerializedBusinessMetaData = JsonConvert.SerializeObject(bookingRequest.BookingRequestInfo);//bookingRequest

            var mintedResponse = await TokenServiceClient.MintToken(TokenID, Minter, Mintee, SerializedBusinessMetaData, TokenSequence);

            //Adding Minted Token Info
            Shipment.BookingRequestTokenInfo = new TokenMintedInfo()
            {
                TokenID = Shipment.TokenId,
                TokenSequence = TokenSequence,
                BusinessMetaData = SerializedBusinessMetaData,
                TokenMintedDate = System.DateTime.Now,
                TokenMintee = Mintee,
                TokenMinter = Minter,
                MintedTokenTitle = bookingRequest.BookingRequestTitle,
                MintedTokenDescription = bookingRequest.BookingRequestDescription
            };

            //Save TokenInfo into Repository
            Shipment.CurrentStatus = ShipmentStage.OnBookingRequest;
            await ContosoCargoIncidentRepository.SaveAsync(Shipment);
            return Shipment;

        }

        public async Task<CargoTokenShipment> BookingConfimation(BookingConfirmationRequestMessage bookingConfirmationRequest)
        {
            CargoTokenShipment Shipment = ContosoCargoIncidentRepository.Get(Guid.Parse(bookingConfirmationRequest.ShipmentID));

            //Mint Token
            var Minter = bookingConfirmationRequest.CallerID;
            var Mintee = bookingConfirmationRequest.CustomerID;
            var TokenID = Shipment.TokenId;
            var TokenSequence = 3;  //BookingConfirmation sequence is "3"
            var SerializedBusinessMetaData = JsonConvert.SerializeObject(bookingConfirmationRequest.BookingConfirmationInfo); //bookingRequest

            var mintedResponse = await TokenServiceClient.MintToken(TokenID, Minter, Mintee, SerializedBusinessMetaData, TokenSequence);

            //Adding Minted Token Info
            Shipment.BookedTokenInfo = new TokenMintedInfo()
            {
                TokenID = Shipment.TokenId,
                TokenSequence = TokenSequence, 
                BusinessMetaData = SerializedBusinessMetaData,
                TokenMintedDate = System.DateTime.Now,
                TokenMintee = Mintee,
                TokenMinter = Minter,
                MintedTokenTitle = bookingConfirmationRequest.BookingConfirmationTitle,
                MintedTokenDescription = bookingConfirmationRequest.BookingConfirmationDescription
            };

            //Save TokenInfo into Repository
            Shipment.CurrentStatus = ShipmentStage.Booked;
            await ContosoCargoIncidentRepository.SaveAsync(Shipment);
            return Shipment;
        }

        public async Task<TokenMintedInfo> GetMintedTokenInfoFromChain<T>(Guid id, GetTokenMetaDataRequest getTokenMetaDataRequest)
        {
            var Shipment = ContosoCargoIncidentRepository.Find(new GenericSpecification<CargoTokenShipment>(
                                                                    x => x.Id.Equals(id)));

            if (!Shipment.Contracter.Equals(getTokenMetaDataRequest.CallerID)) return null;



             if (typeof(T).Equals(typeof(Quotation)))
            {
                TokenMintedInfo mintedTokenInfo = Shipment.QuoteRequestTokenInfo;
                getTokenMetaDataRequest.TokenSequence = "0";

                var result = await TokenServiceClient.GetTokenMetaData(getTokenMetaDataRequest.TokenID, getTokenMetaDataRequest.CallerID, 0);
                mintedTokenInfo.BusinessMetaData = result;

                return mintedTokenInfo;


            } 
            else if (typeof(T).Equals(typeof(RateQuotation)))
            {
                TokenMintedInfo mintedTokenInfo = Shipment.QuotedTokenInfo;
                getTokenMetaDataRequest.TokenSequence = "1";

                var result = await TokenServiceClient.GetTokenMetaData(getTokenMetaDataRequest.TokenID, getTokenMetaDataRequest.CallerID, 1);
                mintedTokenInfo.BusinessMetaData = result;

                return mintedTokenInfo;

            }
            else if (typeof(T).Equals(typeof(BookingRequest)))
            {
                TokenMintedInfo mintedTokenInfo = Shipment.BookingRequestTokenInfo;
                getTokenMetaDataRequest.TokenSequence = "2";

                var result = await TokenServiceClient.GetTokenMetaData(getTokenMetaDataRequest.TokenID, getTokenMetaDataRequest.CallerID, 2);
                mintedTokenInfo.BusinessMetaData = result;

                return mintedTokenInfo;


            }
            else if (typeof(T).Equals(typeof(BookingConfirmation)))
            {
                TokenMintedInfo mintedTokenInfo = Shipment.BookingRequestTokenInfo;
                getTokenMetaDataRequest.TokenSequence = "3";

                var result = await TokenServiceClient.GetTokenMetaData(getTokenMetaDataRequest.TokenID, getTokenMetaDataRequest.CallerID, 3); ;
                mintedTokenInfo.BusinessMetaData = result;

                return mintedTokenInfo;
            }


            return null;
        }
    }
}
