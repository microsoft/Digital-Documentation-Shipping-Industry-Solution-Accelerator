using ContosoCargo.DigitalDocument.TokenService.Models;
using Microsoft.Build.Utilities;
using ContosoCargo.DigitalDocument.TokenService.Host.Messages;
using ContosoCargo.DigitalDocument.TokenService.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
