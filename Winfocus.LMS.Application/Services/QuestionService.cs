using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Question;
using Winfocus.LMS.Application.DTOs.Review;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using TaskStatus = Winfocus.LMS.Domain.Enums.TaskStatus;
using Microsoft.EntityFrameworkCore;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service responsible for handling all business logic related to Questions.
    /// </summary>
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITaskAssignmentRepository _taskRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionService"/> class.
        /// </summary>
        /// <param name="questionRepository">Repository used for data access.</param>
        /// <param name="taskRepository">taskRepository used for data access.</param>
        /// <param name="mapper">mapper.</param>
        public QuestionService(
            IQuestionRepository questionRepository,
            ITaskAssignmentRepository taskRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateQuestionDto dto, Guid operatorId)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskId);
            if (task == null || task.OperatorId != operatorId)
                throw new Exception("Invalid task or access denied.");

            //ValidateQuestion(dto);

            var question = new Question
            {
                Id = Guid.NewGuid(),
                TaskId = dto.TaskId,
                OperatorId = operatorId,
                QuestionText = dto.QuestionText,
                Marks = dto.Marks,
                CorrectAnswer = dto.CorrectAnswer,
                CorrectAnswerText = dto.CorrectAnswerText,
                Reference = FormatReference(dto.Reference),
                QuestionType = dto.QuestionTypeId,
                Status = dto.SaveAsDraft
                    ? (int)QuestionStatus.Draft
                    : (int)QuestionStatus.Submitted
            };

            if (dto.QuestionTypeId == 0)
            {
                if (dto.Options == null || dto.Options.Count != 4)
                    throw new Exception("MCQ questions must have exactly 4 options.");

                if (string.IsNullOrWhiteSpace(dto.CorrectAnswer))
                    throw new Exception("CorrectAnswer is required for MCQ questions.");

                var labels = dto.Options.Select(o => o.OptionLabel?.Trim().ToUpper()).ToList();
                if (labels.Distinct().Count() != labels.Count || !labels.All(l => new[] { "A", "B", "C", "D" }.Contains(l)))
                    throw new Exception("Option labels must be unique and one of A,B,C,D.");

                question.Options = dto.Options.Select(o => new QuestionOption
                {
                    Id = Guid.NewGuid(),
                    OptionLabel = o.OptionLabel?.Trim().ToUpper() ?? string.Empty,
                    OptionText = o.OptionText ?? string.Empty,
                    IsCorrect = string.Equals(o.OptionLabel?.Trim(), dto.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase)
                }).ToList();
            }

            if (dto.QuestionTypeId != 0 && string.IsNullOrWhiteSpace(dto.CorrectAnswerText))
            {
            }

            await _questionRepository.AddAsync(question);

            if (!dto.SaveAsDraft)
            {
                var t = task;
                if (t != null)
                {
                    if (t.CompletedCount == 0)
                    {
                        t.Status = (int)TaskStatus.InProgress;
                    }

                    t.CompletedCount += 1;
                    await _taskRepository.UpdateAsync(t);
                }
            }

            return question.Id;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Guid id, CreateQuestionDto dto)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                throw new Exception("Question not found.");

            if (question.Status != (int)QuestionStatus.Draft &&
                question.Status != (int)QuestionStatus.Rejected)
                throw new Exception("Only Draft or Rejected questions can be edited.");

            //ValidateQuestion(dto);

            question.QuestionText = dto.QuestionText;
            question.Marks = dto.Marks;
            question.CorrectAnswer = dto.CorrectAnswer;
            question.CorrectAnswerText = dto.CorrectAnswerText;
            question.Reference = FormatReference(dto.Reference);
            question.QuestionType = dto.QuestionTypeId;

            // Replace options only if MCQ
            if (dto.QuestionTypeId == 0)
            {
                if (dto.Options == null || dto.Options.Count != 4)
                    throw new Exception("MCQ questions must have exactly 4 options.");

                // Clear existing and set new
                question.Options.Clear();
                foreach (var o in dto.Options)
                {
                    question.Options.Add(new QuestionOption
                    {
                        Id = Guid.NewGuid(),
                        OptionLabel = o.OptionLabel?.Trim().ToUpper() ?? string.Empty,
                        OptionText = o.OptionText ?? string.Empty,
                        IsCorrect = string.Equals(o.OptionLabel?.Trim(), dto.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase)
                    });
                }
            }
            else
            {
                // Remove options for non-MCQ
                question.Options.Clear();
            }

            await _question_repository_update_async_wrapper(question);
        }

        // Helper to call repository update (keeps readability for patch)
        private async Task _question_repository_update_async_wrapper(Question question)
        {
            await _questionRepository.UpdateAsync(question);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                throw new Exception("Question not found.");

            if (question.Status != (int)QuestionStatus.Draft)
                throw new Exception("Only Draft questions can be deleted.");

            await _questionRepository.DeleteAsync(question);
        }

        /// <inheritdoc/>
        public async Task SubmitAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                throw new Exception("Question not found.");

            if (question.Status != (int)QuestionStatus.Draft)
                throw new Exception("Only Draft questions can be submitted.");

            question.Status = (int)QuestionStatus.Submitted;

            await _questionRepository.UpdateAsync(question);

            // Update associated task counters
            var task = await _taskRepository.GetByIdAsync(question.TaskId);
            if (task != null)
            {
                if (task.CompletedCount == 0)
                {
                    task.Status = (int)TaskStatus.InProgress;
                }

                task.CompletedCount += 1;
                await _taskRepository.UpdateAsync(task);
            }
        }

        /// <inheritdoc/>
        public async Task<QuestionResponseDto> GetByIdAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                throw new Exception("Question not found.");

            return new QuestionResponseDto
            {
                Id = question.Id,
                TaskId = question.TaskId,
                QuestionText = question.QuestionText,
                Marks = question.Marks,
                CorrectAnswer = question.CorrectAnswer,
                Reference = question.Reference,
                Status = ((QuestionStatus)question.Status).ToString(),
                CreatedAt = question.CreatedAt,

                Options = question.Options.Select(o => new QuestionOptionDto
                {
                    OptionLabel = o.OptionLabel,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect,
                }).ToList(),

                ReviewHistory = question.Reviews.Select(r => new ReviewResponseDto
                {
                    ReviewerRole = r.ReviewerRole,
                    Feedback = r.Feedback,
                    ReviewedAt = r.ReviewedAt
                }).ToList()
            };
        }

        /// <inheritdoc/>
        public async Task<Winfocus.LMS.Application.DTOs.Stats.QuestionStatsDto> GetQuestionStatsAsync(Guid operatorId)
        {
            return await _question_repository_stats_wrapper(operatorId);
        }

        private async Task<Winfocus.LMS.Application.DTOs.Stats.QuestionStatsDto> _question_repository_stats_wrapper(Guid operatorId)
        {
            return await _questionRepository.GetStatsForOperatorAsync(operatorId);
        }

        /// <inheritdoc/>
        public async Task<List<QuestionListDto>> GetByTaskIdAsync(Guid taskId, int page, int pageSize)
        {
            var questions = await _questionRepository.GetByTaskIdAsync(taskId, page, pageSize);

            return questions.Select(q => new QuestionListDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Status = ((QuestionStatus)q.Status).ToString()
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<QuestionPreviewDto> PreviewAsync(Guid id)
        {
            var q = await _questionRepository.GetByIdAsync(id);

            if (q == null)
                throw new Exception("Question not found.");

            return new QuestionPreviewDto
            {
                QuestionText = q.QuestionText,
                CorrectAnswer = q.CorrectAnswer,
                Options = q.Options.Select(o => new QuestionOptionDto
                {
                    OptionLabel = o.OptionLabel,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };
        }

        /// <summary>
        /// Gets filtered QuestionBankItem.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="subject">The paged subject.</param>
        /// <param name="chapter">The paged chapter.</param>
        /// <param name="status">The paged status.</param>
        /// <param name="questionType">The paged questionType.</param>
        /// <returns>Paginated QuestionBankItemDto result.</returns>
        public async Task<CommonResponse<PagedResult<QuestionBankItemDto>>> GetFilteredAsync(
            PagedRequest request,
            string? subject,
            string? chapter,
            int? status,
            int? questionType)
        {
            try
            {

                var query = _questionRepository.Query();

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
                        x.QuestionText.ToLower().Contains(searchTerm) ||
                        x.QuestionType.ToString().ToLower().Contains(searchTerm) ||
                        (x.TaskAssignment != null && x.TaskAssignment.TaskCode != null && x.TaskAssignment.TaskCode.ToLower().Contains(searchTerm)) ||
                        (x.TaskAssignment != null && x.TaskAssignment.Subject != null && x.TaskAssignment.Subject.Name.ToLower().Contains(searchTerm)) ||
                        (x.TaskAssignment != null && x.TaskAssignment.Chapter != null && x.TaskAssignment.Chapter.Name.ToLower().Contains(searchTerm)));
                }

                // ── Additional Filters (subject, chapter, status, question type) ──
                if (!string.IsNullOrWhiteSpace(subject))
                {
                    var subjectTerm = subject.Trim().ToLower();
                    query = query.Where(x => x.TaskAssignment != null && x.TaskAssignment.Subject != null && x.TaskAssignment.Subject.Name.ToLower().Contains(subjectTerm));
                }

                if (!string.IsNullOrWhiteSpace(chapter))
                {
                    var chapterTerm = chapter.Trim().ToLower();
                    query = query.Where(x => x.TaskAssignment != null && x.TaskAssignment.Chapter != null && x.TaskAssignment.Chapter.Name.ToLower().Contains(chapterTerm));
                }

                if (status.HasValue)
                    query = query.Where(x => x.Status == status.Value);

                if (questionType.HasValue)
                    query = query.Where(x => x.QuestionType == questionType.Value);

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<QuestionBankItemDto>>.SuccessResponse(
                        "No Question bank found.",
                        new PagedResult<QuestionBankItemDto>(
                            new List<QuestionBankItemDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "questiontext" => isDesc ? query.OrderByDescending(x => x.QuestionText)
                                             : query.OrderBy(x => x.QuestionText),
                    "questiontype" => isDesc ? query.OrderByDescending(x => x.QuestionType)
                                             : query.OrderBy(x => x.QuestionType),
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

                var dtoList = subjects.Select((c, idx) =>
                {
                    var dto = Map(c);
                    dto.QuestionNumber = request.Offset + idx + 1;
                    return dto;
                }).ToList();

                return CommonResponse<PagedResult<QuestionBankItemDto>>.SuccessResponse(
                    "QuestionBankItemDto data fetched successfully.",
                    new PagedResult<QuestionBankItemDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                return CommonResponse<PagedResult<QuestionBankItemDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats reference string into standard format.
        /// Example: subject-Unit-Chapter.
        /// </summary>
        private string? FormatReference(string? reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return null;

            return reference.Trim();
        }

        private static QuestionBankItemDto Map(Question c) =>
    new QuestionBankItemDto
    {
       Id = c.Id,
       QuestionType = c.QuestionType == 0 ? "MCQ" : "LongAnswer",
       QuestionText = c.QuestionText,
       Subject = c.TaskAssignment?.Subject?.Name,
       Chapter = c.TaskAssignment?.Chapter?.Name,
       Marks = c.Marks,
       Status = ((QuestionStatus)c.Status).ToString(),
       TaskCode = c.TaskAssignment?.TaskCode,
       CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd"),
    };
    }
}
