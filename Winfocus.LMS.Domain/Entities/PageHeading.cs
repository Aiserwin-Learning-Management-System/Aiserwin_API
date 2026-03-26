namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Stores editable main heading and sub heading for each admin page.
    /// </summary>
    public class PageHeading : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique page key identifier.
        /// </summary>
        public string PageKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main heading displayed on the page.
        /// </summary>
        public string MainHeading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sub heading displayed below the main heading.
        /// </summary>
        public string SubHeading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the module this page belongs to.
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;
    }
}
