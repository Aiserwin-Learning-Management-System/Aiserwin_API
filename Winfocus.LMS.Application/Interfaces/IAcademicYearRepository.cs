using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="AcademicYear"/> entities.
    /// </summary>
    public interface IAcademicYearRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>BatchTimingMTF.</returns>
        Task<IReadOnlyList<AcademicYear>> GetAllAsync();
    }
}
