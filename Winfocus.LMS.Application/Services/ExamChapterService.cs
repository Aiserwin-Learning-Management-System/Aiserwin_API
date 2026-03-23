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
    public class ExamChapterService : IExamChapterService
    {
        private readonly IExamChapterRepository _repository;
        private readonly ILogger<ExamChapterService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamChapterService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ExamChapterService(IExamChapterRepository repository, ILogger<ExamChapterService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamChapterDto.</returns>
        public async Task<CommonResponse<List<ExamChapterDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Exam chapter");
            var Examchapter = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} Exam chapter", Examchapter.Count());
            var mappeddata = Examchapter.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ExamChapterDto>>.SuccessResponse("Fetching all Exam chapter", mappeddata);
            }
            else
            {
                return CommonResponse<List<ExamChapterDto>>.FailureResponse("Not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="unitid">The identifier.</param>
        /// <returns>ExamChapterDto.</returns>
        public async Task<CommonResponse<ExamChapterDto>> GetByIdAsync(Guid id, Guid unitid)
        {
            _logger.LogInformation("Fetching Exam chapter details by Id: {Id}", id);
            var examchapter = await _repository.GetByIdAsync(id, unitid);
            _logger.LogInformation("Exam chapter details fetched successfully for Id: {Id}", id);
            var mappeddata = examchapter == null ? null : Map(examchapter);
            if (mappeddata != null)
            {
                return CommonResponse<ExamChapterDto>.SuccessResponse("Exam chapter details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ExamChapterDto>.FailureResponse("Exam chapter details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamUnitDto.</returns>
        public async Task<CommonResponse<ExamChapterDto>> CreateAsync(ExamChapterRequestDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.name, request.unitId);
                if (alreadyexist)
                {
                    return CommonResponse<ExamChapterDto>.FailureResponse("Exam chapter Already exist.");
                }

                var ExamChapter = new ExamChapter
                {
                    Name = request.name,
                    Description = request.description,
                    ChapterNumber = request.chapterNumber,
                    UnitId = request.unitId,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(ExamChapter);
                _logger.LogInformation(
               "Exam chapter created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamChapterDto>.SuccessResponse(
                  "Exam chapter created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Exam unit : {Name}", request.name);
                return CommonResponse<ExamChapterDto>.FailureResponse(
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
        public async Task<CommonResponse<ExamChapterDto>> UpdateAsync(Guid id, ExamChapterRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating Exam chapter Id: {Id}", id);

                var ExamChapter = await _repository.GetByIdAsync(id, request.unitId);
                if (ExamChapter == null)
                {
                    return CommonResponse<ExamChapterDto>.FailureResponse("Exam chapter not found");
                }

                ExamChapter.Name = request.name;
                ExamChapter.Description = request.description;
                ExamChapter.UnitId = request.unitId;
                ExamChapter.ChapterNumber = request.chapterNumber;
                ExamChapter.UpdatedAt = DateTime.UtcNow;
                ExamChapter.UpdatedBy = request.userid;

                var updated = await _repository.UpdateAsync(ExamChapter);

                _logger.LogInformation("Exam chapter updated Id: {Id}", id);
                return CommonResponse<ExamChapterDto>.SuccessResponse(
                    "Exam chapter updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Exam chapter Id: {Id}", id);
                return CommonResponse<ExamChapterDto>.FailureResponse(
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
            _logger.LogInformation("Deleting Exam chapter session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Exam chapter session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Exam chapter deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets filtered exam chapter.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated exam chapter result.</returns>
        public async Task<CommonResponse<PagedResult<ExamChapterDto>>> GetFilteredAsync(
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

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToString().Contains(searchTerm) ||
                        x.Unit.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamChapterDto>>.SuccessResponse(
                        "No exam chapter found.",
                        new PagedResult<ExamChapterDto>(
                            new List<ExamChapterDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "subjectname" => isDesc ? query.OrderByDescending(x => x.Unit.Name)
                                             : query.OrderBy(x => x.Unit.Name),

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
                    "Returning {Count} of {Total} Exam chapter",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ExamChapterDto>>.SuccessResponse(
                    "Exam chapter data fetched successfully.",
                    new PagedResult<ExamChapterDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered Exam chapter.");
                return CommonResponse<PagedResult<ExamChapterDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<ExamChapterDto> Map(IEnumerable<ExamChapter> examChapter)
        {
            return examChapter.Select(Map).ToList();
        }

        private static ExamChapterDto Map(ExamChapter c) =>
       new ExamChapterDto
       {
           Id = c.Id,
           Name = c.Name,
           Description = c.Description,
           UnitId = c.UnitId,
           ChapterNumber = c.ChapterNumber,
           IsActive = c.IsActive,
           UnitName = c.Unit.Name,
           SubjectId = c.Unit.SubjectId,
           SubjectName = c.Unit.Subject.Name,
           GradeId = c.Unit.Subject.GradeId,
           GradeName = c.Unit.Subject.Grade.Name,
           SyllabusId = c.Unit.Subject.Grade.SyllabusId,
           SyllabusName = c.Unit.Subject.Grade.Syllabus.Name,
       };
    }
}
