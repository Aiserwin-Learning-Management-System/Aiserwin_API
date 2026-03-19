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
    /// Provides business operations for <see cref="StaffCategory"/> entities.
    /// </summary>
    public class StaffCategoryService : IStaffCategoryService
    {
        private readonly IStaffCategoryRepository _repository;
        private readonly ILogger<StaffCategoryService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StaffCategoryService(IStaffCategoryRepository repository, ILogger<StaffCategoryService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StaffCategoryDto.</returns>
        public async Task<CommonResponse<List<StaffCategoryDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all staff categories.");
            var staffcategory = await _repository.GetAllAsync();
            var data = staffcategory.Select(Map).ToList();
            return CommonResponse<List<StaffCategoryDto>>.SuccessResponse("Staff Categories ", data);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StaffCategoryDto.</returns>
        public async Task<CommonResponse<StaffCategoryDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching staff category by Id: {Id}", id);
                var staffcategory = await _repository.GetByIdAsync(id);
                _logger.LogInformation("staff category fetched successfully for Id: {Id}", id);
                var mappeddata = staffcategory == null ? null : Map(staffcategory);
                if (mappeddata != null)
                {
                    return CommonResponse<StaffCategoryDto>.SuccessResponse("fetching staff category by id", mappeddata);
                }
                else
                {
                    return CommonResponse<StaffCategoryDto>.FailureResponse("Staff category not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Staff category.");
                return CommonResponse<StaffCategoryDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StaffCategoryDto.</returns>
        public async Task<CommonResponse<StaffCategoryDto>> CreateAsync(StaffCategoryRequestDto request)
        {
            try
            {
                var exists = await _repository.Query()
            .AnyAsync(x => x.Name.ToLower() == request.name.ToLower());

                if (exists)
                {
                    return CommonResponse<StaffCategoryDto>.FailureResponse(
                        "Staff category with the same name already exists");
                }

                var placeHolderText = string.IsNullOrWhiteSpace(request.placeholder) ? $"Enter {request.name}" : request.placeholder;

                var staffcategory = new StaffCategory
                {
                    Name = request.name,
                    Description = request.description,
                    CreatedBy = request.userId,
                    CreatedAt = DateTime.UtcNow,
                    PlaceholderText = placeHolderText,
                };

                var created = await _repository.AddAsync(staffcategory);
                _logger.LogInformation(
               "staff category created successfully. Id: {Id}",
               created.Id);
                return CommonResponse<StaffCategoryDto>.SuccessResponse(
                  "staff category created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating staff category: {Name}", request.name);
                return CommonResponse<StaffCategoryDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Staff category not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<StaffCategoryDto>> UpdateAsync(Guid id, StaffCategoryRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating staff category Id: {Id}", id);

                var staff_category = await _repository.GetByIdAsync(id);
                if (staff_category == null)
                {
                    return CommonResponse<StaffCategoryDto>.FailureResponse("staff category not found");
                }

                var exists = await _repository.Query()
                    .AnyAsync(x => x.Name.ToLower() == request.name.ToLower() && x.Id != id);

                if (exists)
                {
                    return CommonResponse<StaffCategoryDto>.FailureResponse(
                        "Staff category with the same name already exists");
                }

                var placeHolderText = string.IsNullOrWhiteSpace(request.placeholder) ? $"Enter {request.name}" : request.placeholder;

                staff_category.Name = request.name;
                staff_category.Description = request.description;
                staff_category.UpdatedAt = DateTime.UtcNow;
                staff_category.UpdatedBy = request.userId;
                staff_category.PlaceholderText = placeHolderText;

                var updated = await _repository.UpdateAsync(staff_category);

                _logger.LogInformation("staff category updated Id: {Id}", id);
                return CommonResponse<StaffCategoryDto>.SuccessResponse(
                    "staff category updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating staff category Id: {Id}", id);
                return CommonResponse<StaffCategoryDto>.FailureResponse(
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
                _logger.LogInformation("Deleting staff category Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);
                var count = await _repository.Query()
                                .CountAsync(x => x.Id == id);

                if (count > 0)
                {
                    return CommonResponse<bool>.FailureResponse(
                        $"Category is in use by {count} registration form(s)");
                }

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Staff Category deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Staff Category not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Staff Category Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered staff category with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated staff category result.</returns>
        public async Task<CommonResponse<PagedResult<StaffCategoryDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Staff Category. Filters => Active:{Active}, " +
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

                // ── Search on category name or description ──
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
                    return CommonResponse<PagedResult<StaffCategoryDto>>.SuccessResponse(
                        "No staff acategory found.",
                        new PagedResult<StaffCategoryDto>(
                            new List<StaffCategoryDto>(), 0, request.Limit, request.Offset));
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

                // ── Pagination ──
                var subjects = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = subjects.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} staff category",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<StaffCategoryDto>>.SuccessResponse(
                    "staff category fetched successfully.",
                    new PagedResult<StaffCategoryDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered staff category.");
                return CommonResponse<PagedResult<StaffCategoryDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<StaffCategoryDto> Map(IEnumerable<StaffCategory> staffcategory)
        {
            return staffcategory.Select(Map).ToList();
        }

        private static StaffCategoryDto Map(StaffCategory c) =>
    new StaffCategoryDto
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        PlaceHolder = c.PlaceholderText,
        IsActive = c.IsActive,
    };
    }
}
