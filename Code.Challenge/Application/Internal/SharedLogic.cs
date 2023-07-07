using Code.Challenge.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Code.Challenge.Application.Internal
{
    /// <summary>
    /// Shared Logic implementation for All Query/Command Services.
    /// </summary>
    internal class SharedLogic : ISharedLogic
    {
        /// <summary>
        /// The <see cref="IUnitOfWork"/>.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Dependency Injection <see cref="SharedLogic"/> constructor.
        /// </summary>
        /// <param name="dbContext">The <see cref="IUnitOfWork"/>.></param>
        /// <exception cref="ArgumentNullException"><see cref="IUnitOfWork"/> for missing dependencies</exception>
        public SharedLogic(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        /// <inheritdoc/>
        async Task<int> ISharedLogic.FindClientPosition(long personId, CancellationToken cancellationToken)
        {
            var persons = _unitOfWork.Persons
                .FindAll()
                .OrderByDescending(p => p.NumberOfRecommendations)
                .ThenByDescending(p => p.NumberOfConnections)
                .Select(p => new { p.PersonId })
                ;
            var position = 0;
            await foreach (var person in persons.AsAsyncEnumerable().ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();

                position++;
                if (person.PersonId == personId)
                {
                    return position;
                }
            }
            throw new ApplicationException($"Person ID {personId} not found in Repository");
        }
    }
}