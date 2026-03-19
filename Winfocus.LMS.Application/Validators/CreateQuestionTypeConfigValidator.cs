namespace Winfocus.LMS.Application.Validators
{
    using FluentValidation;
    using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;

    /// <summary>
    /// Validator for <see cref="CreateQuestionTypeConfigDto"/>.
    /// </summary>
    public class CreateQuestionTypeConfigValidator : AbstractValidator<CreateQuestionTypeConfigDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionTypeConfigValidator"/> class.
        /// </summary>
        public CreateQuestionTypeConfigValidator()
        {
            RuleFor(x => x.SyllabusId)
                .NotEmpty().WithMessage("Syllabus is required.");

            RuleFor(x => x.GradeId)
                .NotEmpty().WithMessage("Grade is required.");

            RuleFor(x => x.SubjectId)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.UnitId)
                .NotEmpty().WithMessage("Unit is required.");

            RuleFor(x => x.ChapterId)
                .NotEmpty().WithMessage("Chapter is required.");

            RuleFor(x => x.ResourceTypeId)
                .NotEmpty().WithMessage("Resource Type is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Question Type name is required.")
                .MaximumLength(100).WithMessage("Question Type name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
