using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="TeachingToolsService"/> entities.
    /// </summary>
    public class TeachingToolsService : ITeachingToolsService
    {
        private readonly ITeachingToolsRepository _repository;
        private readonly ILogger<TeachingToolsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachingToolsService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public TeachingToolsService(ITeachingToolsRepository repository, ILogger<TeachingToolsService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TeachingToolsResponseDto.</returns>
        public async Task<CommonResponse<List<TeachingToolsResponseDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all teaching tools.");
            var tools = await _repository.GetAllAsync();
            var data = tools.Select(Map).ToList();
            return CommonResponse<List<TeachingToolsResponseDto>>.SuccessResponse("Teaching tools", data);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        public async Task<CommonResponse<TeachingToolsResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching teaching tools by Id: {Id}", id);
                var tools = await _repository.GetByIdAsync(id);
                _logger.LogInformation("teaching tools fetched successfully for Id: {Id}", id);
                var mappeddata = tools == null ? null : Map(tools);
                if (mappeddata != null)
                {
                    return CommonResponse<TeachingToolsResponseDto>.SuccessResponse("fetching teaching tools by id", mappeddata);
                }
                else
                {
                    return CommonResponse<TeachingToolsResponseDto>.FailureResponse("teaching tools not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching teaching tools.");
                return CommonResponse<TeachingToolsResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        public async Task<CommonResponse<TeachingToolsResponseDto>> CreateAsync(TeachingToolsDto request)
        {
            try
            {
                var tools = new TeachingTools
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = request.Userid,
                    CreatedAt = DateTime.UtcNow,
                };

                var created = await _repository.AddAsync(tools);
                _logger.LogInformation(
               "teaching tools created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<TeachingToolsResponseDto>.SuccessResponse(
                  "teaching tools created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating academic year: {Name}", request.Name);
                return CommonResponse<TeachingToolsResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">teaching tools not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<TeachingToolsResponseDto>> UpdateAsync(Guid id, TeachingToolsDto request)
        {
            try
            {
                _logger.LogInformation("Updating teaching tools Id: {Id}", id);

                var tools = await _repository.GetByIdAsync(id);
                if (tools == null)
                {
                    return CommonResponse<TeachingToolsResponseDto>.FailureResponse("teaching tools not found");
                }

                tools.Name = request.Name;
                tools.Description = request.Description;
                tools.UpdatedAt = DateTime.UtcNow;
                tools.UpdatedBy = request.Userid;

                var updated = await _repository.UpdateAsync(tools);

                _logger.LogInformation("teaching tools updated Id: {Id}", id);
                return CommonResponse<TeachingToolsResponseDto>.SuccessResponse(
                    "teaching tools updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating teaching tools Id: {Id}", id);
                return CommonResponse<TeachingToolsResponseDto>.FailureResponse(
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
                _logger.LogInformation("Deleting academic year Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "teaching tools deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "teaching tools not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting teaching tools Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered teaching tools with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated teaching tools result.</returns>
        public async Task<CommonResponse<PagedResult<TeachingToolsResponseDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered teaching tools. Filters => Active:{Active}, " +
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
                        x.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<TeachingToolsResponseDto>>.SuccessResponse(
                        "No teaching tools found.",
                        new PagedResult<TeachingToolsResponseDto>(
                            new List<TeachingToolsResponseDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                var teachingtools = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = teachingtools.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} teaching tools",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<TeachingToolsResponseDto>>.SuccessResponse(
                    "teaching tools fetched successfully.",
                    new PagedResult<TeachingToolsResponseDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered teaching tools.");
                return CommonResponse<PagedResult<TeachingToolsResponseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static TeachingToolsResponseDto Map(TeachingTools c) =>
         new TeachingToolsResponseDto
         {
             Id = c.Id,
             Name = c.Name,
             Description = c.Description,
         };

    }
}
