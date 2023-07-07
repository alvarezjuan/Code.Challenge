
/// <summary>
/// Code Challenge Solution : Web Api application for Client operations
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddTransversal();
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();
}

// Swagger/OpenAPI Configuarion (https://aka.ms/aspnetcore/swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure services to the pipeline.
{
    app.UseTransversal();
    app.UseInfrastructure();
    app.UseMapUserEndpoints();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// Start Web Api Service
app.Run();