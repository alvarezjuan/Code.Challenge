using Code.Challenge.Application.AddClientService;
using Code.Challenge.Application.ClientPositionService;
using Code.Challenge.Application.TopClientsService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring Api routes.
    /// </summary>
    public static class EndpointsServiceCollectionExtensions
    {
        /// <summary>
        /// Register Api User Endpoints.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IEndpointRouteBuilder UseMapUserEndpoints(this IEndpointRouteBuilder router)
        {
            router
                .MapGet("/api/topclients/{n}",
                    ([FromRoute] int n, [FromServices] IMediator mediator, CancellationToken cancellationToken)
                        => mediator.Send(new TopClientsQueryRequest() { NumberOfResults = n }, cancellationToken));
            router
                .MapGet("/api/clientposition/{id}",
                    ([FromRoute] long id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
                        => mediator.Send(new ClientPositionQueryRequest() { PersonId = id }, cancellationToken));
            router
                .MapPost("/api/client",
                    ([FromBody] AddClientCommandRequest person, [FromServices] IMediator mediator, CancellationToken cancellationToken)
                        => mediator.Send(person, cancellationToken));

            return router;
        }
    }
}