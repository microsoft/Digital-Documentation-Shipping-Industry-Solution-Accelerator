using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.Storage
{
    public interface IRepository<TEntity, in TIdentifier>
    {
        TEntity Get(TIdentifier id);

        IEnumerable<TEntity> GetAll();

        TEntity Save(TEntity entity);

        Task<TEntity> SaveAsync(TEntity entity);

        void Delete(TIdentifier id);

        void Delete(TEntity entity);

        IEnumerable<TEntity> GetAll(IEnumerable<TIdentifier> identifiers);

        TEntity Find(ISpecification<TEntity> specification);

        IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);
    }
}