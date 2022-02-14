// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class RateQuotation 
    {
        public RateQuotation()
        {
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get; }
        public string CargoNature { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ContainerSize ContainerSize { get; set; }
        public ContainerType ContainerType { get; set; }

        public DateTime CargoReadyDate { get; set; }
    }

    public enum ContainerType
    {
        TypeA,
        TypeB,
        TypeC
    }

    public enum ContainerSize
    {
        Large,
        Medium,
        Small
    }

}
