using Code.Challenge.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Code.Challenge.Infraestructure.Internal
{
    /// <summary>
    /// Repository Base implementation for Work abstraction pattern.
    /// </summary>
    internal class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        /// <summary>
        /// The <see cref="TContext"/>.
        /// </summary>
        protected TContext DbContext { get; set; }

        /// <summary>
        /// The Dependency Injection <see cref="RepositoryBase<TEntity, TContext>"/> constructor.
        /// </summary>
        /// <param name="dbContext">The <see cref="TContext"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="TContext"/> for missing dependencies</exception>
        public RepositoryBase(TContext dbContext)
            => this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <inheritdoc/>
        public IQueryable<TEntity> FindAll()
            => this.DbContext.Set<TEntity>().AsNoTracking();

        /// <inheritdoc/>
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression)
            => this.DbContext.Set<TEntity>().Where(expression).AsNoTracking();

        /// <inheritdoc/>
        public void Create(TEntity entity)
            => this.DbContext.Set<TEntity>().Add(entity);

        /// <inheritdoc/>
        public void Update(TEntity entity)
            => this.DbContext.Set<TEntity>().Update(entity);

        /// <inheritdoc/>
        public void Delete(TEntity entity)
            => this.DbContext.Set<TEntity>().Remove(entity);
    }
}