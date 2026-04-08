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
    /// Provides business operations for <see cref="BatchTimingSunday"/> entities.
    /// </summary>
    public class BatchTimingSundayService : IBatchTimingSundayService
    {
        private readonly IBatchTimingSundayRepository _repository;
        private readonly ILogger<BatchTimingSundayService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTimingSundayService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public BatchTimingSundayService(IBatchTimingSundayRepository repository, ILogger<BatchTimingSundayService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingSundayDto.</returns>
        public async Task<CommonResponse<List<BatchTimingSundayDto>>> GetAllAsync(Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching all Batches");
                var batchtiming = await _repository.GetAllAsync(centerId);
                _logger.LogInformation("Fetched {Count} Batches", batchtiming.Count());
                var mapped = batchtiming.Select(Map).ToList();
                if (mapped.Any())
                {
                    return CommonResponse<List<BatchTimingSundayDto>>.SuccessResponse("batch timing sunday", mapped);
                }
                else
                {
                    return CommonResponse<List<BatchTimingSundayDto>>.FailureResponse("no batch timing found");
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching batch timing.");
                return CommonResponse<List<BatchTimingSundayDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchTimingSundayDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching batchtiming by Id: {Id}", id);
                var batchtiming = await _repository.GetByIdAsync(id);
                _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
                var mappeddata = batchtiming == null ? null : Map(batchtiming);
                if (mappeddata != null)
                {
                    return CommonResponse<BatchTimingSundayDto>.SuccessResponse("batch timing sunday", mappeddata);
                }
                else
                {
                    return CommonResponse<BatchTimingSundayDto>.FailureResponse("batch timing not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batch timing.");
                return CommonResponse<BatchTimingSundayDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        public async Task<CommonResponse<BatchTimingSundayDto>> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching batchtiming by Id: {Id}", id);
                var batchtiming = await _repository.GetByIdCenterIdAsync(id,centerId);
                _logger.LogInformation("batch fetched successfully for Id: {Id}", id);
                var mappeddata = batchtiming == null ? null : Map(batchtiming);
                if (mappeddata != null)
                {
                    return CommonResponse<BatchTimingSundayDto>.SuccessResponse("batch timing sunday", mappeddata);
                }
                else
                {
                    return CommonResponse<BatchTimingSundayDto>.FailureResponse("batch timing not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batch timing.");
                return CommonResponse<BatchTimingSundayDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchTimingMTFDto.</returns>
        /// <exception cref="InvalidOperationException">batch code already exists. </exception>
        public async Task<CommonResponse<BatchTimingSundayDto>> CreateAsync(BatchTimingRequest request)
        {
            try
            {
                var batchtiming = new BatchTimingSunday
                {
                    BatchTime = request.batchTime,
                    SubjectId = request.subjectId,
                    CreatedBy = request.userId,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(batchtiming);
                _logger.LogInformation(
              "Batch timing created successfully. Id: {Id}",
              created.Id);
                return CommonResponse<BatchTimingSundayDto>.SuccessResponse(
                  "batch timing created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating batch timing: {Batchtime}", request.batchTime);
                return CommonResponse<BatchTimingSundayDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Batch Timing not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<BatchTimingSundayDto>> UpdateAsync(Guid id, BatchTimingRequest request)
        {
            try
            {
                _logger.LogInformation("Updating batch time Id: {Id}", id);

                var batch = await _repository.GetByIdAsync(id);
                if (batch == null)
                {
                    return CommonResponse<BatchTimingSundayDto>.FailureResponse("batch time not found");
                }

                batch.BatchTime = request.batchTime;
                batch.SubjectId = request.subjectId;
                batch.UpdatedAt = DateTime.UtcNow;
                batch.UpdatedBy = request.userId;

                var updated = await _repository.UpdateAsync(batch);

                _logger.LogInformation("batch updated Id: {Id}", id);
                return CommonResponse<BatchTimingSundayDto>.SuccessResponse(
                    "batch time updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating batch time Id: {Id}", id);
                return CommonResponse<BatchTimingSundayDto>.FailureResponse(
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
                _logger.LogInformation("Deleting batch time Id: {Id}", id);
                var result = await _repository.DeleteAsync(id, centerId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Batch time deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Batch time not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Batch time Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTimingSundayDto.</returns>
        public async Task<CommonResponse<List<BatchTimingSundayDto>>> GetBySubjectIdAsync(Guid subjectid)
        {
            try
            {
                _logger.LogInformation("Fetching all Batch timing by subject");
                var batchtiming = await _repository.GetBySubjectIdAsync(subjectid);
                _logger.LogInformation("Fetched {Count} Batch timing", batchtiming.Count());
                var mapped = batchtiming.Select(Map).ToList();
                if (mapped.Any())
                {
                    return CommonResponse<List<BatchTimingSundayDto>>.SuccessResponse("batch timing sunday by subject", mapped);
                }
                else
                {
                    return CommonResponse<List<BatchTimingSundayDto>>.FailureResponse("no batch timing found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batch timing.");
                return CommonResponse<List<BatchTimingSundayDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>.</returns>
        public async Task BatchTimingSubjectCreate(SubjectBatchTimingRequest request)
        {
            var batchtiming = new SubjectBatchTimingSunday
            {
                SubjectId = request.subjectId,
                SubjectBatchTimingSundays = request.Batchtimingids
                    .Distinct()
                    .Select(id => new SubjectBatchTimingSunday { SubjectId = id })
                    .ToList(),
            };

            await _repository.BatchTimingSubjectCreate(batchtiming);
            _logger.LogInformation(
           "Batch Timing for monaday to friday for subject created successfully.");
        }

        /// <summary>
        /// Gets filtered batch timing for sunday with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerid.</param>
        /// <returns>Paginated batch timing for sunday result.</returns>
        public async Task<CommonResponse<PagedResult<BatchTimingSundayDto>>> GetFilteredAsync(
            PagedRequest request, Guid centerId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered batch timings. Filters => Active:{Active}, " +
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
                        x.BatchTime.ToString().Contains(searchTerm) ||
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
                    return CommonResponse<PagedResult<BatchTimingSundayDto>>.SuccessResponse(
                        "No batch timings found.",
                        new PagedResult<BatchTimingSundayDto>(
                            new List<BatchTimingSundayDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.BatchTime)
                                             : query.OrderBy(x => x.BatchTime),
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
                    "Returning {Count} of {Total} batch timing for sunday",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<BatchTimingSundayDto>>.SuccessResponse(
                    "batch timings fetched successfully.",
                    new PagedResult<BatchTimingSundayDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered batch timings.");
                return CommonResponse<PagedResult<BatchTimingSundayDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<BatchTimingSundayDto> Map(IEnumerable<BatchTimingSunday> batchTimingMTFs)
        {
            return batchTimingMTFs.Select(Map).ToList();
        }

        private static BatchTimingSundayDto Map(BatchTimingSunday c) =>
   new BatchTimingSundayDto
   {
       Id = c.Id,
       BatchTime = DateTime.SpecifyKind(c.BatchTime, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'"),
       BatchTimeDisplay = DateTime.SpecifyKind(c.BatchTime, DateTimeKind.Utc).ToString("hh:mm tt"),
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
