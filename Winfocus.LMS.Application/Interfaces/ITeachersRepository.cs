using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs.DtpAdmin;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository for querying teachers grouped by staff category.
    /// </summary>
    public interface ITeachersRepository
    {
        /// <summary>
        /// Gets teachers grouped by staff category. If <paramref name="categoryName"/>
        /// is provided, only that category is returned.
        /// </summary>
        /// <param name="categoryName">Optional staff category name filter.</param>
        /// <returns>List of grouped teacher DTOs.</returns>
        Task<List<TeachersByCategoryDto>> GetTeachersByCategoryAsync(string? categoryName = null);
    }
}
