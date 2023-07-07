using MediatR;

namespace Code.Challenge.Application.TopClientsService
{
    /// <summary>
    /// The <see cref="TopClientsQueryRequest"/> Query Request
    /// </summary>
    public class TopClientsQueryRequest : IRequest<IEnumerable<TopClientsQueryResponse>>
    {
        /// <summary>
        /// The request Number Of Results
        /// </summary>
        public int NumberOfResults { get; set; }
    }
}