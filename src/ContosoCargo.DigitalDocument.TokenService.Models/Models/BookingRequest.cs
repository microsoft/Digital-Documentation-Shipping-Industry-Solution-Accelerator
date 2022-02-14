// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class BookingRequest 
    {
        public string Shipper { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ContainerSize ContainerSize { get; set; }
        public ContainerType ContainerType { get; set; }
        public int Quantity { get; set; }
        public ShippingPlace Place { get; set; }
        public int Weight { get; set; }
        public CargoNature CargoNature { get; set; }
    }

    public enum ShippingPlace
    {
        Yard,
        Door
    }

    public enum CargoNature
    {
        Normal,
        Dangerous
    }
}
