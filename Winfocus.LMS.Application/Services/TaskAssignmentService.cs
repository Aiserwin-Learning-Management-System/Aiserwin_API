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
    /// TaskAssignmentService.
    /// </summary>
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _repository;
        private readonly ILogger<TaskAssignmentService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAssignmentService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public TaskAssignmentService(
            ITaskAssignmentRepository repository,
            ILogger<TaskAssignmentService> logger)
        {
            _repository = repository;
            _logger = logger;
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
                var taskassignment = new TaskAssignment
                {
                    OperatorId = request.OperatorId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.Createdby,
                    AssignedBy = request.AssignedBy,
                    QuestionType = request.QuestionType,
                    Year = request.Year,
                    ChapterId = request.ChapterId,
                    TotalQuestions = request.TotalQuestions,
                    CompletedCount = request.CompletedCount,
                    Deadline = request.Deadline,
                    Priority = request.Priority,
                    Instructions = request.Instructions,
                    Status = request.Status,
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
        public async Task<CommonResponse<TaskResponseDto>> GetByOperatorIdAsync(Guid operatorid)
        {
            try
            {
                _logger.LogInformation("Fetching task by operatorId: {operatorId}", operatorid);
                var tasks = await _repository.GetByOperatorIdAsync(operatorid);
                _logger.LogInformation("tasks fetched successfully for operatorId: {operatorId}", operatorid);
                var mappeddata = tasks == null ? null : Map(tasks);
                if (mappeddata != null)
                {
                    return CommonResponse<TaskResponseDto>.SuccessResponse("fetched tasks for this operator id", mappeddata);
                }
                else
                {
                    return CommonResponse<TaskResponseDto>.FailureResponse("tasks not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks by Id: {Id}", operatorid);
                return CommonResponse<TaskResponseDto>.FailureResponse($"An error occurred: {ex.Message}");
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
