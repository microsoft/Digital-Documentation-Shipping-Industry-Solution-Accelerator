// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class TokenMintedInfo
    {
        public string TokenID { get; set; }
        public long? TokenSequence { get; set; }
        public string TokenMintee { get; set; }
        public string TokenMinter { get; set; }
        public DateTime TokenMintedDate { get; set; }
        public string BusinessMetaData { get; set; }
        public string MintedTokenDescription { get; set; }
        public string MintedTokenTitle { get; set; }
    }
}
