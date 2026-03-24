using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Dashboard;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.QuestionConfig;
using Winfocus.LMS.Application.DTOs.Task;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using TaskStatus = Winfocus.LMS.Domain.Enums.TaskStatus;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// TaskAssignmentService.
    /// </summary>
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _repository;
        private readonly ILogger<TaskAssignmentService> _logger;
        private readonly IOperatorDashboardService _operatorDashboardService;
        private readonly IExamSyllabusRepository _syllabusRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IExamGradeRepository _gradeRepository;
        private readonly IExamSubjectRepository _subjectRepository;
        private readonly IExamUnitRepository _unitRepository;
        private readonly IExamChapterRepository _chapterRepository;
        private readonly IContentResourceTypeRepository _resourceTypeRepository;
        private readonly IQuestionTypeConfigRepository _questionTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAssignmentService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="operatorDashboardService">The operatorDashboardService.</param>
        /// <param name="syllabusRepository">The syllabus repository.</param>
        /// <param name="academicYearRepository">The academic year repository.</param>
        /// <param name="gradeRepository">The grade repository.</param>
        /// <param name="subjectRepository">The subject repository.</param>
        /// <param name="unitRepository">The unit repository.</param>
        /// <param name="chapterRepository">The chapter repository.</param>
        /// <param name="resourceTypeRepository">The resource type repository.</param>
        /// <param name="questionTypeRepository">The question type repository.</param>
        public TaskAssignmentService(
            ITaskAssignmentRepository repository,
            ILogger<TaskAssignmentService> logger,
            IOperatorDashboardService operatorDashboardService,
            IExamSyllabusRepository syllabusRepository,
            IAcademicYearRepository academicYearRepository,
            IExamGradeRepository gradeRepository,
            IExamSubjectRepository subjectRepository,
            IExamUnitRepository unitRepository,
            IExamChapterRepository chapterRepository,
            IContentResourceTypeRepository resourceTypeRepository,
            IQuestionTypeConfigRepository questionTypeRepository)
        {
            _repository = repository;
            _logger = logger;
            _operatorDashboardService = operatorDashboardService;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TaskResponseDto.</returns>
        public async Task<CommonResponse<List<TaskResponseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all task assignments");
                var syllabuses = await _repository.GetAllAsync();
                var mappeddata = syllabuses.Select(Map).ToList();
                if (mappeddata.Any())
                {
                    return CommonResponse<List<TaskResponseDto>>.SuccessResponse("Fetched all tasks", mappeddata);
                }
                else
                {
                    return CommonResponse<List<TaskResponseDto>>.FailureResponse("tasks not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks");
                return CommonResponse<List<TaskResponseDto>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskResponseDto.</returns>
        public async Task<CommonResponse<TaskResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching task by Id: {Id}", id);
                var tasks = await _repository.GetByIdAsync(id);
                _logger.LogInformation("tasks fetched successfully for Id: {Id}", id);
                var mappeddata = tasks == null ? null : Map(tasks);
                if (mappeddata != null)
                {
                    return CommonResponse<TaskResponseDto>.SuccessResponse("fetched tasks for this id", mappeddata);
                }
                else
                {
                    return CommonResponse<TaskResponseDto>.FailureResponse("tasks not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks by Id: {Id}", id);
                return CommonResponse<TaskResponseDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TaskResponseDto.</returns>
        public async Task<CommonResponse<TaskResponseDto>> CreateAsync(CreateTaskDto request)
        {
            try
            {
                var res = await _operatorDashboardService.GetProfileAsync(request.OperatorId);
                if (!res.Success)
                {
                    return CommonResponse<TaskResponseDto>.FailureResponse("Invalid Operator!");
                }

                //_logger.LogInformation("Creating Task Asiignments with code: {Code}", request.TaskCode);

                //bool codeExists = await _repository.CodeExistsAsync(request.TaskCode);
                //if (codeExists)
                //{
                //    return CommonResponse<TaskResponseDto>.FailureResponse(
                //        $"Task Code '{request.TaskCode}' already exists");
                //}
                var codereq = new TaskCodeRequest
                {
                    OperatorId = request.OperatorId,
                    SyllabusId = request.SyllabusId,
                    AcademicYearId = request.AcademicYearId,
                    GradeId = request.GradeId,
                    SubjectId = request.SubjectId,
                    UnitId = request.UnitId,
                    ChapterId = request.ChapterId,
                    ContentresourceTypeId = request.ResourceTypeId,
                    QuestionTypeId = request.QuestionTypeId,
                };

                CommonResponse<SuggestedCodeResponseDto> taskcodeobj = await TaskCodeAsync(codereq);
                string taskcode = taskcodeobj.Data.SuggestedCode + taskcodeobj.Data.NextSequence;

                var taskassignment = new TaskAssignment
                {
                    OperatorId = request.OperatorId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.Createdby,
                    AssignedBy = request.AssignedBy,
                    QuestionTypeId = request.QuestionTypeId,
                    Year = request.Year,
                    ChapterId = request.ChapterId,
                    TotalQuestions = request.TotalQuestions,
                    CompletedCount = request.CompletedCount,
                    Deadline = request.Deadline,
                    Priority = request.Priority,
                    Instructions = request.Instructions,
                    Status = (int)TaskStatus.Pending,
                    SequenceNumber = taskcodeobj.Data.NextSequence,
                };

                var created = await _repository.AddAsync(taskassignment);
                return CommonResponse<TaskResponseDto>.SuccessResponse(
                 "task created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tasks");
                return CommonResponse<TaskResponseDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">task not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<TaskResponseDto>> UpdateAsync(Guid id, CreateTaskDto request)
        {
            try
            {
                _logger.LogInformation("Updating task Id: {Id}", id);

                var task = await _repository.GetByIdAsync(id);
                if (task == null)
                {
                    return CommonResponse<TaskResponseDto>.FailureResponse("task not found");
                }

                task.OperatorId = request.OperatorId;
                task.UpdatedAt = DateTime.UtcNow;
                task.UpdatedBy = request.Createdby;
                task.AssignedBy = request.AssignedBy;
                task.QuestionType = request.QuestionType;
                task.Status = request.Status;
                task.Year = request.Year;
                task.ChapterId = request.ChapterId;
                task.TotalQuestions = request.TotalQuestions;
                task.CompletedCount = request.CompletedCount;
                task.Deadline = request.Deadline;
                task.Priority = request.Priority;

                var updated = await _repository.UpdateAsync(task);

                _logger.LogInformation("task updated Id: {Id}", id);
                return CommonResponse<TaskResponseDto>.SuccessResponse(
                    "task updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task Id: {Id}", id);
                return CommonResponse<TaskResponseDto>.FailureResponse(
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
                _logger.LogInformation("Deleting task Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "task deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "task not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="operatorid">The identifier.</param>
        /// <returns>TaskResponseDto.</returns>
        public async Task<CommonResponse<List<TaskResponseDto>>> GetByOperatorIdAsync(Guid operatorid)
        {
            try
            {
                _logger.LogInformation("Fetching task by operatorId: {operatorId}", operatorid);
                var tasks = await _repository.GetByOperatorIdAsync(operatorid);
                _logger.LogInformation("tasks fetched successfully for operatorId: {operatorId}", operatorid);
                var mappeddata = tasks == null ? null : MapList(tasks);
                if (mappeddata != null)
                {
                    return CommonResponse<List<TaskResponseDto>>.SuccessResponse("fetched tasks for this operator id", mappeddata);
                }
                else
                {
                    return CommonResponse<List<TaskResponseDto>>.FailureResponse("tasks not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks by Id: {Id}", operatorid);
                return CommonResponse<List<TaskResponseDto>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves task based on filter criteria with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated task result.</returns>
        public async Task<CommonResponse<PagedResult<TaskResponseDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered tasks. Filters => Active:{Active}, Search:{SearchText}, " +
                    "SortBy:{SortBy}, SortOrder:{SortOrder}, Limit:{Limit}, Offset:{Offset}",
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

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<TaskResponseDto>>.SuccessResponse(
                        "No task found.",
                        new PagedResult<TaskResponseDto>(
                            new List<TaskResponseDto>(), 0, request.Limit, request.Offset));
                }

                // ── Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    //"name" => isDesc ? query.OrderByDescending(x => x.Name)
                    //                      : query.OrderBy(x => x.Name),
                    //"centername" => isDesc ? query.OrderByDescending(x => x.Center.Name)
                    //                      : query.OrderBy(x => x.Center.Name),
                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                          : query.OrderBy(x => x.IsActive),
                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                          : query.OrderBy(x => x.CreatedAt),
                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                          : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var tasks = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = tasks.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} tasks",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<TaskResponseDto>>.SuccessResponse(
                    "Tasks fetched successfully.",
                    new PagedResult<TaskResponseDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered tasks.");
                return CommonResponse<PagedResult<TaskResponseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<TaskOverviewDto> GetOverviewAsync()
        {
            var tasks = await _repository.GetAllForOverviewAsync();

            int totalTasks = 0;
            int activeTasks = 0;
            int completedTasks = 0;
            int overdueTasks = 0;
            int totalQuestionsAssigned = 0;
            int totalQuestionsCompleted = 0;

            var now = DateTime.UtcNow;

            var operatorDict = new Dictionary<Guid, OperatorStatus>();

            foreach (var task in tasks)
            {
                totalTasks++;

                // Status checks
                var status = (TaskStatus)task.Status;
                if (status == TaskStatus.Completed)
                {
                    completedTasks++;
                }
                else
                {
                    activeTasks++;
                }

                if (task.Deadline < now && status != TaskStatus.Completed)
                {
                    overdueTasks++;
                }

                // Question aggregation
                totalQuestionsAssigned += task.TotalQuestions;
                totalQuestionsCompleted += task.CompletedCount;

                // Operator aggregation
                if (!operatorDict.TryGetValue(task.OperatorId, out var opStat))
                {
                    opStat = new OperatorStatus
                    {
                        OperatorId = task.OperatorId,
                        OperatorName = task.Operator?.StaffCategory.Name ?? string.Empty,
                        ActiveTasks = 0,
                        CompletedQuestions = 0,
                        TotalAssigned = 0,
                    };

                    operatorDict[task.OperatorId] = opStat;
                }

                if (status != TaskStatus.Completed)
                {
                    opStat.ActiveTasks++;
                }

                opStat.CompletedQuestions += task.CompletedCount;
                opStat.TotalAssigned += task.TotalQuestions;
            }

            foreach (var op in operatorDict.Values)
            {
                op.CompletionRate = op.TotalAssigned == 0 ? 0 :
                    (decimal)op.CompletedQuestions / op.TotalAssigned * 100;
            }

            var completionRate = totalQuestionsAssigned == 0 ? 0 :
                (decimal)totalQuestionsCompleted / totalQuestionsAssigned * 100;

            return new TaskOverviewDto
            {
                TotalTasks = totalTasks,
                ActiceTasks = activeTasks,
                CompletedTasks = completedTasks,
                OverdueTasks = overdueTasks,
                TotalQuestionsAssigned = totalQuestionsAssigned,
                totalQuestionsCompleted = totalQuestionsCompleted,
                completionRate = completionRate,
                operatorStatus = operatorDict.Values.ToList(),
            };
        }

        /// <inheritdoc />
        public async Task<CommonResponse<SuggestedCodeResponseDto>> TaskCodeAsync(TaskCodeRequest dto)
        {
            try
            {
                _logger.LogInformation("Generating suggested Task Code");

                ExamSyllabus? syllabus = await _syllabusRepository.GetByIdAsync(dto.SyllabusId, dto.AcademicYearId);
                if (syllabus == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Syllabus not found");
                }

                AcademicYear? academicYear = await _academicYearRepository.GetByIdAsync(dto.AcademicYearId);
                if (academicYear == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Academic Year not found");
                }

                ExamGrade? grade = await _gradeRepository.GetByIdAsync(dto.GradeId, dto.SyllabusId);
                if (grade == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Grade not found");
                }

                ExamSubject? subject = await _subjectRepository.GetByIdAsync(dto.SubjectId, dto.GradeId);
                if (subject == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Subject not found");
                }

                ExamUnit? unit = await _unitRepository.GetByIdAsync(dto.UnitId, dto.SubjectId);
                if (unit == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Unit not found");
                }

                ExamChapter? chapter = await _chapterRepository.GetByIdAsync(dto.ChapterId, dto.UnitId);
                if (chapter == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Chapter not found");
                }

                ContentResourceType? resourceType = await _resourceTypeRepository.GetByIdAsync(dto.ContentresourceTypeId);
                if (resourceType == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Content Resource Type not found");
                }

                QuestionTypeConfig? questionType = await _questionTypeRepository.GetByIdAsync(dto.QuestionTypeId);
                if (questionType == null)
                {
                    return CommonResponse<SuggestedCodeResponseDto>.FailureResponse("Question Type not found");
                }

                int nextSequence = await _repository.GetNextSequenceAsync(
                    dto.SyllabusId, dto.AcademicYearId, dto.GradeId,
                    dto.SubjectId, dto.UnitId, dto.ChapterId, dto.ContentresourceTypeId,dto.QuestionTypeId, dto.OperatorId);

                string code = BuildQuestionCode(
                    syllabus.Name, academicYear.Name, grade.Name,
                    subject.Code ?? subject.Name.Substring(0, Math.Min(3, subject.Name.Length)).ToUpper(),
                    unit.UnitNumber, chapter.ChapterNumber,
                    resourceType.Name, questionType.Name, nextSequence);

                _logger.LogInformation("Suggested code: {Code}", code);

                return CommonResponse<SuggestedCodeResponseDto>.SuccessResponse(
                    "Code suggested successfully",
                    new SuggestedCodeResponseDto
                    {
                        SuggestedCode = code,
                        NextSequence = nextSequence,
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating suggested code");
                return CommonResponse<SuggestedCodeResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Builds the Question Code string from master data values.
        /// Format: [SYL]-[YYYY]-[GRD]-[SUB]-[UNIT]-[CH]-[TYPE]-[SEQ].
        /// </summary>
        /// <param name="syllabusName">The syllabus name.</param>
        /// <param name="academicYearName">The academic year name.</param>
        /// <param name="gradeName">The grade name.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <param name="unitNumber">The unit number.</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <param name="questionTypeCode">The question type code.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <returns>The formatted Question Code string.</returns>
        private static string BuildQuestionCode(
            string syllabusName,
            string academicYearName,
            string gradeName,
            string subjectCode,
            int unitNumber,
            int chapterNumber,
            string resourceTypeCode,
            string questionTypeCode,
            int sequenceNumber)
        {
            string yearPart = academicYearName.Length >= 4
                ? academicYearName.Substring(0, 4)
                : academicYearName;

            string gradePart = new string(gradeName.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(gradePart))
            {
                gradePart = gradeName;
            }

            return $"{syllabusName.ToUpper()}" +
                   $"-{yearPart}" +
                   $"-{gradePart}" +
                   $"-{subjectCode.ToUpper()}" +
                   $"-U{unitNumber:D2}" +
                   $"-CH{chapterNumber:D2}" +
                   $"-{resourceTypeCode.ToUpper()}" +
                   $"-{questionTypeCode.ToUpper()}" +
                   $"-{sequenceNumber:D4}";
        }

        private List<TaskResponseDto> MapList(List<TaskAssignment> tasks)
        {
            return tasks.Select(x => Map(x)).ToList();
        }

        private static TaskResponseDto Map(TaskAssignment c) =>
     new TaskResponseDto
     {
         Id = c.Id,
         OperatorId = c.OperatorId,
         IsActive = c.IsActive,
         AssignedBy = c.AssignedBy,
         QuestionType = c.QuestionType,
         Year = c.Year,
         Chapter = c.Chapter,
         TotalQuestions = c.TotalQuestions,
         CompletedCount = c.CompletedCount,
         Deadline = c.Deadline,
         Priority = c.Priority,
         Instructions = c.Instructions,
         Status = c.Status,
     };
    }
}
