using System;
using System.Linq.Expressions;

namespace Microsoft.TokenService.Storage
{
    public class GenericSpecification<TEntity> : ISpecification<TEntity>
    {
        public GenericSpecification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// Gets or sets the func delegate query to execute against the repository for searching records.
        /// </summary>
        public Expression<Func<TEntity, bool>> Predicate { get; }
    }
}