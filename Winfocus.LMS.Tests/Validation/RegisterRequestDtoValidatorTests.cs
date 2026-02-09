namespace Winfocus.LMS.Application.Tests.Validation
{
    using FluentValidation.TestHelper;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.Validators;
    using Xunit;

    /// <summary>
    /// Tests for RegisterRequestDto validation rules.
    /// </summary>
    public sealed class RegisterRequestDtoValidatorTests
    {
        private readonly RegisterRequestDtoValidator _validator = new();

        /// <summary>
        /// Shoulds the fail when username is empty.
        /// </summary>
        [Fact]
        public void Should_Fail_When_Username_Is_Empty()
        {
            var dto = new RegisterRequestDto(
                "", "test@winfocus.com", "Password@123", null);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.username);
        }

        /// <summary>
        /// Shoulds the fail when email is invalid.
        /// </summary>
        [Fact]
        public void Should_Fail_When_Email_Is_Invalid()
        {
            var dto = new RegisterRequestDto(
                "user", "invalid-email", "Password@123", null);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.email);
        }

        /// <summary>
        /// Shoulds the fail when password is too short.
        /// </summary>
        [Fact]
        public void Should_Fail_When_Password_Is_Too_Short()
        {
            var dto = new RegisterRequestDto(
                "user", "test@winfocus.com", "123", null);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.password);
        }
    }
}
