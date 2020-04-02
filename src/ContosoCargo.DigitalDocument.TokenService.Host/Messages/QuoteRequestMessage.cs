using ContosoCargo.DigitalDocument.TokenService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Host.Messages
{
    public class QuoteRequestMessage
    {
        public string ShipmentID { get; set; }
        public string QuoteTitle { get; set; }
        public string QuoteDescription { get; set; }
        public string CallerID { get; set; }
        public string CustomerID { get; set; }
        public RateQuotation QuoteInfo { get; set; }
    }
}
