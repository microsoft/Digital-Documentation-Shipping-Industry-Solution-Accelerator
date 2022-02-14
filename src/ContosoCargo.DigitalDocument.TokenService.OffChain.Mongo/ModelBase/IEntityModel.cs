// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase
{
    public interface IEntityModel<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}