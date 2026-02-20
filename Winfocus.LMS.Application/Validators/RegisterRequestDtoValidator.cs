namespace Winfocus.LMS.Application.Validators
{
    using FluentValidation;
    using Winfocus.LMS.Application.DTOs.Auth;

    /// <summary>
    /// Validator for RegisterRequestDto.
    /// </summary>
    public sealed class RegisterRequestDtoValidator
        : AbstractValidator<RegisterRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequestDtoValidator"/> class.
        /// </summary>
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.username)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
