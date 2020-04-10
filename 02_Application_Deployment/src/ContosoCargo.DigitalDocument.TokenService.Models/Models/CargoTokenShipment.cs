using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase;
using Newtonsoft.Json;
using System;
namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class CargoTokenShipment : IEntityModel<Guid>
    {

        public Guid Id { get; set; }
        public string TokenId { get; set; }
        public TokenMeta TokenMetaInfo { get; set; }

        public string Contracter { get; set; }
        //public string ContracterName { get; set; }
        public string Contractee { get; set; }
        //public string ContracteeName { get; set; }

        public DateTime CreatedTime { get { return QuoteRequestTokenInfo.TokenMintedDate; } }

        public CargoTokenShipment()
        {
            Id = Guid.NewGuid();
        }

        //Current Status of Shipment
        public ShipmentStage CurrentStatus { get; set; }


        public TokenMintedInfo QuoteRequestTokenInfo { get; set; }

        //QuotedDate => ShipmentStage : Quoted
        public TokenMintedInfo QuotedTokenInfo { get; set; }


        //BookingRequestedDate => ShipmentStage : OnBookingRequest
        public TokenMintedInfo BookingRequestTokenInfo { get; set; }

        //BookedDate => ShipmentStage : Booked
        public TokenMintedInfo BookedTokenInfo { get; set; }

        //BookedDate => ShipmentStage : JobOrdered
        public TokenMintedInfo JobOrderTokenInfo { get; set; }
    }

    public enum ShipmentStage
    {
        OnQuoteRequest,
        Quoted,
        OnBookingRequest,
        Booked,
        JobOrdered
    }
}
