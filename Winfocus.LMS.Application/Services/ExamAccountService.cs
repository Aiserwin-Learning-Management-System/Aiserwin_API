using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Exam;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="ExamAccount"/> entities.
    /// </summary>
    public class ExamAccountService : IExamAccountService
    {
        private readonly IExamAccountRepository _repository;
        private readonly ILogger<ExamAccountService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamAccountService"/> class.
        /// </summary>
        /// <param name="repository">Repository instance for persistence operations.</param>
        /// <param name="logger">Logger instance.</param>
        public ExamAccountService(IExamAccountRepository repository, ILogger<ExamAccountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all exam accounts, optionally scoped to a center.
        /// </summary>
        /// <param name="centerId">Center identifier for scoping results.</param>
        /// <returns>CommonResponse containing list of <see cref="ExamAccountDto"/>.</returns>
        public async Task<CommonResponse<List<ExamAccountDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all exam accounts");
                var items = await _repository.GetAllAsync();
                var dto = items.Select(Map).ToList();
                if (dto.Any())
                {
                    _logger.LogInformation("Fetched {Count} exam accounts", dto.Count);
                    return CommonResponse<List<ExamAccountDto>>.SuccessResponse("exam accounts fetched", dto);
                }

                _logger.LogInformation("No exam accounts found");
                return CommonResponse<List<ExamAccountDto>>.FailureResponse("no exam accounts found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exam accounts");
                return CommonResponse<List<ExamAccountDto>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// get the exam account.
        /// </summary>
        /// <param name="id">Identifier of the exam account to update.</param>
        /// <returns>CommonResponse containing the updated <see cref="ExamAccountDto"/>.</returns>
        public async Task<CommonResponse<ExamAccountDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching exam account by id: {Id}", id);
                var item = await _repository.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("ExamAccount not found: {Id}", id);
                    return CommonResponse<ExamAccountDto>.FailureResponse("exam account not found");
                }

                var dto = Map(item);
                _logger.LogInformation("ExamAccount {Id} fetched successfully", id);
                return CommonResponse<ExamAccountDto>.SuccessResponse("fetched", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exam account {Id}", id);
                return CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new exam account.
        /// </summary>
        /// <param name="request">Request containing exam account details.</param>
        /// <returns>CommonResponse containing created <see cref="ExamAccountDto"/>.</returns>
        public async Task<CommonResponse<ExamAccountDto>> CreateAsync(ExamAccountRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new ExamAccount for SubjectId={SubjectId}", request.subjectId);

                var entity = new ExamAccount
                {
                    ActivationStartDate = request.activationStartDate,
                    ActivationEndDate = request.activationEndDate,
                    BatchId = request.batchId,
                    StudentId = request.studentId,
                    ExamDate = request.examDate,
                    SubjectId = request.subjectId,
                    ResourceId = request.resourceId,
                    UnitId = request.unitId,
                    ChapterId = request.chapterId,
                    QuestionTypeId = request.questionTypeId,
                    ExamId = request.examId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.userId,
                };

                var created = await _repository.AddAsync(entity);
                _logger.LogInformation("Exam account created: {Id}", created.Id);
                return CommonResponse<ExamAccountDto>.SuccessResponse("exam account created", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam account for SubjectId={SubjectId}", request.subjectId);
                return CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing exam account.
        /// </summary>
        /// <param name="id">Identifier of the exam account to update.</param>
        /// <param name="request">Update values.</param>
        /// <returns>CommonResponse containing the updated <see cref="ExamAccountDto"/>.</returns>
        public async Task<CommonResponse<ExamAccountDto>> UpdateAsync(Guid id, ExamAccountRequest request)
        {
            try
            {
                _logger.LogInformation("Updating exam account id: {Id}", id);
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("ExamAccount not found for update: {Id}", id);
                    return CommonResponse<ExamAccountDto>.FailureResponse("exam account not found");
                }

                entity.ActivationStartDate = request.activationStartDate;
                entity.ActivationEndDate = request.activationEndDate;
                entity.BatchId = request.batchId;
                entity.StudentId = request.studentId;
                entity.ExamDate = request.examDate;
                entity.SubjectId = request.subjectId;
                entity.ResourceId = request.resourceId;
                entity.UnitId = request.unitId;
                entity.ChapterId = request.chapterId;
                entity.QuestionTypeId = request.questionTypeId;
                entity.ExamId = request.examId;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = request.userId;

                var updated = await _repository.UpdateAsync(entity);
                _logger.LogInformation("ExamAccount updated successfully: {Id}", id);
                return CommonResponse<ExamAccountDto>.SuccessResponse("exam account updated", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exam account {Id}", id);
                return CommonResponse<ExamAccountDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes (soft) an exam account.
        /// </summary>
        /// <param name="id">Exam account identifier.</param>
        /// <param name="centerId">Center id used to validate ownership.</param>
        /// <returns>CommonResponse indicating deletion success.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting exam account id: {Id}", id);
                var res = await _repository.DeleteAsync(id);
                if (res)
                {
                    _logger.LogInformation("ExamAccount deleted: {Id}", id);
                    return CommonResponse<bool>.SuccessResponse("exam account deleted", true);
                }

                _logger.LogWarning("Failed to delete ExamAccount {Id}", id);
                return CommonResponse<bool>.FailureResponse("exam account not found or not permitted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam account {Id}", id);
                return CommonResponse<bool>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered and paginated exam accounts according to the supplied request.
        /// </summary>
        /// <param name="request">PagedRequest containing filters, sort and pagination details.</param>
        /// <returns>CommonResponse containing a <see cref="PagedResult{ExamAccountDto}"/>.</returns>
        public async Task<CommonResponse<PagedResult<ExamAccountDto>>> GetFilteredAsync(PagedRequest request)
        {
            try
            {
                _logger.LogInformation("Fetching filtered exam accounts");

                var query = _repository.Query();

                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var s = request.SearchText.Trim().ToLower();
                    query = query.Where(x => (x.Subject != null && x.Subject.Name.ToLower().Contains(s)) || (x.Batch != null && x.Batch.Name.ToLower().Contains(s)));
                }

                var total = await query.CountAsync();
                if (total == 0)
                {
                    _logger.LogInformation("No exam accounts matched the filter");
                    return CommonResponse<PagedResult<ExamAccountDto>>.SuccessResponse("No records found", new PagedResult<ExamAccountDto>(new List<ExamAccountDto>(), 0, request.Limit, request.Offset));
                }

                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);
                query = request.SortBy.ToLower() switch
                {
                    "examdate" => isDesc ? query.OrderByDescending(x => x.ExamDate) : query.OrderBy(x => x.ExamDate),
                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                };

                var items = await query.Skip(request.Offset).Take(request.Limit).ToListAsync();
                var dto = items.Select(Map).ToList();

                _logger.LogInformation("Returning {Count} of {Total} exam accounts", dto.Count, total);
                return CommonResponse<PagedResult<ExamAccountDto>>.SuccessResponse("fetched", new PagedResult<ExamAccountDto>(dto, total, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered exam accounts");
                return CommonResponse<PagedResult<ExamAccountDto>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        private static ExamAccountDto Map(ExamAccount c)
        {
            return new ExamAccountDto
            {
                Id = c.Id,
                ActivationStartDate = c.ActivationStartDate,
                ActivationEndDate = c.ActivationEndDate,
                BatchId = c.BatchId,
                Batch = c.Batch == null ? null : new BatchDto { Id = c.Batch.Id, Name = c.Batch.Name, SubjectId = c.Batch.SubjectId },
                StudentId = c.StudentId,
                Student = c.Student == null ? null : new StudentDto { Id = c.Student.Id },
                ExamDate = c.ExamDate,
                SubjectId = c.SubjectId,
                Subject = c.Subject == null ? null : new SubjectDto { Id = c.Subject.Id, Name = c.Subject.Name, CourseId = c.Subject.CourseId },
                ResourceId = c.ResourceId,
                ResourceType = c.ResourceType == null ? null : new ContentResourceTypeDto { Id = c.ResourceType.Id, Name = c.ResourceType.Name },
                UnitId = c.UnitId,
                Unit = c.Unit == null ? null : new ExamUnitDto { Id = c.Unit.Id, Name = c.Unit.Name, SubjectId = c.Unit.SubjectId },
                ChapterId = c.ChapterId,
                Chapter = c.Chapter == null ? null : new ExamChapterDto { Id = c.Chapter.Id, Name = c.Chapter.Name, UnitId = c.Chapter.UnitId },
                QuestionTypeId = c.QuestionTypeId,
                QuestionTypeConfig = c.QuestionTypeConfig == null ? null : new QuestionTypeConfigDto { Id = c.QuestionTypeConfig.Id, Name = c.QuestionTypeConfig.Name },
                ExamId = c.ExamId,
                Exam = c.Exam == null ? null : new ExamDto { Id = c.Exam.Id, ExamDate = c.Exam.ExamDate, ExamTitle = c.Exam.ExamTitle },
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                UpdatedAt = c.UpdatedAt,
                UpdatedBy = c.UpdatedBy,
                IsActive = c.IsActive,
            };
        }
    }
}
