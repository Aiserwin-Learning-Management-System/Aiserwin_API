using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="AcademicYear"/> entities.
    /// </summary>
    public interface IAcademicYearService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        Task<IReadOnlyList<AcademicYearDto>> GetAllAsync();
    }
}
