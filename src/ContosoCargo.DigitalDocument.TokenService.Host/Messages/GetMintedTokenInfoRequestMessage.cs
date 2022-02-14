// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Host.Messages
{
    public class GetMintedTokenInfoRequestMessage
    {
        public string TokenID { get; set; }
        public string CallerID { get; set; }
        public string TokenSequence { get; set; }
    }
}
