using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Exam;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.EntityFrameworkCore;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Exam"/> entities.
    /// </summary>
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ILogger<ExamService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamService"/> class.
        /// </summary>
        /// <param name="examrepository">Repository used for data access.</param>
        /// <param name="logger">Logger instance.</param>
        public ExamService(IExamRepository examrepository, ILogger<ExamService> logger)
        {
            _examRepository = examrepository ?? throw new ArgumentNullException(nameof(examrepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamDto.</returns>
        public async Task<CommonResponse<ExamDto>> CreateAsync(ExamRequest request)
        {
            try
            {
                var examsreq = new Exam
                {
                    CountryId = request.countryId,
                    CenterId = request.centerId,
                    SyllabusId = request.syllabusId,
                    //Mode = request.mode,
                    GradeId = request.gradeId,
                    StreamId = request.streamId,
                    CourseId = request.courseId,
                    UnitId = request.unitId,
                    ChapterId = request.chapterId,
                    QuestionTypeId = request.questionTypeId,
                    ExamTitle = request.examTitle,
                    ExamQuestionNumber = request.examQuestionNumber,
                    ExamDate = request.examDate,
                    ExamDuration = request.examDuration,
                    TotalMark = request.totalMark,
                    PassingMark = request.passingMark,
                    Status = ExamStatus.Draft,
                    CreatedBy = request.userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _examRepository.AddAsync(examsreq);
                _logger.LogInformation(
               "exam created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ExamDto>.SuccessResponse(
                  "batch created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating batch");
                return CommonResponse<ExamDto>.FailureResponse(
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
                _logger.LogInformation("Deleting exams Id: {Id}", id);
                bool result = await _examRepository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "exam deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "exam not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamDto.</returns>
        public async Task<CommonResponse<List<ExamDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all exams");
                var exams = await _examRepository.GetAllAsync();
                _logger.LogInformation("Fetched {Count} exams", exams.Count());
                var res = exams.Select(Map).ToList();
                if (res.Any())
                {
                    return CommonResponse<List<ExamDto>>.SuccessResponse("fetching all exams", res);
                }
                else
                {
                    return CommonResponse<List<ExamDto>>.FailureResponse("no exam found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exams.");
                return CommonResponse<List<ExamDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ExamDto.</returns>
        public async Task<CommonResponse<ExamDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching exams by Id: {Id}", id);
            var exams = Map(await _examRepository.GetByIdAsync(id));
            if (exams == null)
            {
                _logger.LogWarning("exam not found for Id: {Id}", id);
                return CommonResponse<ExamDto>.FailureResponse("no exam found");
            }
            else
            {
                _logger.LogInformation("exam fetched successfully for Id: {Id}", id);
                return CommonResponse<ExamDto>.SuccessResponse("fetching all exams", exams);
            }
        }

        /// <summary>
        /// Gets filtered batches with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated exam result.</returns>
        public async Task<CommonResponse<PagedResult<ExamDto>>> GetFilteredAsync(PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered exams. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _examRepository.Query();

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
                        x.ExamTitle.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ExamDto>>.SuccessResponse(
                        "No exams found.",
                        new PagedResult<ExamDto>(
                            new List<ExamDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "title" => isDesc ? query.OrderByDescending(x => x.ExamTitle)
                                             : query.OrderBy(x => x.ExamTitle),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var exams = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = exams.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} exams",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ExamDto>>.SuccessResponse(
                    "exam fetched successfully.",
                    new PagedResult<ExamDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered exams.");
                return CommonResponse<PagedResult<ExamDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="examId">The identifier.</param>
        /// <returns>task.</returns>
        public Task<CommonResponse<List<ExamQuestion>>> GetQuestionsForExamAsync(Guid examId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Batch not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ExamDto>> UpdateAsync(Guid id, ExamRequest request)
        {
            try
            {
                _logger.LogInformation("Updating exam Id: {Id}", id);

                var exam = await _examRepository.GetByIdAsync(id);
                if (exam == null)
                {
                    return CommonResponse<ExamDto>.FailureResponse("exam not found");
                }

                exam.CountryId = request.countryId;
                exam.CenterId = request.centerId;
                exam.SyllabusId = request.syllabusId;
               // exam.Mode = request.mode;
                exam.GradeId = request.gradeId;
                exam.StreamId = request.streamId;
                exam.CourseId = request.courseId;
                exam.UnitId = request.unitId;
                exam.ChapterId = request.chapterId;
                exam.QuestionTypeId = request.questionTypeId;
                exam.ExamTitle = request.examTitle;
                exam.ExamQuestionNumber = request.examQuestionNumber;
                exam.ExamDate = request.examDate;
                exam.ExamDuration = request.examDuration;
                exam.TotalMark = request.totalMark;
                exam.PassingMark = request.passingMark;
                exam.Status = ExamStatus.Draft;
                exam.UpdatedAt = DateTime.UtcNow;
                exam.UpdatedBy = request.userid;
                exam.UpdatedBy = request.userid;

                var updated = await _examRepository.UpdateAsync(exam);

                _logger.LogInformation("exam updated Id: {Id}", id);
                return CommonResponse<ExamDto>.SuccessResponse(
                    "exam updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exam Id: {Id}", id);
                return CommonResponse<ExamDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static ExamDto Map(Exam c) =>
   new ExamDto
   {
       Id = c.Id,
       CountryId = c.CountryId,
       CenterId = c.CenterId,
       SyllabusId = c.SyllabusId,
      // Mode = c.Mode,
       GradeId = c.GradeId,
       StreamId = c.StreamId,
       CourseId = c.CourseId,
       UnitId = c.UnitId,
       ChapterId = c.ChapterId,
       QuestionTypeId = c.QuestionTypeId,
       ExamTitle = c.ExamTitle,
       ExamDate = c.ExamDate,
       ExamDuration = c.ExamDuration,
       ExamQuestionNumber = c.ExamQuestionNumber,
       TotalMark = c.TotalMark,
       PassingMark = c.PassingMark,
       Status = c.Status,
       IsActive = c.IsActive,
   };
    }
}
