namespace Winfocus.LMS.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;

    /// <summary>
    /// Service for teacher-related operations.
    /// </summary>
    public interface ITeachersService
    {
        /// <summary>
        /// Gets teachers grouped by staff category.
        /// </summary>
        /// <param name="category">Optional category name to filter.</param>
        /// <returns>list of TeachersByCategoryDto.</returns>
        Task<List<TeachersByCategoryDto>> GetTeachersByCategoryAsync(string? category = null);
    }
}
