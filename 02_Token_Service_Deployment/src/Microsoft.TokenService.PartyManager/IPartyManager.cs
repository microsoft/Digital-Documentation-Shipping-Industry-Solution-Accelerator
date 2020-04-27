using Microsoft.TokenService.PartyManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.PartyManager
{
    public interface IPartyManager
    {
        IEnumerable<Party> GetAllParty();
        Party GetParty(Guid Id);
        Task<Party> RegisterParty(string PartyName, string Description);
        void UnRegisterParty(Guid Id);
    }
}