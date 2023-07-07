using MediatR;
using System.Diagnostics;

namespace Code.Challenge.Transversal.Internal
{
    /// <summary>
    /// Instrumentation Transversal Non Functional Behaviour.
    /// </summary>
    internal class InstrumentacionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        /// <summary>
        /// The <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The Dependency Injection <see cref="InstrumentacionBehavior"/> constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger<InstrumentacionBehavior<TRequest, TResponse>>"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="ILogger<ArgumentNullException<TRequest, TResponse>>"/> for missing dependencies</exception>
        public InstrumentacionBehavior(ILogger<InstrumentacionBehavior<TRequest, TResponse>> logger)
            => this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Handle Instrumentation Cross-cutting Concern Behavior.
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

            var sw = Stopwatch.StartNew();

            try
            {
                var response = await next();

                return response;
            }
            finally
            {
                sw.Stop();
                this._logger.LogInformation("Instrumented [{opName}] take [{elapsed} ms]", opName, sw.ElapsedMilliseconds);
            }
        }
    }
}