using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="AcademicYear"/> entities.
    /// </summary>
    public class AcademiYearService : IAcademicYearService
    {
        private readonly IAcademicYearRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcademiYearService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        public AcademiYearService(IAcademicYearRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        public async Task<IReadOnlyList<AcademicYearDto>> GetAllAsync()
        {
            var academicyear = await _repository.GetAllAsync();
            return academicyear.Select(Map).ToList();
        }

        private static AcademicYearDto Map(AcademicYear c) =>
         new AcademicYearDto
         {
             Id = c.Id,
             Name = c.Name,
             CreatedAt = c.CreatedAt,
             CreatedBy = c.CreatedBy,
             UpdatedAt = c.UpdatedAt,
             UpdatedBy = c.UpdatedBy,
             IsActive = c.IsActive,
         };
    }
}
