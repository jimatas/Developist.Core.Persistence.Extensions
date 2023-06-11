using System;
using System.Collections.Generic;

namespace Developist.Core.Persistence.Extensions
{
    /// <summary>
    /// Provides data for the event raised when entities are retrieved from a repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    public class EntitiesRetrievedEventArgs<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesRetrievedEventArgs{TEntity}"/> class with the specified list of entities and repository.
        /// </summary>
        /// <param name="entities">The list of entities that were retrieved, if any.</param>
        /// <param name="repository">The repository from which the entities were retrieved.</param>
        public EntitiesRetrievedEventArgs(IReadOnlyList<TEntity> entities, IRepository<TEntity> repository)
        {
            Entities = entities ?? Array.Empty<TEntity>();
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the list of entities that were retrieved, if any.
        /// </summary>
        /// <remarks>
        /// The list may contain at most one entity when the <c>FirstOrDefaultAsync</c>, <c>SingleOrDefaultAsync</c> method, or one of their overloads was used.
        /// </remarks>
        public IReadOnlyList<TEntity> Entities { get; }

        /// <summary>
        /// Gets the repository from which the entities were retrieved.
        /// </summary>
        public IRepository<TEntity> Repository { get; }
    }
}
