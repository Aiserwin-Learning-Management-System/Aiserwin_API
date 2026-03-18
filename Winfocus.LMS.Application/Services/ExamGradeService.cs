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
    /// ExamGradeService.
    /// </summary>
    public class ExamGradeService : IExamGradeService
    {
        private readonly IExamGradeRepository _repository;
        private readonly ILogger<ExamGradeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamGradeService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ExamGradeService(IExamGradeRepository repository, ILogger<ExamGradeService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamGradeDto.</returns>
        public async Task<CommonResponse<List<ExamGradeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Exam grade");
            var examgrade = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Exam grade", examgrade.Count());
            var mappeddata = examgrade.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ExamGradeDto>>.SuccessResponse("Fetching all Exam Grade", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamGradeDto>>.FailureResponse("no doubt Exam grade");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="syllabusID">The identifier.</param>
        /// <returns>ExamGradeDto.</returns>
        public async Task<CommonResponse<ExamGradeDto>> GetByIdAsync(Guid id, Guid syllabusID)
        {
            _logger.LogInformation("Fetching exam Syllabus details by Id: {Id}", id);
            var examgrade = await _repository.GetByIdAsync(id, syllabusID);
            _logger.LogInformation("Exam Syllabus details fetched successfully for Id: {Id}", id);
            var mappeddata = examgrade == null ? null : Map(examgrade);
            if (mappeddata != null)
            {
                return CommonResponse<ExamGradeDto>.SuccessResponse("Exam Syllabus details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ExamGradeDto>.FailureResponse("Exam Syllabus details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamGradeDto.</returns>
        public async Task<CommonResponse<ExamGradeDto>> CreateAsync(ExamGradeRequestDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.name, request.syllabusId);
                if (alreadyexist)
                {
                    return CommonResponse<ExamGradeDto>.FailureResponse("Exam Syllabus Already exist.");
                }

                var examGrade = new ExamGrade
                {
                    Name = request.name,
                    Description = request.description,
                    SyllabusId = request.syllabusId,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(examGrade);
                _logger.LogInformation(
               "Exam grade created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamGradeDto>.SuccessResponse(
                  "Exam grade created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Exam grade : {Name}", request.name);
                return CommonResponse<ExamGradeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam Syllabus not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ExamGradeDto>> UpdateAsync(Guid id, ExamGradeRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating exam syllabus Id: {Id}", id);

                var examgrade = await _repository.GetByIdAsync(id, request.syllabusId);
                if (examgrade == null)
                {
                    return CommonResponse<ExamGradeDto>.FailureResponse("Exam syllabus not found");
                }

                examgrade.Name = request.name;
                examgrade.Description = request.description;
                examgrade.SyllabusId = request.syllabusId;
                examgrade.UpdatedAt = DateTime.UtcNow;
                examgrade.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(examgrade);

                _logger.LogInformation("Exam grade updated Id: {Id}", id);
                return CommonResponse<ExamGradeDto>.SuccessResponse(
                    "Exam grade updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Exam grade Id: {Id}", id);
                return CommonResponse<ExamGradeDto>.FailureResponse(
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
            _logger.LogInformation("Deleting Exam grade session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Exam grade session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Exam grade deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<ExamGradeDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Exam grade. Filters => Active:{Active}, " +
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
                        x.Name.ToString().Contains(searchTerm) ||
                        x.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamGradeDto>>.SuccessResponse(
                        "No doubt clear session found.",
                        new PagedResult<ExamGradeDto>(
                            new List<ExamGradeDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "accademicname" => isDesc ? query.OrderByDescending(x => x.Syllabus.Name)
                                             : query.OrderBy(x => x.Syllabus.Name),

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
                    "Returning {Count} of {Total} exam Syllabus schedule session",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ExamGradeDto>>.SuccessResponse(
                    "exam grade data fetched successfully.",
                    new PagedResult<ExamGradeDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered exam grade.");
                return CommonResponse<PagedResult<ExamGradeDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<ExamGradeDto> Map(IEnumerable<ExamGrade> examgrade)
        {
            return examgrade.Select(Map).ToList();
        }

        private static ExamGradeDto Map(ExamGrade c) =>
   new ExamGradeDto
   {
       Id = c.Id,
       Name = c.Name,
       Description = c.Description,
       SyllabusId = c.SyllabusId,
       IsActive = c.IsActive,
       Syllabus = c.Syllabus == null ? null : new ExamSyllabusDto
       {
           Id = c.SyllabusId,
           Name = c.Syllabus.Name,
           AcademicYearId = c.Syllabus.AcademicYearId,
           AcademicYear =c.Syllabus.AcademicYear == null ? null : new AcademicYearDto
           {
               Name =c.Syllabus.AcademicYear.Name,
               StartDate = c.Syllabus.AcademicYear.StartDate,
               EndDate = c.Syllabus.AcademicYear.EndDate
           }
           
       }
   };
    }
}
