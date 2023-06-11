using System;

namespace Developist.Core.Persistence.Extensions
{
    /// <summary>
    /// Provides data for the event raised when an entity is removed from a repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityRemovedEventArgs<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRemovedEventArgs{TEntity}"/> class with the specified entity and repository.
        /// </summary>
        /// <param name="entity">The entity that was removed.</param>
        /// <param name="repository">The repository from which the entity was removed.</param>
        public EntityRemovedEventArgs(TEntity entity, IRepository<TEntity> repository)
        {
            Entity = entity;
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the entity that was removed.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Gets the repository from which the entity was removed.
        /// </summary>
        public IRepository<TEntity> Repository { get; }
    }
}
