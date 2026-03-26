using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Batch"/> entities.
    /// </summary>
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _repository;
        private readonly ILogger<BatchService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchService(IBatchRepository repository, ILogger<BatchService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchDto.</returns>
        public async Task<CommonResponse<List<BatchDto>>> GetAllAsync(Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching all Batches");
                var batch = await _repository.GetAllAsync(centerId);
                _logger.LogInformation("Fetched {Count} Batches", batch.Count());
                var data = batch.Select(Map).ToList();
                if (data.Any())
                {
                    return CommonResponse<List<BatchDto>>.SuccessResponse("Fetching all Batches", data);
                }
                else
                {
                    return CommonResponse<List<BatchDto>>.FailureResponse("no batches found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batches.");
                return CommonResponse<List<BatchDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching batch by Id: {Id}", id);
                var batch = await _repository.GetByIdAsync(id);
                _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
                var mappeddata = batch == null ? null : Map(batch);
                if (mappeddata != null)
                {
                    return CommonResponse<BatchDto>.SuccessResponse("fetching batch by id", mappeddata);
                }
                else
                {
                    return CommonResponse<BatchDto>.FailureResponse("batch not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batches.");
                return CommonResponse<BatchDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchDto>> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching batch by Id: {Id}", id);
                var batch = await _repository.GetByIdCenterIdAsync(id, centerId);
                _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
                var mappeddata = batch == null ? null : Map(batch);
                if (mappeddata != null)
                {
                    return CommonResponse<BatchDto>.SuccessResponse("fetching batch by id", mappeddata);
                }
                else
                {
                    return CommonResponse<BatchDto>.FailureResponse("batch not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batches.");
                return CommonResponse<BatchDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        public async Task<CommonResponse<BatchDto>> CreateAsync(BatchRequest request)
        {
            try
            {
                var batchtiming = new Batch
                {
                    Name = request.name,
                    SubjectId = request.subjectId,
                    CreatedBy = request.userId,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(batchtiming);
                _logger.LogInformation(
               "Batch created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<BatchDto>.SuccessResponse(
                  "batch created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating batch: {Name}", request.name);
                return CommonResponse<BatchDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Batch not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<BatchDto>> UpdateAsync(Guid id, BatchRequest request)
        {
            try
            {
                _logger.LogInformation("Updating batch Id: {Id}", id);

                var batch = await _repository.GetByIdAsync(id);
                if (batch == null)
                {
                    return CommonResponse<BatchDto>.FailureResponse("batch not found");
                }

                batch.Name = request.name;
                batch.SubjectId = request.subjectId;
                batch.UpdatedAt = DateTime.UtcNow;
                batch.UpdatedBy = request.userId;

                var updated = await _repository.UpdateAsync(batch);

                _logger.LogInformation("batch updated Id: {Id}", id);
                return CommonResponse<BatchDto>.SuccessResponse(
                    "batch updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating batch Id: {Id}", id);
                return CommonResponse<BatchDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId)
        {
            try
            {
                _logger.LogInformation("Deleting batch Id: {Id}", id);
                var result = await _repository.DeleteAsync(id, centerId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Batch deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Batch not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Batch Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered batches with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated batches result.</returns>
        public async Task<CommonResponse<PagedResult<BatchDto>>> GetFilteredAsync(
            PagedRequest request, Guid centerId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered batches. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query(centerId);

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
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<BatchDto>>.SuccessResponse(
                        "No batches found.",
                        new PagedResult<BatchDto>(
                            new List<BatchDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "subjectname" => isDesc ? query.OrderByDescending(x => x.Subject.Name)
                                             : query.OrderBy(x => x.Subject.Name),

                    "coursename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Name)
                                             : query.OrderBy(x => x.Subject.Course.Name),

                    "streamname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Syllabus.Name),

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
                    "Returning {Count} of {Total} batches",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<BatchDto>>.SuccessResponse(
                    "batches fetched successfully.",
                    new PagedResult<BatchDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered batches.");
                return CommonResponse<PagedResult<BatchDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static BatchDto Map(Batch c) =>
           new BatchDto
           {
               Id = c.Id,
               Name = c.Name,
               SubjectId = c.SubjectId,
               IsActive = c.IsActive,
               Subject = c.Subject == null ? null : new SubjectDto
               {
                   Id = c.Subject.Id,
                   Name = c.Subject.Name,
                   CourseId = c.Subject.CourseId,
                   Course = c.Subject.Course == null ? null : new CourseDto
                   {
                       Id = c.Subject.Course.Id,
                       Name = c.Subject.Course.Name,
                       IsActive = c.Subject.Course.IsActive,
                       StreamId = c.Subject.Course.StreamId,
                       Stream = c.Subject.Course.Stream == null ? null : new StreamDto
                       {
                           Id = c.Subject.Course.Stream.Id,
                           Name = c.Subject.Course.Stream.Name,
                           IsActive = c.Subject.Course.Stream.IsActive,
                           GradeId = c.Subject.Course.Stream.GradeId,
                           Grade = c.Subject.Course.Stream.Grade == null ? null : new GradeDto
                           {
                               Id = c.Subject.Course.Stream.Grade.Id,
                               Name = c.Subject.Course.Stream.Grade.Name,
                               IsActive = c.Subject.Course.Stream.Grade.IsActive,
                               SyllabusId = c.Subject.Course.Stream.Grade.SyllabusId,
                               Syllabus = c.Subject.Course.Stream.Grade.Syllabus == null ? null : new SyllabusDto
                               {
                                   Id = c.Subject.Course.Stream.Grade.Syllabus.Id,
                                   Name = c.Subject.Course.Stream.Grade.Syllabus.Name,
                                   IsActive = c.Subject.Course.Stream.Grade.Syllabus.IsActive
                               }
                           }
                       }
                   }
               }
           };
    }
}
