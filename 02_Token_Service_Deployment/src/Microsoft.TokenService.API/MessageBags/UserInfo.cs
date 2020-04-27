using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.TokenService.API.MessageBags
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PartyID { get; set; }
        public Guid BlockchainNetworkID { get; set; }
    }
}
