using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase
{
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// Gets or sets the func delegate query to execute against the repository for searching records.
        /// </summary>
        Expression<Func<TEntity, bool>> Predicate { get; }
    }
}
