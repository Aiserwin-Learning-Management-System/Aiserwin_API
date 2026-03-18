namespace Winfocus.LMS.Application.Validators
{
    using FluentValidation;
    using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;

    /// <summary>
    /// Validator for <see cref="BulkCreateQuestionTypeConfigDto"/>.
    /// </summary>
    public class BulkCreateQuestionTypeConfigValidator : AbstractValidator<BulkCreateQuestionTypeConfigDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkCreateQuestionTypeConfigValidator"/> class.
        /// </summary>
        public BulkCreateQuestionTypeConfigValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .Must(items => items.Count <= 50).WithMessage("Maximum 50 items per request.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateQuestionTypeConfigValidator());
        }
    }
}
