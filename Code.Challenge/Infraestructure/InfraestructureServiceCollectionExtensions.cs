using Code.Challenge.Application.Contracts;
using Code.Challenge.Domain;
using Code.Challenge.Infraestructure.Internal;
using EFCore.BulkExtensions;
using EntityFramework.Exceptions.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring Infraestructure Dependant Responsabilities.
    /// </summary>
    public static class InfraestructureServiceCollectionExtensions
    {
        private static readonly SqliteConnection conn = new("Data Source=inmemorydb; Mode=memory; Cache=shared");

        /// <summary>
        /// Register Infraestructure Dependant Responsabilities.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(conn);

            services.AddDbContext<PersonsContext>(options => options.UseSqlite(conn).UseExceptionProcessor());

            services.AddScoped<IUnitOfWork, PersonsUnitOfWork>();

            return services;
        }

        /// <summary>
        /// Register the middleware for Infraestructure Dependant Responsabilities to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            // Build repository estructure
            conn.Open();
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Persons (
    PersonId INTEGER PRIMARY KEY, 
    FirstName TEXT NOT NULL, 
    LastName TEXT NOT NULL, 
    CurrentRole TEXT NOT NULL, 
    Country TEXT NOT NULL, 
    Industry TEXT NOT NULL, 
    NumberOfRecommendations INTEGER NULL, 
    NumberOfConnections INTEGER NULL
);";
                cmd.ExecuteNonQuery();
            }

            // Load external data in repository
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var context = scopedServices.GetRequiredService<PersonsContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                using var r = new StreamReader("./people.json");

                var json = r.ReadToEnd();
                var persons = JsonSerializer.Deserialize<List<PersonEntity>>(json) ?? new List<PersonEntity>();
                context.BulkInsert(persons);

                context.SaveChanges();
            }

            return app;
        }
    }
}