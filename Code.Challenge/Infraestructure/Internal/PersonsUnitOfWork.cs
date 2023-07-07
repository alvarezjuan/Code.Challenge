using Code.Challenge.Application.Contracts;
using Code.Challenge.Domain;

namespace Code.Challenge.Infraestructure.Internal
{
    /// <summary>
    /// Repository Entity Unit of Work abstraction pattern.
    /// </summary>
    internal class PersonsUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The <see cref="PersonsContext"/>.
        /// </summary>
        private readonly PersonsContext dbContext;

        /// <summary>
        /// The Dependency Injection <see cref="PersonsUnitOfWork"/> constructor.
        /// </summary>
        /// <param name="dbContext">The <see cref="PersonsContext"/>.></param>
        /// <exception cref="ArgumentNullException"><see cref="PersonsContext"/> for missing dependencies</exception>
        public PersonsUnitOfWork(PersonsContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Persons = new RepositoryBase<PersonEntity, PersonsContext>(this.dbContext);
        }

        /// <inheritdoc/>
        public IRepository<PersonEntity> Persons { get; private set; }

        /// <inheritdoc/>
        public Task CommitAsync(CancellationToken cancellationToken)
            => dbContext.SaveChangesAsync(cancellationToken);
    }
}