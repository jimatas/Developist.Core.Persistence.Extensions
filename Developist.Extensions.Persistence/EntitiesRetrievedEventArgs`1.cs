using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;

using System;
using System.Collections.Generic;

namespace Developist.Extensions.Persistence
{
    public class EntitiesRetrievedEventArgs<TEntity>
        where TEntity : IEntity
    {
        public EntitiesRetrievedEventArgs(IReadOnlyList<TEntity> entities, IReadOnlyRepository<TEntity> repository)
        {
            Entities = entities ?? Array.Empty<TEntity>();
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// The list of entities that were retrieved, if any.
        /// </summary>
        public IReadOnlyList<TEntity> Entities { get; }

        /// <summary>
        /// The repository from which the entities were retrieved.
        /// </summary>
        public IReadOnlyRepository<TEntity> Repository { get; }
    }
}
