namespace Winfocus.LMS.Application.Helpers
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Shared helper to extract hierarchy info from TaskAssignment.
    /// Used by Review, Correction, and Dashboard services.
    /// </summary>
    public static class HierarchyMapper
    {
        /// <summary>
        /// Gets the task code.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>The task code in format T-YYYYMMDD-XXXX or "N/A" if task is null.</returns>
        public static string GetTaskCode(TaskAssignment? task)
        {
            if (task == null) return "N/A";
            return $"T-{task.CreatedAt:yyyyMMdd}-{task.Id.ToString()[..4].ToUpper()}";
        }

        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>The year or null if task is null.</returns>
        public static int? GetYear(TaskAssignment? task) => task?.Year;

        /// <summary>
        /// Gets the syllabus.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>The syllabus name or "N/A" if task or syllabus is null.</returns>
        public static string GetSyllabus(TaskAssignment? task) => task?.Syllabus?.Name ?? "N/A";

        /// <summary>
        /// Gets the grade.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static string GetGrade(TaskAssignment? task) => task?.Grade?.Name ?? "N/A";

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static string GetSubject(TaskAssignment? task) => task?.Subject?.Name ?? "N/A";

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static string? GetUnit(TaskAssignment? task) => task?.Unit?.Name;

        /// <summary>
        /// Gets the chapter.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static string? GetChapter(TaskAssignment? task) => task?.Chapter?.Name;

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static string GetResourceType(TaskAssignment? task) => task?.ResourceType?.Name ?? "N/A";

        /// <summary>
        /// Maps the type of the question.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string MapQuestionType(int type) => type switch
        {
            0 => "MCQ",
            1 => "Short Answer",
            2 => "Long Answer",
            _ => "Unknown"
        };

        /// <summary>
        /// Maps the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static string MapStatus(int status) => status switch
        {
            0 => "Draft",
            1 => "Submitted",
            2 => "UnderReview",
            3 => "Approved",
            4 => "Rejected",
            _ => "Unknown"
        };

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        /// <param name="operator_">The operator.</param>
        /// <returns></returns>
        public static string GetOperatorName(StaffRegistration? operator_)
        {
            if (operator_?.Values == null) return "Unknown";

            var nameValue = operator_.Values.FirstOrDefault(v =>
                v.FieldName != null &&
                (v.FieldName.Equals("full_name", StringComparison.OrdinalIgnoreCase) ||
                 v.FieldName.Equals("fullname", StringComparison.OrdinalIgnoreCase) ||
                 v.FieldName.Equals("name", StringComparison.OrdinalIgnoreCase)));

            if (nameValue != null && !string.IsNullOrWhiteSpace(nameValue.Value))
                return nameValue.Value;

            var first = operator_.Values.FirstOrDefault(v =>
                v.FieldName?.Equals("first_name", StringComparison.OrdinalIgnoreCase) == true)?.Value;
            var last = operator_.Values.FirstOrDefault(v =>
                v.FieldName?.Equals("last_name", StringComparison.OrdinalIgnoreCase) == true)?.Value;

            if (!string.IsNullOrWhiteSpace(first))
                return string.IsNullOrWhiteSpace(last) ? first : $"{first} {last}";

            return "Unknown";
        }

        /// <summary>
        /// Builds the review history.
        /// </summary>
        /// <param name="reviews">The reviews.</param>
        /// <returns></returns>
        public static List<DTOs.Review.ReviewHistoryDto> BuildReviewHistory(
            ICollection<QuestionReview>? reviews)
        {
            if (reviews == null || !reviews.Any()) return new();

            return reviews
                .OrderBy(r => r.ReviewedAt)
                .Select((r, i) => new DTOs.Review.ReviewHistoryDto
                {
                    Cycle = i + 1,
                    Action = r.Action == 0 ? "Approved" : "Rejected",
                    Feedback = r.Feedback,
                    ReviewerRole = r.ReviewerRole,
                    ReviewedAt = r.ReviewedAt,
                }).ToList();
        }
    }
}
