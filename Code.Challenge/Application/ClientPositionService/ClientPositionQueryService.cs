using Code.Challenge.Application.Contracts;
using Code.Challenge.Application.Internal;
using MediatR;

namespace Code.Challenge.Application.ClientPositionService
{
    /// <summary>
    /// Client Position Query Service.
    /// </summary>
    internal class ClientPositionQueryService : IRequestHandler<ClientPositionQueryRequest, ClientPositionQueryResponse>
    {
        /// <summary>
        /// The <see cref="ISharedLogic"/>.
        /// </summary>
        private readonly ISharedLogic _sharedLogic;

        /// <summary>
        /// The Dependency Injection <see cref="ClientPositionQueryService"/> constructor.
        /// </summary>
        /// <param name="sharedLogic">The <see cref="ISharedLogic"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="ISharedLogic"/> for missing dependencies</exception>
        public ClientPositionQueryService(ISharedLogic sharedLogic)
        {
            this._sharedLogic = sharedLogic ?? throw new ArgumentNullException(nameof(sharedLogic));
        }

        /// <inheritdoc/>
        public async Task<ClientPositionQueryResponse> Handle(ClientPositionQueryRequest request, CancellationToken cancellationToken)
        {
            var position = await this._sharedLogic.FindClientPosition(request.PersonId, cancellationToken).ConfigureAwait(false);

            return new ClientPositionQueryResponse()
            {
                Position = position,
            };
        }
    }
}