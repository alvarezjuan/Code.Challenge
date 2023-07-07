using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Code.Challenge.Tests
{
    internal static class TestHelpers
    {
        public static IMediator BuildSut()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddTransversal();
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();
            
            var app = builder.Build();
            app.UseTransversal();
            app.UseInfrastructure();
            
            var scope = app.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            
            var sut = scopedServices.GetService<IMediator>();
            
            Assert.IsNotNull(sut);
            
            return sut;
        }
    }
}