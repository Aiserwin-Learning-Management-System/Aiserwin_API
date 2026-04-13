namespace Winfocus.LMS.Application.DTOs.Common
{
    /// <summary>
    /// Common paged request for all filter endpoints.
    /// </summary>
    public class PagedRequest
    {
        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        public int Limit { get; set; } = 20;

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the sort by column.
        /// </summary>
        public string SortBy { get; set; } = "CreatedAt";

        /// <summary>
        /// Gets or sets the sort order (asc/desc).
        /// </summary>
        public string SortOrder { get; set; } = "asc";

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Gets or sets the active status filter.
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets the start date filter.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date filter.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the academic year filter.
        /// null = show all, true = only with academic year, false = only without academic year.
        /// </summary>
        public bool? AcademicYear { get; set; }
    }

    /// <summary>
    /// PagedResult.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="totalCount"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <typeparam name="T">The type of items in the paged result.</typeparam>
    public record PagedResult<T>(
        IEnumerable<T> items,
        int totalCount,
        int limit,
        int offset);
}
