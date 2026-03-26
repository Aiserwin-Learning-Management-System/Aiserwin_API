namespace Winfocus.LMS.Application.Validators
{
    using FluentValidation;
    using Winfocus.LMS.Application.DTOs.QuestionConfig;

    /// <summary>
    /// Validator for <see cref="CreateQuestionConfigurationDto"/>.
    /// </summary>
    public class CreateQuestionConfigurationValidator : AbstractValidator<CreateQuestionConfigurationDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionConfigurationValidator"/> class.
        /// </summary>
        public CreateQuestionConfigurationValidator()
        {
            RuleFor(x => x.SyllabusId)
                .NotEmpty().WithMessage("Syllabus is required.");

            RuleFor(x => x.AcademicYearId)
                .NotEmpty().WithMessage("Academic Year is required.");

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

            RuleFor(x => x.QuestionTypeId)
                .NotEmpty().WithMessage("Question Type is required.");

            RuleFor(x => x.QuestionCode)
                .NotEmpty().WithMessage("Question Code is required.")
                .MaximumLength(100).WithMessage("Question Code must not exceed 100 characters.");
        }
    }
}
