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
    /// ExamSubjectService.
    /// </summary>
    public class ExamSubjectService : IExamSubjectService
    {
        private readonly IExamSubjectRepository _repository;
        private readonly ILogger<ExamSubjectService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSubjectService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ExamSubjectService(IExamSubjectRepository repository, ILogger<ExamSubjectService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSubjectDto.</returns>
        public async Task<CommonResponse<List<ExamSubjectDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Exam subject");
            var ExamSubject = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Exam subject", ExamSubject.Count());
            var mappeddata = ExamSubject.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ExamSubjectDto>>.SuccessResponse("Fetching all Exam subject", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamSubjectDto>>.FailureResponse("no doubt Exam subject");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="gradeId">The identifier.</param>
        /// <returns>ExamSubjectDto.</returns>
        public async Task<CommonResponse<ExamSubjectDto>> GetByIdAsync(Guid id, Guid gradeId)
        {
            _logger.LogInformation("Fetching exam subjects details by Id: {Id}", id);
            var ExamSubject = await _repository.GetByIdAsync(id, gradeId);
            _logger.LogInformation("Exam subjects details fetched successfully for Id: {Id}", id);
            var mappeddata = ExamSubject == null ? null : Map(ExamSubject);
            if (mappeddata != null)
            {
                return CommonResponse<ExamSubjectDto>.SuccessResponse("Exam Subjects details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ExamSubjectDto>.FailureResponse("Exam Subjects details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSubjectDto.</returns>
        public async Task<CommonResponse<ExamSubjectDto>> CreateAsync(ExamSubjectRequestDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.name, request.gradeId);
                if (alreadyexist)
                {
                    return CommonResponse<ExamSubjectDto>.FailureResponse("Exam Subjects Already exist.");
                }

                var ExamSubject = new ExamSubject
                {
                    Name = request.name,
                    Description = request.description,
                    Code = request.code,
                    GradeId = request.gradeId,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(ExamSubject);
                _logger.LogInformation(
               "Exam subject created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamSubjectDto>.SuccessResponse(
                  "Exam subject created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Exam subject : {Name}", request.name);
                return CommonResponse<ExamSubjectDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam Subjects not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ExamSubjectDto>> UpdateAsync(Guid id, ExamSubjectRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating exam Subjects Id: {Id}", id);

                var ExamSubject = await _repository.GetByIdAsync(id, request.gradeId);
                if (ExamSubject == null)
                {
                    return CommonResponse<ExamSubjectDto>.FailureResponse("Exam Subjects not found");
                }

                ExamSubject.Name = request.name;
                ExamSubject.Description = request.description;
                ExamSubject.GradeId = request.gradeId;
                ExamSubject.Code = request.code;
                ExamSubject.UpdatedAt = DateTime.UtcNow;
                ExamSubject.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(ExamSubject);

                _logger.LogInformation("Exam subject updated Id: {Id}", id);
                return CommonResponse<ExamSubjectDto>.SuccessResponse(
                    "Exam subject updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Exam subject Id: {Id}", id);
                return CommonResponse<ExamSubjectDto>.FailureResponse(
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
            _logger.LogInformation("Deleting Exam subject session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Exam subject session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Exam subject deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<ExamSubjectDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Exam subject. Filters => Active:{Active}, " +
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
                        x.Grade.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamSubjectDto>>.SuccessResponse(
                        "No exam subject found.",
                        new PagedResult<ExamSubjectDto>(
                            new List<ExamSubjectDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "gradename" => isDesc ? query.OrderByDescending(x => x.Grade.Name)
                                             : query.OrderBy(x => x.Grade.Name),

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
                    "Returning {Count} of {Total} exam Subjects",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ExamSubjectDto>>.SuccessResponse(
                    "exam subject data fetched successfully.",
                    new PagedResult<ExamSubjectDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered exam subject.");
                return CommonResponse<PagedResult<ExamSubjectDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeId">The identifier.</param>
        /// <returns>ExamSubjectDto.</returns>
        public async Task<CommonResponse<List<ExamSubjectDto>>> GetByGradeIdAsync(Guid gradeId)
        {
            _logger.LogInformation("Fetching exam subject details by grade Id: {Id}", gradeId);
            var examsubject = await _repository.GetByGradeIdAsync(gradeId);
            _logger.LogInformation("Exam subject details by grade fetched successfully for Id: {Id}", gradeId);
            var mappeddata = examsubject == null ? null : Map(examsubject);
            if (mappeddata != null)
            {
                return CommonResponse<List<ExamSubjectDto>>.SuccessResponse("Exam subject by grade details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamSubjectDto>>.FailureResponse("Exam subject by grade details not found");
            }
        }

        private static List<ExamSubjectDto> Map(IEnumerable<ExamSubject> ExamSubject)
        {
            return ExamSubject.Select(Map).ToList();
        }

        private static ExamSubjectDto Map(ExamSubject c) =>
       new ExamSubjectDto
       {
           Id = c.Id,
           Name = c.Name,
           Description = c.Description,
           GradeId = c.GradeId,
           Code = c.Code,
           IsActive = c.IsActive,
           Grade = c.Grade == null ? null : new ExamGradeDto
           {
               Id = c.GradeId,
               Name = c.Grade.Name,
               SyllabusName = c.Grade.Syllabus.Name,
               AcademicYearId = c.Grade.Syllabus.AcademicYearId,
               AcademicYearName = c.Grade.Syllabus.AcademicYear.Name,
           }
       };
    }
}
