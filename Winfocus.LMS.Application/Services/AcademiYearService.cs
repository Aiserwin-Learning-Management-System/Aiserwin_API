using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
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
        private readonly ILogger<AcademiYearService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcademiYearService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public AcademiYearService(IAcademicYearRepository repository, ILogger<AcademiYearService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>AcademicYearDto.</returns>
        public async Task<CommonResponse<List<AcademicYearDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all academic years.");
            var academicyear = await _repository.GetAllAsync();
            var data = academicyear.Select(Map).ToList();
            return CommonResponse<List<AcademicYearDto>>.SuccessResponse("Academic years", data);
        }

        /// <summary>
        /// Gets the current academic year based on the system date.
        /// </summary>
        /// <returns>The current academic year if available; otherwise null.</returns>
        public async Task<AcademicYearDto?> GetCurrentAcademicYearAsync()
        {
            _logger.LogInformation("Fetching current academic year.");
            var today = DateTime.UtcNow.Date;

            var academicYear = await _repository
                .GetByDateAsync(today);

            return academicYear == null ? null : Map(academicYear);
        }

        private static AcademicYearDto Map(AcademicYear c) =>
         new AcademicYearDto
         {
             Id = c.Id,
             Name = c.Name,
             StartDate = c.StartDate,
             EndDate = c.EndDate,
             CreatedAt = c.CreatedAt,
             CreatedBy = c.CreatedBy,
             UpdatedAt = c.UpdatedAt,
             UpdatedBy = c.UpdatedBy,
             IsActive = c.IsActive,
         };
    }
}
