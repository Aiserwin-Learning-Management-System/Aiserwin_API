namespace Winfocus.LMS.Application.Tests.Validation
{
    using FluentValidation.TestHelper;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Validators;
    using Xunit;

    /// <summary>
    /// Tests for LoginRequestDto validation rules.
    /// </summary>
    public sealed class LoginRequestDtoValidatorTests
    {
        private readonly LoginRequestDtoValidator _validator = new ();

        /// <summary>
        /// Shoulds the fail when username is empty.
        /// </summary>
        [Fact]
        public void Should_Fail_When_Username_Is_Empty()
        {
            var dto = new LoginRequestDto(string.Empty, "Password@123");

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.username);
        }

        /// <summary>
        /// Shoulds the fail when password is empty.
        /// </summary>
        [Fact]
        public void Should_Fail_When_Password_Is_Empty()
        {
            var dto = new LoginRequestDto("user", string.Empty);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.password);
        }

        /// <summary>
        /// Shoulds the pass for valid input.
        /// </summary>
        [Fact]
        public void Should_Pass_For_Valid_Input()
        {
            var dto = new LoginRequestDto("user", "Password@123");

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
