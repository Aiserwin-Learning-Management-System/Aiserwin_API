namespace Winfocus.LMS.Application.DTOs.DtpAdmin
{
    /// <summary>
    /// Column definitions + fixed columns for building the table.
    /// </summary>
    public class OperatorColumnsResponseDto
    {
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public List<OperatorColumnDto> Columns { get; set; } = new ();

        /// <summary>
        /// Gets or sets the fixed columns.
        /// </summary>
        /// <value>
        /// The fixed columns.
        /// </value>
        public List<string> FixedColumns { get; set; } = new () { "slNo", "status", "actions" };
    }
}
