using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.PartyManager.Model;
using Microsoft.TokenService.Storage;
using Microsoft.TokenService.Storage.Mongo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.PartyManager
{
    public class Parties : RepositoryBase<Party, Guid>, IPartyManager
    {
        public Parties(string DataConnectionString, string CollectionName) : base(DataConnectionString, CollectionName)
        {
        }

        public async Task<Party> RegisterParty(string PartyName, string Description)
        {
            var party = new Party()
            {
                PartyName = PartyName,
                Description = Description
            };

            await this.ObjectCollection.SaveAsync(party);
            return party;
        }

        public IEnumerable<Party> GetAllParty()
        {
            return this.ObjectCollection.GetAll();
        }

        public Party GetParty(Guid Id)
        {
            return this.ObjectCollection.Find(new GenericSpecification<Party>(x => x.Id == Id));
        }


        public void UnRegisterParty(Guid Id)
        {
            this.ObjectCollection.Delete(Id);
        }
    }
}
