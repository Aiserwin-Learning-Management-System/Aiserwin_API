namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    /// <summary>
    /// Response DTO containing the auto-generated suggested Question Code.
    /// Returned by the suggest endpoint without saving to the database.
    /// </summary>
    public class SuggestedCodeResponseDto
    {
        /// <summary>
        /// Gets or sets the auto-generated suggested Question Code.
        /// Format: [SYL]-[YYYY]-[GRD]-[SUB]-[UNIT]-[CH]-[TYPE]-[SEQ].
        /// Example: CBSE-2025-12-PHY-U01-CH01-MCQ-0001.
        /// </summary>
        public string SuggestedCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the next sequence number for this scope.
        /// </summary>
        public int NextSequence { get; set; }
    }
}
