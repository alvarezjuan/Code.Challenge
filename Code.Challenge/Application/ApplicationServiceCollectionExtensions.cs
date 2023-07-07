using Code.Challenge.Application.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring Application Responsabilities.
    /// </summary>
    public static class ApplicationServiceCollectionExtensions
    {
        /// <summary>
        /// Register Application Responsabilities.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISharedLogic, SharedLogic>();

            return services;
        }
    }
}