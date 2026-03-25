using FluentValidation;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for DarRequestDto when updating a Daily Activity Report.
    /// </summary>
    public class UpdateDarValidator : AbstractValidator<DarRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDarValidator"/> class.
        /// </summary>
        public UpdateDarValidator()
        {
            RuleFor(x => x.ReportDate)
                .NotEmpty().WithMessage("Report date is required")
                .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Report date cannot be in the future");

            RuleFor(x => x.QuestionsTyped)
                .GreaterThanOrEqualTo(0).WithMessage("Questions typed must be at least 0")
                .LessThanOrEqualTo(1000).WithMessage("Questions typed cannot exceed 1000");

            RuleFor(x => x.TimeSpentHours)
                .GreaterThan(0).WithMessage("Time spent must be greater than 0")
                .LessThanOrEqualTo(24).WithMessage("Time spent cannot exceed 24 hours");

            RuleFor(x => x.IssuesFaced)
                .MaximumLength(1000).WithMessage("Issues faced must not exceed 1000 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(1000).WithMessage("Remarks must not exceed 1000 characters");
        }
    }
}
