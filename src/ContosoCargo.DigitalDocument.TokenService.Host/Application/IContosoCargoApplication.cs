// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoCargo.DigitalDocument.TokenService.Models;
using ContosoCargo.DigitalDocument.TokenService.Host.Messages;


namespace ContosoCargo.DigitalDocument.TokenService.Host.Application
{
    public interface IContosoCargoApplication
    {
        Task<CargoTokenShipment> CreateQuote(QuoteRequestMessage quoteRequest);

        Task<CargoTokenShipment> CreateQuoteRequest(QuoteRequestMessage quoteRequest);

        IEnumerable<CargoTokenShipment> GetMyShipments(GetTokenShipmentsRequest getTokenShipmentsRequest);

        Task<bool> DeleteMyShipment(Guid id, DeleteTokenRequest deleteTokenRequest);

        CargoTokenShipment GetShipment(Guid id);

        Task<CargoTokenShipment> BookingRequest(BookingRequestMessage bookingRequest);

        Task<CargoTokenShipment> BookingConfimation(BookingConfirmationRequestMessage bookingConfirmationRequest);

        Task<TokenMintedInfo> GetMintedTokenInfoFromChain<T>(Guid id, GetTokenMetaDataRequest getMintedTokenInfoRequest);
    }
}
