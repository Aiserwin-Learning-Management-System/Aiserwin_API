using FluentValidation;
using Winfocus.LMS.Application.DTOs.QuestionConfig;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// FluentValidation validator for <see cref="SuggestQuestionCodeDto"/>.
    /// Ensures all 7 hierarchy IDs are provided for code suggestion.
    /// </summary>
    public class SuggestQuestionCodeValidator : AbstractValidator<SuggestQuestionCodeDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestQuestionCodeValidator"/> class.
        /// </summary>
        public SuggestQuestionCodeValidator()
        {
            RuleFor(x => x.SyllabusId)
                .NotEmpty()
                .WithMessage("Syllabus is required.");

            RuleFor(x => x.AcademicYearId)
                .NotEmpty()
                .WithMessage("Academic Year is required.");

            RuleFor(x => x.GradeId)
                .NotEmpty()
                .WithMessage("Grade is required.");

            RuleFor(x => x.SubjectId)
                .NotEmpty()
                .WithMessage("Subject is required.");

            RuleFor(x => x.UnitId)
                .NotEmpty()
                .WithMessage("Unit is required.");

            RuleFor(x => x.ChapterId)
                .NotEmpty()
                .WithMessage("Chapter is required.");

            RuleFor(x => x.QuestionTypeId)
                .NotEmpty()
                .WithMessage("Question Type is required.");
        }
    }
}
