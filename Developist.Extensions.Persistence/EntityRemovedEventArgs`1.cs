using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;

using System;

namespace Developist.Extensions.Persistence
{
    public class EntityRemovedEventArgs<TEntity>
        where TEntity : IEntity
    {
        public EntityRemovedEventArgs(TEntity entity, IRepository<TEntity> repository)
        {
            Entity = entity;
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// The entitiy that was removed.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// The repository from which the entity was removed.
        /// </summary>
        public IRepository<TEntity> Repository { get; }
    }
}
