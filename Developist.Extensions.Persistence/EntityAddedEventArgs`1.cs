using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;

using System;

namespace Developist.Extensions.Persistence
{
    public class EntityAddedEventArgs<TEntity>
        where TEntity : IEntity
    {
        public EntityAddedEventArgs(TEntity entity, IRepository<TEntity> repository)
        {
            Entity = entity;
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// The entity that was added.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// The repository to which the entity was added.
        /// </summary>
        public IRepository<TEntity> Repository { get; }
    }
}
