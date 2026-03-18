using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// ExamUnitService.
    /// </summary>
    public class ExamUnitService : IExamUnitService
    {
        private readonly IExamUnitRepository _repository;
        private readonly ILogger<ExamUnitService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamUnitService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ExamUnitService(IExamUnitRepository repository, ILogger<ExamUnitService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamUnitDto.</returns>
        public async Task<CommonResponse<List<ExamUnitDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Exam unit");
            var ExamUnit = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Exam unit", ExamUnit.Count());
            var mappeddata = ExamUnit.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ExamUnitDto>>.SuccessResponse("Fetching all Exam unit", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamUnitDto>>.FailureResponse("no doubt Exam unit");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="subjectId">The identifier.</param>
        /// <returns>ExamUnitDto.</returns>
        public async Task<CommonResponse<ExamUnitDto>> GetByIdAsync(Guid id, Guid subjectId)
        {
            _logger.LogInformation("Fetching Exam units details by Id: {Id}", id);
            var ExamUnit = await _repository.GetByIdAsync(id, subjectId);
            _logger.LogInformation("Exam units details fetched successfully for Id: {Id}", id);
            var mappeddata = ExamUnit == null ? null : Map(ExamUnit);
            if (mappeddata != null)
            {
                return CommonResponse<ExamUnitDto>.SuccessResponse("Exam units details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ExamUnitDto>.FailureResponse("Exam units details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamUnitDto.</returns>
        public async Task<CommonResponse<ExamUnitDto>> CreateAsync(ExamUnitRequestDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.name, request.subjectId);
                if (alreadyexist)
                {
                    return CommonResponse<ExamUnitDto>.FailureResponse("Exam units Already exist.");
                }

                var ExamUnit = new ExamUnit
                {
                    Name = request.name,
                    Description = request.description,
                    UnitNumber = request.unitNumber,
                    SubjectId = request.subjectId,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(ExamUnit);
                _logger.LogInformation(
               "Exam unit created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamUnitDto>.SuccessResponse(
                  "Exam unit created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Exam unit : {Name}", request.name);
                return CommonResponse<ExamUnitDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam units not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ExamUnitDto>> UpdateAsync(Guid id, ExamUnitRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating Exam units Id: {Id}", id);

                var ExamUnit = await _repository.GetByIdAsync(id, request.subjectId);
                if (ExamUnit == null)
                {
                    return CommonResponse<ExamUnitDto>.FailureResponse("Exam units not found");
                }

                ExamUnit.Name = request.name;
                ExamUnit.Description = request.description;
                ExamUnit.SubjectId = request.subjectId;
                ExamUnit.UnitNumber = request.unitNumber;
                ExamUnit.UpdatedAt = DateTime.UtcNow;
                ExamUnit.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(ExamUnit);

                _logger.LogInformation("Exam unit updated Id: {Id}", id);
                return CommonResponse<ExamUnitDto>.SuccessResponse(
                    "Exam unit updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Exam unit Id: {Id}", id);
                return CommonResponse<ExamUnitDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting Exam unit session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Exam unit session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Exam unit deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<ExamUnitDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Exam unit. Filters => Active:{Active}, " +
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

                // ── Search on Subject, Course, Stream, subject, AND Subjects Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToString().Contains(searchTerm) ||
                        x.Subject.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamUnitDto>>.SuccessResponse(
                        "No doubt clear session found.",
                        new PagedResult<ExamUnitDto>(
                            new List<ExamUnitDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "subjectname" => isDesc ? query.OrderByDescending(x => x.Subject.Name)
                                             : query.OrderBy(x => x.Subject.Name),

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
                    "Returning {Count} of {Total} Exam units schedule session",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ExamUnitDto>>.SuccessResponse(
                    "Exam unit data fetched successfully.",
                    new PagedResult<ExamUnitDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered Exam unit.");
                return CommonResponse<PagedResult<ExamUnitDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<ExamUnitDto> Map(IEnumerable<ExamUnit> ExamUnit)
        {
            return ExamUnit.Select(Map).ToList();
        }

        private static ExamUnitDto Map(ExamUnit c) =>
   new ExamUnitDto
   {
       Id = c.Id,
       Name = c.Name,
       Description = c.Description,
       SubjectId = c.SubjectId,
       UnitNumber = c.UnitNumber,
       IsActive = c.IsActive,
       Subject = c.Subject == null ? null : new ExamSubjectDto
       {
           Id = c.SubjectId,
           Name = c.Subject.Name,
           Code =c.Subject.Code,
           GradeId = c.Subject.GradeId,
       }
   };


    }
}
