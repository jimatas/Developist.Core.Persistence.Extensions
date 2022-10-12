using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Developist.Extensions.Persistence
{
    /// <summary>
    /// Implements the wrapper or decorator design pattern by wrapping an <see cref="IUnitOfWork"/> instance that the default method implementations delegate to.
    /// Provides a convenience base class to derive from and adapt as needed.
    /// </summary>
    public class UnitOfWorkWrapper : IUnitOfWork
    {
        public UnitOfWorkWrapper(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected IUnitOfWork UnitOfWork { get; }

        public virtual bool IsTransactional => UnitOfWork.IsTransactional;

        public event EventHandler<UnitOfWorkCompletedEventArgs>? Completed
        {
            add => UnitOfWork.Completed += value;
            remove => UnitOfWork.Completed -= value;
        }

        public virtual Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.BeginTransactionAsync(cancellationToken);
        }

        public virtual Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.CompleteAsync(cancellationToken);
        }

        public virtual ValueTask DisposeAsync()
        {
            var disposalTask = UnitOfWork.DisposeAsync();
            GC.SuppressFinalize(this);
            return disposalTask;
        }

        public virtual RepositoryWrapper<TEntity> Repository<TEntity>()
            where TEntity : class, IEntity
        {
            return new RepositoryWrapper<TEntity>(UnitOfWork.Repository<TEntity>());
        }

        IRepository<TEntity> IUnitOfWork.Repository<TEntity>()
        {
            return Repository<TEntity>();
        }
    }
}
