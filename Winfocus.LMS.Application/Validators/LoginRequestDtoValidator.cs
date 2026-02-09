namespace Winfocus.LMS.Application.Validators
{
    using FluentValidation;
    using Winfocus.LMS.Application.DTOs.Auth;

    /// <summary>
    /// Validator for LoginRequestDto.
    /// </summary>
    public sealed class LoginRequestDtoValidator
        : AbstractValidator<LoginRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequestDtoValidator"/> class.
        /// </summary>
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.username)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
