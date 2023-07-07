using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;

namespace Code.Challenge.Transversal.Internal
{
    /// <summary>
    /// Exception Handling Transversal Non Functional Behaviour.
    /// </summary>
    internal class ExcepcionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        /// <summary>
        /// The <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The Dependency Injection <see cref="ExcepcionHandlingBehavior"/> constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger<ExcepcionHandlingBehavior<TRequest, TResponse>>"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="ILogger<ArgumentNullException<TRequest, TResponse>>"/> for missing dependencies</exception>
        public ExcepcionHandlingBehavior(ILogger<ExcepcionHandlingBehavior<TRequest, TResponse>> logger)
            => this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Handle Exceptions Cross-cutting Concern Behavior.
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

            try
            {
                var response = await next();

                return response;
            }
            catch (HttpResponseException hrex)
            {
                this._logger.LogError(hrex, "Http Response Error [{opName}] exception [{exception}]", opName, hrex.Message);
                throw;
            }
            catch (FluentValidation.ValidationException vex)
            {
                this._logger.LogError(vex, "Validation Error [{opName}] exception [{exception}]", opName, vex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = JsonContent.Create(vex.Errors.Select(e => e.ErrorMessage).ToList()),
                    ReasonPhrase = vex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (UnauthorizedAccessException uaex)
            {
                this._logger.LogError(uaex, "Unauthorized Access Error [{opName}] exception [{exception}]", opName, uaex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = JsonContent.Create(uaex.Message),
                    ReasonPhrase = uaex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (NotImplementedException niex)
            {
                this._logger.LogError(niex, "Not Implemented Error [{opName}] exception [{exception}]", opName, niex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = JsonContent.Create(niex.Message),
                    ReasonPhrase = niex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (ArgumentNullException anex)
            {
                this._logger.LogError(anex, "Dependency Error [{opName}] exception [{exception}]", opName, anex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = JsonContent.Create(anex.Message),
                    ReasonPhrase = anex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (InvalidOperationException ioex)
            {
                this._logger.LogError(ioex, "Invalid OperationError [{opName}] exception [{exception}]", opName, ioex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = JsonContent.Create(ioex.Message),
                    ReasonPhrase = ioex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (DbUpdateException duex)
            {
                this._logger.LogError(duex, "Database Error [{opName}] exception [{exception}]", opName, duex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = JsonContent.Create(duex.Message),
                    ReasonPhrase = duex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (ApplicationException aex)
            {
                this._logger.LogError(aex, "Application Error [{opName}] exception [{exception}]", opName, aex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = JsonContent.Create(aex.Message),
                    ReasonPhrase = aex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error [{opName}] exception [{exception}]", opName, ex.Message);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = JsonContent.Create(ex.Message),
                    ReasonPhrase = ex.Message.Replace(Environment.NewLine, string.Empty)
                });
            }
        }
    }
}