using Code.Challenge.Application.Contracts;
using FluentValidation;

namespace Code.Challenge.Application.AddClientService
{
    /// <summary>
    /// Add Client Request Validator.
    /// </summary>
    internal class AddClientCommandValidator : AbstractValidator<AddClientCommandRequest>
    {
        /// <summary>
        /// The <see cref="IUnitOfWork"/>.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Dependency Injection <see cref="AddClientCommandValidator"/> constructor.
        /// </summary>
        /// <param name="options">The <see cref="IUnitOfWork"/>.</param>
        public AddClientCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            RuleFor(x => x.PersonId)
                .Must(personId => _unitOfWork.Persons.FindBy(y => y.PersonId == personId).Any() == false)
                .WithMessage("PersonId already exists");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName can not be empty");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName can not be empty");

            RuleFor(x => x.CurrentRole)
                .NotEmpty()
                .WithMessage("CurrentRole can not be empty");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country can not be empty");

            RuleFor(x => x.Industry)
                .NotEmpty()
                .WithMessage("Industry can not be empty");
        }
    }
}