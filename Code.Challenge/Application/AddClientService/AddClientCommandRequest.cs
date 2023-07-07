using MediatR;

namespace Code.Challenge.Application.AddClientService
{
    /// <summary>
    /// The <see cref="AddClientCommandRequest"/> Command Request
    /// </summary>
    public class AddClientCommandRequest : IRequest<AddClientCommandResponse>
    {
        /// <summary>
        /// The request Person Id
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// The request First Name
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// The request Last Name
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// The request Current Role
        /// </summary>
        public string CurrentRole { get; set; } = default!;

        /// <summary>
        /// The request Country
        /// </summary>
        public string Country { get; set; } = default!;

        /// <summary>
        /// The request Industry
        /// </summary>
        public string Industry { get; set; } = default!;

        /// <summary>
        /// The request Number Of Recommendations
        /// </summary>
        public int? NumberOfRecommendations { get; set; }

        /// <summary>
        /// The request Number Of Connections
        /// </summary>

        public int? NumberOfConnections { get; set; }
    }
}
