using Code.Challenge.Application.Contracts;
using Code.Challenge.Application.Internal;
using Code.Challenge.Domain;
using MediatR;

namespace Code.Challenge.Application.AddClientService
{
    /// <summary>
    /// Add Client Command Service.
    /// </summary>
    internal class AddClientCommandService : IRequestHandler<AddClientCommandRequest, AddClientCommandResponse>
    {
        /// <summary>
        /// The <see cref="ISharedLogic"/>.
        /// </summary>
        private readonly ISharedLogic _sharedLogic;

        /// <summary>
        /// The <see cref="IUnitOfWork"/>.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Dependency Injection <see cref="AddClientCommandService"/> constructor.
        /// </summary>
        /// <param name="sharedLogic">The <see cref="ISharedLogic"/>.</param>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="ISharedLogic"/>, <see cref="IUnitOfWork"/> for missing dependencies</exception>
        public AddClientCommandService(ISharedLogic sharedLogic, IUnitOfWork unitOfWork)
        {
            this._sharedLogic = sharedLogic ?? throw new ArgumentNullException(nameof(sharedLogic));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <inheritdoc/>
        public async Task<AddClientCommandResponse> Handle(AddClientCommandRequest request, CancellationToken cancellationToken)
        {
            _unitOfWork.Persons.Create(new PersonEntity()
            {
                PersonId = request.PersonId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CurrentRole = request.CurrentRole,
                Country = request.Country,
                Industry = request.Industry,
                NumberOfRecommendations = request.NumberOfRecommendations,
                NumberOfConnections = request.NumberOfConnections,
            });
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

            var position = await this._sharedLogic.FindClientPosition(request.PersonId, cancellationToken).ConfigureAwait(false);

            return new AddClientCommandResponse()
            {
                Priority = position,
            };
        }
    }
}