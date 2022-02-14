// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using ContosoCargo.DigitalDocument.TokenService.Models;
namespace ContosoCargo.DigitalDocument.TokenService.Host.Messages
{
    public class BookingRequestMessage
    {
        public string CallerID { get; set; }
        public string ShipmentID { get; set; }
        public string BookingRequestTitle { get; set; }
        public string BookingRequestDescription { get; set; }
        public string CustomerID { get; set; }

        public BookingRequest BookingRequestInfo { get; set; } 
    }
}
