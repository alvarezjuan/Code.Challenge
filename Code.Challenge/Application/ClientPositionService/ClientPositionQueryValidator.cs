using Code.Challenge.Application.Contracts;
using FluentValidation;

namespace Code.Challenge.Application.ClientPositionService
{
    /// <summary>
    /// Client Position Request Validator.
    /// </summary>
    internal class ClientPositionQueryValidator : AbstractValidator<ClientPositionQueryRequest>
    {
        /// <summary>
        /// The <see cref="IUnitOfWork"/>.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Dependency Injection <see cref="ClientPositionQueryValidator"/> constructor.
        /// </summary>
        /// <param name="options">The <see cref="IUnitOfWork"/>.</param>
        public ClientPositionQueryValidator(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            RuleFor(x => x.PersonId)
                .Must(personId => this._unitOfWork.Persons.FindBy(y => y.PersonId == personId).Any())
                .WithMessage("PersonId does not exists");
        }
    }
}