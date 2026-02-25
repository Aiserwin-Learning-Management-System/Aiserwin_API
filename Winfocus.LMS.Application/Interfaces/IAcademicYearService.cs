using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

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
        Task<CommonResponse<List<AcademicYearDto>>> GetAllAsync();

        /// <summary>
        /// Gets the currently active academic year.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        Task<AcademicYearDto?> GetCurrentAcademicYearAsync();
    }
}
