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
    /// ContentResourceTypeService.
    /// </summary>
    public class ContentResourceTypeService : IContentResourceTypeService
    {
        private readonly IContentResourceTypeRepository _repository;
        private readonly ILogger<ContentResourceTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceTypeService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public ContentResourceTypeService(IContentResourceTypeRepository repository, ILogger<ContentResourceTypeService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ContentResourceTypeDto.</returns>
        public async Task<CommonResponse<List<ContentResourceTypeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all ContentResource Type");
            var contentResourceType = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} ContentResourceType", contentResourceType.Count());
            var mappeddata = contentResourceType.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<ContentResourceTypeDto>>.SuccessResponse("Fetching all ContentResourceType", mappeddata);
            }
            else
            {
                return CommonResponse<List<ContentResourceTypeDto>>.FailureResponse("no ContentResourceType");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        public async Task<CommonResponse<ContentResourceTypeDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching ContentResourceType details by Id: {Id}", id);
            var content = await _repository.GetByIdAsync(id);
            _logger.LogInformation("ContentResourceType details fetched successfully for Id: {Id}", id);
            var mappeddata = content == null ? null : Map(content);
            if (mappeddata != null)
            {
                return CommonResponse<ContentResourceTypeDto>.SuccessResponse("ContentResourceType details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<ContentResourceTypeDto>.FailureResponse("ContentResourceType details not found");
            }
        }


        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        public async Task<CommonResponse<ContentResourceTypeDto>> CreateAsync(ContentResourceTypeDto request)
        {
            try
            {
                bool alreadyexist = await _repository.ExistsByNameAsync(request.Name);
                if (alreadyexist)
                {
                    return CommonResponse<ContentResourceTypeDto>.FailureResponse("ContentResourceType Already exist.");
                }

                var content = new ContentResourceType
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(content);
                _logger.LogInformation(
               "ContentResourceType created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<ContentResourceTypeDto>.SuccessResponse(
                  "ContentResourceType created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ContentResourceType : {Name}", request.Name);
                return CommonResponse<ContentResourceTypeDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">ContentResourceType not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<ContentResourceTypeDto>> UpdateAsync(Guid id, ContentResourceTypeDto request)
        {
            try
            {
                _logger.LogInformation("Updating ContentResourceType: {Id}", id);

                var content = await _repository.GetByIdAsync(id);
                if (content == null)
                {
                    return CommonResponse<ContentResourceTypeDto>.FailureResponse("ContentResourceType not found");
                }

                content.Name = request.Name;
                content.Description = request.Description;
                content.UpdatedAt = DateTime.UtcNow;
                content.UpdatedBy = request.UpdatedBy;

                var updated = await _repository.UpdateAsync(content);

                _logger.LogInformation("ContentResourceType updated Id: {Id}", id);
                return CommonResponse<ContentResourceTypeDto>.SuccessResponse(
                    "ContentResourceType updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ContentResourceType Id: {Id}", id);
                return CommonResponse<ContentResourceTypeDto>.FailureResponse(
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
            _logger.LogInformation("Deleting ContentResourceType Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("ContentResourceType deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("ContentResourceType deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets ContentResourceType with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated ContentResourceType result.</returns>
        public async Task<CommonResponse<PagedResult<ContentResourceTypeDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered ContentResourceType. Filters => Active:{Active}, " +
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
                        x.Name.ToString().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<ContentResourceTypeDto>>.SuccessResponse(
                        "No ContentResourceType found.",
                        new PagedResult<ContentResourceTypeDto>(
                            new List<ContentResourceTypeDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "description" => isDesc ? query.OrderByDescending(x => x.Description)
                                             : query.OrderBy(x => x.Description),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var content = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = content.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} ContentResourceType",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<ContentResourceTypeDto>>.SuccessResponse(
                    "ContentResourceType data fetched successfully.",
                    new PagedResult<ContentResourceTypeDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ContentResourceType.");
                return CommonResponse<PagedResult<ContentResourceTypeDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="chapterid">The identifier.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        public async Task<CommonResponse<List<ContentResourceTypeDto>>> GetByChapterIdAsync(Guid chapterid)
        {
            _logger.LogInformation("Fetching content resource type by chapter Id: {Id}", chapterid);
            var content = await _repository.GetByChapterIdAsync(chapterid);
            _logger.LogInformation("content resource type by chapter fetched successfully for Id: {Id}", chapterid);
            var mappeddata = content == null ? null : Map(content);
            if (mappeddata != null)
            {
                return CommonResponse<List<ContentResourceTypeDto>>.SuccessResponse("content resource type by chapter details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<List<ContentResourceTypeDto>>.FailureResponse("content resource type by chapter details not found");
            }
        }

        private static List<ContentResourceTypeDto> Map(IEnumerable<ContentResourceType> content)
        {
            return content.Select(Map).ToList();
        }

        private static ContentResourceTypeDto Map(ContentResourceType c) =>
   new ContentResourceTypeDto
   {
       Id = c.Id,
       Name = c.Name,
       Description = c.Description,
       IsActive = c.IsActive,
       Chapter = c.Chapter == null ? null : new ExamChapterDto
       {
           Id = c.Chapter.Id,
           Name = c.Chapter.Name,
           Description = c.Chapter.Description,
           UnitId = c.Chapter.UnitId,
           ChapterNumber = c.Chapter.ChapterNumber,
           IsActive = c.Chapter.IsActive,
           UnitName = c.Chapter.Unit.Name,
           SubjectId = c.Chapter.Unit.SubjectId,
           SubjectName = c.Chapter.Unit.Subject.Name,
           GradeId = c.Chapter.Unit.Subject.GradeId,
           GradeName = c.Chapter.Unit.Subject.Grade.Name,
           SyllabusId = c.Chapter.Unit.Subject.Grade.SyllabusId,
           SyllabusName = c.Chapter.Unit.Subject.Grade.Syllabus.Name,
           AcademicYearId = c.Chapter.Unit.Subject.Grade.Syllabus.AcademicYearId,
           AcademicYearName = c.Chapter.Unit.Subject.Grade.Syllabus.AcademicYear.Name,
       },
   };
    }
}
