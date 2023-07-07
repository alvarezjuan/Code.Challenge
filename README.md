
Code Challenge
============

This is a Solution for Code Challenge .

---
## Problem Statement
Develop a .NET Core RESTful API that, given an input file “people.json” with LinkedIn public data, conforms to the following implementation: 

1. An endpoint that finds the N people with the highest chance of becoming our clients, being N a parameter, as a JSON list of PersonId. 
	Ex: http://….../topclients/2 → Response: [{“PersonId”:150},{“PersonId”:985}] 

2. An endpoint that finds, for a given PersonId, the position on the priority potential clients list. 
	Ex: http://….../clientposition/150 → Response: {“Position”:1} 

Bonus implementation: Another endpoint that allows the insertion of a new Person object and calculates its priority value. 

The input file is a JSON array of objects structured like the following example: 
```
[ 
	{ 
		"PersonId": 4580, // long 
		"FirstName": "Jhon", // string 
		"LastName": "Smith", // string 
		"CurrentRole": "co-founder & cto", // string 
		"Country": "Germany", // string 
		"Industry": "United States", // string 
		"NumberOfRecommendations": 10,// int nullable 
		"NumberOfConnections": 500 // int nullable 
	}, 
	{ 
		…. 
	} 
]
```
---
## Desing Patterns
- Domain Driven Design
	- Domain Layer
	- Application Layer
	- Infrastructure Layer
	- Transversal Layer
	- Api Layer
	- Program
- IoC / Dependency Injection
	- Microsoft.Extensions.DependencyInjection
- Unit Of Work / ORM
	- Microsoft.EntityFrameworkCore
	- Microsoft.EntityFrameworkCore.Sqlite
	- EntityFrameworkCore.Exceptions.Sqlite
	- EFCore.BulkExtensions
- Chain of Responsibility
	- MediatR
	- FluentValidation
- Unit Testing
	- MSTest
---

## IOC
All classes are abstrated with corresponding interfaces and registered in dependency control container
``
    public interface IUnitOfWork
    {
        public IRepository<PersonEntity> Persons { get; }
        public Task CommitAsync(CancellationToken cancellationToken);
    }
...
            services.AddScoped<IUnitOfWork, PersonsUnitOfWork>();
``

The class dependencies are declared in constructor through interfaces

``
        private readonly IUnitOfWork _unitOfWork;
        public TopClientsQueryService(IUnitOfWork unitOfWork)
            => this._unitOfWork = unitOfWork ?? 
	            throw new ArgumentNullException(nameof(unitOfWork));
``

---

## Services/Repositories

The access to the repository is abstracted with a Unit Of Work Pattern injected into the service logic

``
    
    internal class PersonsUnitOfWork : IUnitOfWork
    {
        private readonly PersonsContext dbContext;

        public PersonsUnitOfWork(PersonsContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Persons = new RepositoryBase<PersonEntity, PersonsContext>(this.dbContext);
        }

        public IRepository<PersonEntity> Persons { get; private set; }

        public Task CommitAsync(CancellationToken cancellationToken)
            => dbContext.SaveChangesAsync(cancellationToken);
    }

``

The unit of work gives access to the infrastructure Entity Framework Context

``

    internal class PersonsContext : DbContext
    {
        public PersonsContext(DbContextOptions<PersonsContext> options) : base(options) { }

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

        public DbSet<PersonEntity> Persons { get; set; }
    }

``
---

## Chain of Responsibility


The services are processed using a pipeline pattern (MediatR).

This allows inject non functional cross-cutting concern behavior as

- Error Handling Behavior
- Auditing Behavior
- Instrumentation Behavior
- Validation Behavior (FluentValidation)

This Behaviors are processed in a centralized place avoiding the service logic to handle this non funtional requirements and uniform this concerns policies


## Unit Testing

MSTest is used to unit test the services follogind the Arrange/Act/Assert pattern to test the System Under Test component SUT

``

        [TestMethod]
        public async Task Test_FirstInsert_ReturnOk()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new AddClientCommandRequest()
            {
                PersonId = 111111,
	            ...
            };

            // Act
            var response = await sut.Send(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Priority);
        }

``

---

## Decisions in the functional aspect.

The functional prioritization of the people with the highest chance of becoming our clients is implemented by ordering each person with their number of recomendation (descending) the by their number of connections (descending)

``

            var persons = _unitOfWork.Persons
                .FindAll()
                .OrderByDescending(p => p.NumberOfRecommendations)
                .ThenByDescending(p => p.NumberOfConnections)
                .Select(p => new { p.PersonId })
                ;

``

At the start of the Service, the jsond data (people.json) is reaad and bulk (fast insert) into in memory Sqlite data base in order to speed up the response time of the api operations



``

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

``

---

## Technical requirements.

- Dotnet Core 6.0
- Docker / Docker compose
