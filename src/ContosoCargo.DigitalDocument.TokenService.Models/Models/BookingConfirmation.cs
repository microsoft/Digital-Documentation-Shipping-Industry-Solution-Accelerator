// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class BookingConfirmation 
    {
        public string CYCutOff { get; set; }
        public string SICutOff { get; set; }
        public string EmptyContainerPickupLocation { get; set; }
        public string LadenContainerReturnLocation { get; set; }
        public BookingRequest BookingRequest { get; set; }
    }
}
