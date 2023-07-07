using System.ComponentModel.DataAnnotations;

namespace Code.Challenge.Domain
{
    /// <summary>
    /// The <see cref="PersonEntity"/> Domain Entity
    /// </summary>
    public class PersonEntity
    {
        /// <summary>
        /// The entity Person Id
        /// </summary>
        [Key]
        public long PersonId { get; set; }

        /// <summary>
        /// The entity First Name
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// The entity Last Name
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// The entity Current Role
        /// </summary>
        public string CurrentRole { get; set; } = default!;

        /// <summary>
        /// The entity Country
        /// </summary>
        public string Country { get; set; } = default!;

        /// <summary>
        /// The entity Industry
        /// </summary>
        public string Industry { get; set; } = default!;

        /// <summary>
        /// The entity Number Of Recommendations
        /// </summary>
        public int? NumberOfRecommendations { get; set; }

        /// <summary>
        /// The entity Number Of Connections
        /// </summary>
        public int? NumberOfConnections { get; set; }
    }
}