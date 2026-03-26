using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
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

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>AcademicYearDto.</returns>
        public async Task<CommonResponse<AcademicYearDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching academic year by Id: {Id}", id);
                var academicyear = await _repository.GetByIdAsync(id);
                _logger.LogInformation("academic year fetched successfully for Id: {Id}", id);
                var mappeddata = academicyear == null ? null : Map(academicyear);
                if (mappeddata != null)
                {
                    return CommonResponse<AcademicYearDto>.SuccessResponse("fetching academic year by id", mappeddata);
                }
                else
                {
                    return CommonResponse<AcademicYearDto>.FailureResponse("Academic year not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching academic year.");
                return CommonResponse<AcademicYearDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AcademicYearDto.</returns>
        public async Task<CommonResponse<AcademicYearDto>> CreateAsync(AcademicYearRequest request)
        {
            try
            {
                var academicYear = new AcademicYear
                {
                    Name = request.name,
                    StartDate = request.startdate,
                    EndDate = request.enddate,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(academicYear);
                _logger.LogInformation(
               "academic Year created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<AcademicYearDto>.SuccessResponse(
                  "academic year created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating academic year: {Name}", request.name);
                return CommonResponse<AcademicYearDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Academic year not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<AcademicYearDto>> UpdateAsync(Guid id, AcademicYearRequest request)
        {
            try
            {
                _logger.LogInformation("Updating academic year Id: {Id}", id);

                var academicyear = await _repository.GetByIdAsync(id);
                if (academicyear == null)
                {
                    return CommonResponse<AcademicYearDto>.FailureResponse("academic year not found");
                }

                academicyear.Name = request.name;
                academicyear.StartDate = request.startdate;
                academicyear.EndDate = request.enddate;
                academicyear.UpdatedAt = DateTime.UtcNow;
                academicyear.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(academicyear);

                _logger.LogInformation("academic year updated Id: {Id}", id);
                return CommonResponse<AcademicYearDto>.SuccessResponse(
                    "academic year updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating academic year Id: {Id}", id);
                return CommonResponse<AcademicYearDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting academic year Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Academic year deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Academic year not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Academic year Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered academic year with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated academic year result.</returns>
        public async Task<CommonResponse<PagedResult<AcademicYearDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Academic year. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query();

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Subject, Course, Stream, Grade, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<AcademicYearDto>>.SuccessResponse(
                        "No academic year found.",
                        new PagedResult<AcademicYearDto>(
                            new List<AcademicYearDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var subjects = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = subjects.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} academic year",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<AcademicYearDto>>.SuccessResponse(
                    "academic year fetched successfully.",
                    new PagedResult<AcademicYearDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered academic year.");
                return CommonResponse<PagedResult<AcademicYearDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static AcademicYearDto Map(AcademicYear c) =>
         new AcademicYearDto
         {
             Id = c.Id,
             Name = c.Name,
             StartDate = c.StartDate,
             EndDate = c.EndDate,
             IsActive = c.IsActive,
         };
    }
}
