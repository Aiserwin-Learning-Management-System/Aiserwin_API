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
    /// ExamSyllabusService.
    /// </summary>
    public class ExamSyllabusService : IExamSyllabusService
    {
        private readonly IExamSyllabusRepository _repository;
        private readonly ILogger<ExamSyllabusService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSyllabusService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ExamSyllabusService(IExamSyllabusRepository repository, ILogger<ExamSyllabusService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSyllabusDto.</returns>
        public async Task<CommonResponse<List<ExamSyllabusDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Exam Syllabus");
            var doutclearing = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Exam Syllabus", doutclearing.Count());
            var mappeddata = doutclearing.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ExamSyllabusDto>>.SuccessResponse("Fetching all Exam Syllabus", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamSyllabusDto>>.FailureResponse("no doubt Exam Syllabus");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accademicYearId">The identifier.</param>
        /// <returns>ExamSyllabusDto.</returns>
        public async Task<CommonResponse<ExamSyllabusDto>> GetByIdAsync(Guid id, Guid accademicYearId)
        {
            _logger.LogInformation("Fetching exam Syllabus details by Id: {Id}", id);
            var examSyllabus = await _repository.GetByIdAsync(id, accademicYearId);
            _logger.LogInformation("Exam Syllabus details fetched successfully for Id: {Id}", id);
            var mappeddata = examSyllabus == null ? null : Map(examSyllabus);
            if (mappeddata != null)
            {
                return CommonResponse<ExamSyllabusDto>.SuccessResponse("Exam Syllabus details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ExamSyllabusDto>.FailureResponse("Exam Syllabus details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSyllabusDto.</returns>
        public async Task<CommonResponse<ExamSyllabusDto>> CreateAsync(ExamSyllabusRequestDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.name, request.accademicyearid);
                if (alreadyexist)
                {
                    return CommonResponse<ExamSyllabusDto>.FailureResponse("Exam Syllabus Already exist.");
                }

                var examSyllabus = new ExamSyllabus
                {
                    Name = request.name,
                    Description = request.description,
                    AcademicYearId = request.accademicyearid,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(examSyllabus);
                _logger.LogInformation(
               "Exam Syllabus created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamSyllabusDto>.SuccessResponse(
                  "Exam Syllabus created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Exam Syllabus : {Name}", request.name);
                return CommonResponse<ExamSyllabusDto>.FailureResponse(
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
        public async Task<CommonResponse<ExamSyllabusDto>> UpdateAsync(Guid id, ExamSyllabusRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating exam syllabus Id: {Id}", id);

                var examsyllabus = await _repository.GetByIdAsync(id, request.accademicyearid);
                if (examsyllabus == null)
                {
                    return CommonResponse<ExamSyllabusDto>.FailureResponse("Exam syllabus not found");
                }

                examsyllabus.Name = request.name;
                examsyllabus.Description = request.description;
                examsyllabus.AcademicYearId = request.accademicyearid;
                examsyllabus.UpdatedAt = DateTime.UtcNow;
                examsyllabus.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(examsyllabus);

                _logger.LogInformation("Exam Syllabus updated Id: {Id}", id);
                return CommonResponse<ExamSyllabusDto>.SuccessResponse(
                    "Exam Syllabus updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Exam Syllabus Id: {Id}", id);
                return CommonResponse<ExamSyllabusDto>.FailureResponse(
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
            _logger.LogInformation("Deleting Exam syllabus session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Exam syllabus session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Exam Syllabus deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<ExamSyllabusDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Exam Syllabus. Filters => Active:{Active}, " +
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
                        x.AcademicYear.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamSyllabusDto>>.SuccessResponse(
                        "No doubt clear session found.",
                        new PagedResult<ExamSyllabusDto>(
                            new List<ExamSyllabusDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "accademicname" => isDesc ? query.OrderByDescending(x => x.AcademicYear.Name)
                                             : query.OrderBy(x => x.AcademicYear.Name),

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

                return CommonResponse<PagedResult<ExamSyllabusDto>>.SuccessResponse(
                    "exam syllabus data fetched successfully.",
                    new PagedResult<ExamSyllabusDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered exam syllabus.");
                return CommonResponse<PagedResult<ExamSyllabusDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<ExamSyllabusDto> Map(IEnumerable<ExamSyllabus> doubtclearing)
        {
            return doubtclearing.Select(Map).ToList();
        }

        private static ExamSyllabusDto Map(ExamSyllabus c) =>
   new ExamSyllabusDto
   {
       Id = c.Id,
       Name = c.Name,
       Description = c.Description,
       AcademicYearId = c.AcademicYearId,
       IsActive = c.IsActive,
       AcademicYear = c.AcademicYear == null ? null : new AcademicYearDto
       {
           Id = c.AcademicYear.Id,
           Name = c.AcademicYear.Name,
           StartDate = c.AcademicYear.StartDate,
           EndDate = c.AcademicYear.EndDate
       }
   };

    }
}
