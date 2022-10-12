using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;
using Developist.Core.Persistence.Entities.IncludePaths;
using Developist.Core.Persistence.Pagination;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Developist.Extensions.Persistence
{
    /// <summary>
    /// Implements the wrapper or decorator design pattern by wrapping an <see cref="IRepository{TEntity}"/> instance that the default method implementations delegate to.
    /// Provides a convenience base class to derive from and adapt as needed.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryWrapper<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The event that is raised when an entity is added using the <see cref="Add(TEntity)"/> method.
        /// </summary>
        public event EventHandler<EntityAddedEventArgs<TEntity>>? EntityAdded;

        /// <summary>
        /// The event that is raised when an entity is removed using the <see cref="Remove(TEntity)"/> method.
        /// </summary>
        public event EventHandler<EntityRemovedEventArgs<TEntity>>? EntityRemoved;

        /// <summary>
        /// The event that is raised when one of the FindAsync methods is called.
        /// </summary>
        public event EventHandler<EntitiesRetrievedEventArgs<TEntity>>? EntitiesRetrieved;

        public RepositoryWrapper(IRepository<TEntity> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected IRepository<TEntity> Repository { get; }

        public virtual IUnitOfWork UnitOfWork => Repository.UnitOfWork;

        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
            OnEntityAdded(new EntityAddedEventArgs<TEntity>(entity, this));
        }

        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
            OnEntityRemoved(new EntityRemovedEventArgs<TEntity>(entity, this));
        }

        public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(cancellationToken);
        }

        public virtual Task<int> CountAsync(IQueryableFilter<TEntity> filter, CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(filter, cancellationToken);
        }

        public async virtual Task<IReadOnlyList<TEntity>> FindAsync(IQueryableFilter<TEntity> filter, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.FindAsync(filter, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));
            return entities;
        }

        public async virtual Task<IReadOnlyList<TEntity>> FindAsync(IQueryableFilter<TEntity> filter, IIncludePathsBuilder<TEntity> includePaths, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.FindAsync(filter, includePaths, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));
            return entities;
        }

        public async virtual Task<IReadOnlyList<TEntity>> FindAsync(IQueryableFilter<TEntity> filter, IQueryablePaginator<TEntity> paginator, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.FindAsync(filter, paginator, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));
            return entities;
        }

        public async virtual Task<IReadOnlyList<TEntity>> FindAsync(IQueryableFilter<TEntity> filter, IQueryablePaginator<TEntity> paginator, IIncludePathsBuilder<TEntity> includePaths, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.FindAsync(filter, paginator, includePaths, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));
            return entities;
        }

        protected void OnEntityAdded(EntityAddedEventArgs<TEntity> e)
        {
            EntityAdded?.Invoke(this, e);
        }

        protected void OnEntityRemoved(EntityRemovedEventArgs<TEntity> e)
        {
            EntityRemoved?.Invoke(this, e);
        }

        protected void OnEntitiesRetrieved(EntitiesRetrievedEventArgs<TEntity> e)
        {
            EntitiesRetrieved?.Invoke(this, e);
        }
    }
}
