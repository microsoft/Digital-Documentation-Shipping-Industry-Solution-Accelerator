// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using ContosoCargo.DigitalDocument.TokenService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Host.Messages
{
    public class BookingConfirmationRequestMessage
    {
        public string CallerID { get; set; }
        public string ShipmentID { get; set; }
        public string BookingConfirmationTitle { get; set; }
        public string BookingConfirmationDescription { get; set; }

        public string CustomerID { get; set; }
        public BookingConfirmation BookingConfirmationInfo { get; set; }

    }
}
