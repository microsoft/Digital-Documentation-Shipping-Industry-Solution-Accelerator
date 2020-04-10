using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase;
using System;

namespace Microsoft.Azure.TokenService.Management.PersistentModel
{
    public interface IPersistentTokenManager : IEntityModel<Guid>
    {
        public string Name { get; set; }
        public string ResourceID { get; set; }
        public string ResourceType { get; set; }
    }
}
