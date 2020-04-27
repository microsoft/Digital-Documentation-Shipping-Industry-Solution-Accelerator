using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Microsoft.TokenService.Storage.Mongo
{
    public class RepositoryBase<TEntity, Guid> where TEntity : class, IEntityModel<Guid>
    {
        protected IRepository<TEntity, Guid> ObjectCollection;

        public RepositoryBase(string DataConnectionString, string CollectionName, IConfiguration Configuration = null)
        {
            this.ObjectCollection =
                new PersistentObjectStorage<TEntity, Guid>(new MongoClient(DataConnectionString),
                CollectionName);
        }
    }
}
