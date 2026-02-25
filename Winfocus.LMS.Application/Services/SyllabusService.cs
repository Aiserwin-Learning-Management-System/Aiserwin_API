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

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        public async Task<CommonResponse<SyllabusDto>> GetByIdAsync(Guid id)
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

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        /// <exception cref="InvalidOperationException">syllabus code already exists.</exception>
        public async Task<SyllabusDto> CreateAsync(SyllabusRequest request)
        {
            var syllabus = new Syllabus
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
            };

            var created = await _repository.AddAsync(syllabus);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">syllabus not found.</exception>
        /// <returns>task.</returns>
        public async Task<SyllabusDto> UpdateAsync(Guid id, SyllabusRequest request)
        {
            var syllabus = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("syllabus not found");

            syllabus.Name = request.name;
            syllabus.UpdatedBy = request.userId;
            syllabus.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(syllabus));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="centerid">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        public async Task<List<SyllabusDto>> GetByCenterIdAsync(Guid centerid)
        {
            var syllabuses = await _repository.GetByCenterIdAsync(centerid);
            return Map(syllabuses);
        }

        /// <summary>
        /// Retrieves syllabuses based on multiple filter criteria with pagination support.
        /// </summary>
        /// <param name="startDate">Filters syllabuses created on or after this date.</param>
        /// <param name="endDate">Filters syllabuses created on or before this date.</param>
        /// <param name="active">Indicates whether to filter active or inactive syllabuses.</param>
        /// <param name="searchText">Search keyword applied to syllabus name.</param>
        /// <param name="limit">Number of records to return (page size).</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order ("asc" or "desc").</param>
        /// <returns>
        /// A <see cref="CommonResponse{T}"/> containing a paginated list of
        /// <see cref="syllbusDto"/> objects.
        /// </returns>
        public async Task<CommonResponse<PagedResult<SyllabusDto>>> GetFilteredAsync(
       DateTime? startDate,
       DateTime? endDate,
       bool? active,
       string? searchText,
       int limit,
       int offset,
       string sortOrder)
        {
            try
            {
                _logger.LogInformation(
                             "Fetching filtered syllabus. Filters => Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                             active, searchText, limit, offset, sortOrder);
                var query = _repository.Query();
                if (active.HasValue)
                    query = query.Where(x => x.IsActive == active);

                if (startDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= endDate.Value);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(x =>
                        x.Name.Contains(searchText));
                }

                var totalCount = await query.CountAsync();

                _logger.LogInformation(
                "Filtered syllabuses count: {TotalCount}",
                totalCount);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt);

                var syllbuses = await query
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                var dtoList = syllbuses.Select(c => new SyllabusDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                }).ToList();

                _logger.LogInformation(
               "Returning {ReturnedCount} syllabuses (Offset:{Offset}, Limit:{Limit})",
               dtoList.Count, offset, limit);
                return CommonResponse<PagedResult<SyllabusDto>>.SuccessResponse(
                "Syllabuses fetched successfully",
                new PagedResult<SyllabusDto>(
                    dtoList,
                    totalCount,
                    limit,
                    offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error occurred while fetching filtered syllabuses. Filters =>  Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                   active, searchText, limit, offset, sortOrder);
                return CommonResponse<PagedResult<SyllabusDto>>.FailureResponse($"An error occurred while fetching syllabuses: {ex.Message}");
            }
        }

        private static List<SyllabusDto> Map(IEnumerable<Syllabus> syllabuses)
        {
            return syllabuses.Select(Map).ToList();
        }

        private static SyllabusDto Map(Syllabus c) =>
     new SyllabusDto
     {
         Id = c.Id,
         Name = c.Name,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedBy = c.UpdatedBy,
         UpdatedAt = c.UpdatedAt,
     };

    }
}
