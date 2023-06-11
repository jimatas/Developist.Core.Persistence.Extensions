using System;
using System.Threading;
using System.Threading.Tasks;

namespace Developist.Core.Persistence.Extensions
{
    /// <summary>
    /// This class implements the wrapper or decorator design pattern by wrapping an instance of the <see cref="IUnitOfWork"/> interface. 
    /// The default method implementations in this class delegate to the wrapped instance. 
    /// It serves as a convenient base class that can be derived from and adapted as required.
    /// </summary>
    public class UnitOfWorkWrapper : IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkWrapper"/> class with the specified <see cref="IUnitOfWork"/> instance.
        /// </summary>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/> instance to wrap.</param>
        public UnitOfWorkWrapper(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Gets the wrapped <see cref="IUnitOfWork"/> instance.
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc/>
        public virtual bool HasActiveTransaction => UnitOfWork.HasActiveTransaction;

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkCompletedEventArgs>? Completed
        {
            add => UnitOfWork.Completed += value;
            remove => UnitOfWork.Completed -= value;
        }

        /// <inheritdoc/>
        public virtual Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.BeginTransactionAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.CompleteAsync(cancellationToken);
        }

        /// <summary>
        /// Creates and returns a <see cref="RepositoryWrapper{TEntity}"/> for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A <see cref="RepositoryWrapper{TEntity}"/> instance for the specified entity type.</returns>
        public virtual RepositoryWrapper<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            return new RepositoryWrapper<TEntity>(UnitOfWork.Repository<TEntity>());
        }

        /// <inheritdoc/>
        IRepository<TEntity> IUnitOfWork.Repository<TEntity>()
        {
            return Repository<TEntity>();
        }
    }
}
