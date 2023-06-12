using System;
using System.Threading;
using System.Threading.Tasks;

namespace Developist.Core.Persistence.Extensions
{
    /// <summary>
    /// Implements the wrapper or decorator design pattern by wrapping an <see cref="IRepository{TEntity}"/> instance, delegating to the default method implementations. 
    /// Provides a convenient base class for deriving and adapting as needed.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class RepositoryWrapper<TEntity> : IRepository<TEntity>
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
        /// The event that is raised when one of the <c>ListAsync</c>, <c>SingleOrDefaultAsync</c>, <c>FirstOrDefaultAsync</c> methods, or one of their overloads, is called.
        /// </summary>
        public event EventHandler<EntitiesRetrievedEventArgs<TEntity>>? EntitiesRetrieved;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper{TEntity}"/> class with the specified repository to be wrapped.
        /// </summary>
        /// <param name="repository">The repository instance to be wrapped.</param>
        public RepositoryWrapper(IRepository<TEntity> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the wrapped repository instance.
        /// </summary>
        protected IRepository<TEntity> Repository { get; }

        /// <inheritdoc/>
        public virtual IUnitOfWork UnitOfWork => Repository.UnitOfWork;

        /// <inheritdoc/>
        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
            OnEntityAdded(new EntityAddedEventArgs<TEntity>(entity, this));
        }

        /// <inheritdoc/>
        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
            OnEntityRemoved(new EntityRemovedEventArgs<TEntity>(entity, this));
        }

        /// <inheritdoc/>
        public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<int> CountAsync(IFilterCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(criteria, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity> SingleOrDefaultAsync(IFilterCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
        {
            var entity = await Repository.SingleOrDefaultAsync(criteria, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entity is null ? Array.Empty<TEntity>() : new[] { entity }, this));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            var entity = await Repository.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entity is null ? Array.Empty<TEntity>() : new[] { entity }, this));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> FirstOrDefaultAsync(IFilterCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
        {
            var entity = await Repository.FirstOrDefaultAsync(criteria, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entity is null ? Array.Empty<TEntity>() : new[] { entity }, this));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<IPaginatedList<TEntity>> ListAsync(IPaginator<TEntity> paginator, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.ListAsync(paginator, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));

            return entities;
        }

        /// <inheritdoc/>
        public async Task<IPaginatedList<TEntity>> ListAsync(IFilterCriteria<TEntity> criteria, IPaginator<TEntity> paginator, CancellationToken cancellationToken = default)
        {
            var entities = await Repository.ListAsync(criteria, paginator, cancellationToken).ConfigureAwait(false);
            OnEntitiesRetrieved(new EntitiesRetrievedEventArgs<TEntity>(entities, this));

            return entities;
        }

        /// <summary>
        /// Raises the <see cref="EntityAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EntityAddedEventArgs{TEntity}"/> containing the event data.</param>
        protected void OnEntityAdded(EntityAddedEventArgs<TEntity> e)
        {
            EntityAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="EntityRemoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EntityRemovedEventArgs{TEntity}"/> containing the event data.</param>
        protected void OnEntityRemoved(EntityRemovedEventArgs<TEntity> e)
        {
            EntityRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="EntitiesRetrieved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EntitiesRetrievedEventArgs{TEntity}"/> containing the event data.</param>
        protected void OnEntitiesRetrieved(EntitiesRetrievedEventArgs<TEntity> e)
        {
            EntitiesRetrieved?.Invoke(this, e);
        }
    }
}
