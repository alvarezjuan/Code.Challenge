using Code.Challenge.Transversal.Internal;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Reflection;
using System.Web.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring Transversal Non Functional Responsabilities.
    /// </summary>
    public static class TransversalServiceCollectionExtensions
    {
        /// <summary>
        /// Register Transversal Non Functional Cross-cutting concerns Behaviors.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddTransversal(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                // Register all Cross-cutting concern Behaviors
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExcepcionHandlingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuditingBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InstrumentacionBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                
                // Register all services Handlers
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);

            return services;
        }

        /// <summary>
        /// Register the middleware for Transversal Non Functional Cross-cutting concerns to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseTransversal(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex?.Error != null)
                    {
                        if (ex.Error is HttpResponseException hrex)
                        {
                            // All pipeline exception fall here.
                            context.Response.StatusCode = (int)hrex.Response.StatusCode;
                            context.Response.ContentType = hrex.Response.Content.Headers.ContentType?.MediaType ?? Application.Json;
                            var scontent = await hrex.Response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            await context.Response.WriteAsync(scontent).ConfigureAwait(false);
                            return;
                        }
                    }

                    // All non pipeline exception fall here.
                    context.RequestServices.GetService<ILogger>()?.LogError(ex?.Error, "Error [unknown] exception [{exception}]", ex?.Error?.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = Text.Plain;
                    await context.Response.WriteAsync(ex?.Error?.Message ?? string.Empty).ConfigureAwait(false);
                });
            });

            return app;
        }
    }
}