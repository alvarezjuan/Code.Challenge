using Code.Challenge.Domain;

namespace Code.Challenge.Application.Contracts
{
    /// <summary>
    /// Repository Unit of Work Contract abstraction pattern.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// The <see cref="IRepository<PersonEntity>"/>.
        /// </summary>
        public IRepository<PersonEntity> Persons { get; }

        /// <summary>
        /// Commit unit of work operations to repository storage.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/> reference after the operation has completed.</returns>
        public Task CommitAsync(CancellationToken cancellationToken);
    }
}