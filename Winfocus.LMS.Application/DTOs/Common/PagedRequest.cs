namespace Winfocus.LMS.Application.DTOs.Common
{
    /// <summary>
    /// PagedRequest.
    /// </summary>
    public class PagedRequest
    {
        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public int Limit { get; set; } = 20;

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        /// <value>
        /// The sort by.
        /// </value>
        public string SortBy { get; set; } = "FullName";

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public string SortOrder { get; set; } = "asc";
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
