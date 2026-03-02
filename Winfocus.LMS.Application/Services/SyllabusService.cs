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
    /// CountryService.
    /// </summary>
    public sealed class SyllabusService : ISyllabusService
    {
        private readonly ISyllabusRepository _repository;
        private readonly ILogger<SyllabusService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyllabusService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public SyllabusService(
            ISyllabusRepository repository,
            ILogger<SyllabusService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CountryDto.</returns>
        public async Task<CommonResponse<List<SyllabusDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all syllabuses");
                var syllabuses = await _repository.GetAllAsync();
                var mappeddata = syllabuses.Select(Map).ToList();
                if (mappeddata.Any())
                {
                    return CommonResponse<List<SyllabusDto>>.SuccessResponse("Fetched all syllabuses", mappeddata);
                }
                else
                {
                    return CommonResponse<List<SyllabusDto>>.FailureResponse("syllabuses not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching syllabuses");
                return CommonResponse<List<SyllabusDto>>.FailureResponse($"An error occurred: {ex.Message}");
            }
         }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        public async Task<CommonResponse<SyllabusDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching syllabuses by Id: {Id}", id);
                var syllabus = await _repository.GetByIdAsync(id);
                _logger.LogInformation("syllabuses fetched successfully for Id: {Id}", id);
                var mappeddata = syllabus == null ? null : Map(syllabus);
                if (mappeddata != null)
                {
                    return CommonResponse<SyllabusDto>.SuccessResponse("fetched syllabus for this id", mappeddata);
                }
                else
                {
                    return CommonResponse<SyllabusDto>.FailureResponse("syllabus not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching syllabus by Id: {Id}", id);
                return CommonResponse<SyllabusDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
         }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        /// <exception cref="InvalidOperationException">syllabus code already exists.</exception>
        public async Task<CommonResponse<SyllabusDto>> CreateAsync(SyllabusRequest request)
        {
            try
            {

                var syllabus = new Syllabus
                {
                    Name = request.Name,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId,
                };

                var created = await _repository.AddAsync(syllabus);
                return CommonResponse<SyllabusDto>.SuccessResponse(
                 "Syllabus created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating syllabus");
                return CommonResponse<SyllabusDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">syllabus not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<SyllabusDto>> UpdateAsync(Guid id, SyllabusRequest request)
        {
            try
            {
                _logger.LogInformation("Updating Syllabus Id: {Id}", id);

                var batch = await _repository.GetByIdAsync(id);
                if (batch == null)
                {
                    return CommonResponse<SyllabusDto>.FailureResponse("Syllabus not found");
                }

                batch.Name = request.Name;
                batch.UpdatedAt = DateTime.UtcNow;
                batch.UpdatedBy = request.UserId;

                var updated = await _repository.UpdateAsync(batch);

                _logger.LogInformation("Syllabus updated Id: {Id}", id);
                return CommonResponse<SyllabusDto>.SuccessResponse(
                    "Syllabus updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Syllabus Id: {Id}", id);
                return CommonResponse<SyllabusDto>.FailureResponse(
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
                _logger.LogInformation("Deleting Syllabus Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Syllabus deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Syllabus not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Syllabus Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves syllabuses based on filter criteria with pagination.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated syllabus result.</returns>
        public async Task<CommonResponse<PagedResult<SyllabusDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered syllabuses. Filters => Active:{Active}, Search:{SearchText}, " +
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

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<SyllabusDto>>.SuccessResponse(
                        "No syllabuses found.",
                        new PagedResult<SyllabusDto>(
                            new List<SyllabusDto>(), 0, request.Limit, request.Offset));
                }

                // ── Sorting ──
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

                // ── Pagination ──
                var syllabuses = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = syllabuses.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} syllabuses",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<SyllabusDto>>.SuccessResponse(
                    "Syllabuses fetched successfully.",
                    new PagedResult<SyllabusDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered syllabuses.");
                return CommonResponse<PagedResult<SyllabusDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static SyllabusDto Map(Syllabus c) =>
     new SyllabusDto
     {
         Id = c.Id,
         Name = c.Name,
         IsActive = c.IsActive,
     };
     }
}
