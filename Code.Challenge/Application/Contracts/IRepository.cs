using System.Linq.Expressions;

namespace Code.Challenge.Application.Contracts
{
    /// <summary>
    /// Repository Contract for Work abstraction pattern.
    /// </summary>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Find all entities for repository.
        /// </summary>
        /// <returns>The <see cref="IQueryable<TEntity>"/> reference after the operation has completed.</returns>
        public IQueryable<TEntity> FindAll();

        /// <summary>
        /// Find filtered entities for repository given the <see cref="Expression<Func<TEntity, bool>>"/>.
        /// </summary>
        /// <param name="expression"The <see cref="Expression<Func<TEntity, bool>>"/>.</param>
        /// <returns>The <see cref="IQueryable<TEntity>"/> reference after the operation has completed.</returns>
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Create the entity in the repository
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        public void Create(TEntity entity);

        /// <summary>
        /// Update the entity in the repository
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        public void Update(TEntity entity);

        /// <summary>
        /// Delete the entity in the repository
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        public void Delete(TEntity entity);
    }
}