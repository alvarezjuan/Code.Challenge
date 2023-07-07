using FluentValidation;

namespace Code.Challenge.Application.TopClientsService
{
    /// <summary>
    /// Top Clients Request Validator.
    /// </summary>
    internal class TopClientsQueryValidator : AbstractValidator<TopClientsQueryRequest>
    {
        /// <summary>
        /// The Dependency Injection <see cref="TopClientsQueryValidator"/> constructor.
        /// </summary>
        public TopClientsQueryValidator()
        {
            RuleFor(x => x.NumberOfResults)
                .GreaterThan(0)
                .WithMessage("Number of results must greater than 0");
        }
    }
}