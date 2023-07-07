using Code.Challenge.Domain;
using Microsoft.EntityFrameworkCore;

namespace Code.Challenge.Infraestructure.Internal
{
    /// <summary>
    /// Repository Entity Context.
    /// </summary>
    internal class PersonsContext : DbContext
    {
        /// <summary>
        /// The Dependency Injection <see cref="PersonsContext"/> constructor.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions<PersonsContext>"/>.</param>
        public PersonsContext(DbContextOptions<PersonsContext> options) : base(options) { }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.HasKey(e => e.PersonId);
                entity.Property(e => e.FirstName).HasColumnType("VARCHAR");
                entity.Property(e => e.LastName).HasColumnType("VARCHAR");
                entity.Property(e => e.CurrentRole).HasColumnType("VARCHAR");
                entity.Property(e => e.Country).HasColumnType("VARCHAR");
                entity.Property(e => e.Industry).HasColumnType("VARCHAR");
                entity.Property(e => e.NumberOfRecommendations).HasColumnType("INTEGER");
                entity.Property(e => e.NumberOfConnections).HasColumnType("INTEGER");
            });
        }

        /// <summary>
        /// The <see cref="PersonEntity"/> Data Set
        /// </summary>
        public DbSet<PersonEntity> Persons { get; set; }
    }
}