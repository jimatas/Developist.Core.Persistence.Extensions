using System;

namespace Developist.Core.Persistence.Extensions
{
    /// <summary>
    /// Provides data for the event raised when an entity is added to a repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityAddedEventArgs<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAddedEventArgs{TEntity}"/> class with the specified entity and repository.
        /// </summary>
        /// <param name="entity">The entity that was added.</param>
        /// <param name="repository">The repository to which the entity was added.</param>
        public EntityAddedEventArgs(TEntity entity, IRepository<TEntity> repository)
        {
            Entity = entity;
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the entity that was added.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Gets the repository to which the entity was added.
        /// </summary>
        public IRepository<TEntity> Repository { get; }
    }
}
