namespace Winfocus.LMS.Application.DTOs.QuestionTypeConfig
{
    /// <summary>
    /// Request DTO for bulk creating Question Type Configurations via "Add More" button.
    /// </summary>
    public class BulkCreateQuestionTypeConfigDto
    {
        /// <summary>
        /// Gets or sets the list of question type configurations to create.
        /// </summary>
        public List<CreateQuestionTypeConfigDto> Items { get; set; } = new List<CreateQuestionTypeConfigDto>();
    }
}
