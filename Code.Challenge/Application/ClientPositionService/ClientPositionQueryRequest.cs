using MediatR;

namespace Code.Challenge.Application.ClientPositionService
{
    /// <summary>
    /// The <see cref="ClientPositionQueryRequest"/> Query Request
    /// </summary>
    public class ClientPositionQueryRequest : IRequest<ClientPositionQueryResponse>
    {
        /// <summary>
        /// The request Person Id
        /// </summary>
        public long PersonId { get; set; }
    }
}