using Code.Challenge.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Code.Challenge.Application.TopClientsService
{
    /// <summary>
    /// Top Clients Query Service.
    /// </summary>
    internal class TopClientsQueryService : IRequestHandler<TopClientsQueryRequest, IEnumerable<TopClientsQueryResponse>>
    {
        /// <summary>
        /// The <see cref="IUnitOfWork"/>.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Dependency Injection <see cref="TopClientsQueryService"/> constructor.
        /// </summary>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="IUnitOfWork"/> for missing dependencies</exception>
        public TopClientsQueryService(IUnitOfWork unitOfWork)
            => this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        /// <inheritdoc/>
        public async Task<IEnumerable<TopClientsQueryResponse>> Handle(TopClientsQueryRequest request, CancellationToken cancellationToken)
        {
            var persons = this._unitOfWork.Persons
                .FindAll()
                .OrderByDescending(p => p.NumberOfRecommendations)
                .ThenByDescending(p => p.NumberOfConnections)
                .Select(p => new TopClientsQueryResponse() { PersonId = p.PersonId })
                .Take(request.NumberOfResults)
                ;
            return await persons.ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}