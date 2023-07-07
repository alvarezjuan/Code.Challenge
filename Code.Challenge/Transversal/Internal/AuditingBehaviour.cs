using MediatR;
using Newtonsoft.Json;

namespace Code.Challenge.Transversal.Internal
{
    /// <summary>
    /// Auditing Transversal Non Functional Behaviour.
    /// </summary>
    internal class AuditingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        /// <summary>
        /// The <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The Dependency Injection <see cref="AuditingBehaviour"/> constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger<AuditingBehaviour<TRequest, TResponse>>"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="ILogger<ArgumentNullException<TRequest, TResponse>>"/> for missing dependencies</exception>
        public AuditingBehaviour(ILogger<AuditingBehaviour<TRequest, TResponse>> logger)
            => this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Handle Auditing Cross-cutting Concern Behavior.
        /// </summary>
        /// <param name="request">The <see cref="TRequest"/>.</param>
        /// <param name="next">The <see cref="RequestHandlerDelegate<TResponse>"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task<TResponse>"/> instance after the operation has completed.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = typeof(TResponse).GenericTypeArguments.Length > 0 ? typeof(TResponse).GenericTypeArguments[0].Name : typeof(TResponse).Name;
            var requestName = typeName.ToLowerInvariant();
            var opName = requestName.EndsWith("request", StringComparison.InvariantCultureIgnoreCase)
                ? requestName[0..^7] // "request".Length
                : $"{requestName}-{typeName}";

            var jsonRequest = JsonConvert.SerializeObject(request);
            this._logger.LogInformation("Auditing [{opName}] request [{jsonRequest}]", opName, jsonRequest);

            var response = await next();

            var jsonResponse = JsonConvert.SerializeObject(response);
            this._logger.LogInformation("Audited [{opName}] response [{jsonResponse}]", opName, jsonResponse);

            return response;
        }
    }
}