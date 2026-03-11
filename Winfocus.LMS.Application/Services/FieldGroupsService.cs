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
    /// FieldGroupsService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFieldGroupRepository" />
    public sealed class FieldGroupsService : IFieldGroupServices
    {
        private readonly IFieldGroupRepository _repository;
        private readonly ILogger<FieldGroupsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public FieldGroupsService(
            IFieldGroupRepository repository,
            ILogger<FieldGroupsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>FieldGroupDto.</returns>
        public async Task<CommonResponse<List<FieldGroupDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all fieldGroup");
                var filedGroup = await _repository.GetAllAsync();
                var mapped = filedGroup.Select(Map).ToList();
                if (mapped.Any())
                {
                    return CommonResponse<List<FieldGroupDto>>.SuccessResponse("fetching all fieldGroup", mapped);
                }
                else
                {
                    return CommonResponse<List<FieldGroupDto>>.FailureResponse("no fieldGroup found");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<List<FieldGroupDto>>.FailureResponse($"An error occurred while fetching fieldGroup: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FieldGroupDto.</returns>
        public async Task<CommonResponse<FieldGroupDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var filedGroup = await _repository.GetByIdAsync(id);
                var mappeddata = filedGroup == null ? null : Map(filedGroup);
                if (mappeddata != null)
                {
                    return CommonResponse<FieldGroupDto>.SuccessResponse("fetching FieldGroup by id", mappeddata);
                }
                else
                {
                    return CommonResponse<FieldGroupDto>.FailureResponse("FieldGroup not found");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<FieldGroupDto>.FailureResponse($"An error occurred while fetching FieldGroup: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>FieldGroupDto.</returns>
        /// <exception cref="InvalidOperationException">Field GroupDto already exists.</exception>
        public async Task<CommonResponse<FieldGroupDto>> CreateAsync(CreateFieldGroupRequest request, Guid userid)
        {
            try
            {
                bool codeExists = await _repository.ExistsByNameAsync(request.groupName);
                if (codeExists)
                {
                    return CommonResponse<FieldGroupDto>.FailureResponse("Failed to create FieldGroup, because the group name already exist.");
                }

                var filedGrp = new FieldGroup
                {
                    GroupName = request.groupName,
                    DisplayOrder = request.displayOrder,
                    Description = request.description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userid,
                };

                var created = await _repository.AddAsync(filedGrp);
                var mapped = Map(created);
                if (mapped == null)
                {
                    return CommonResponse<FieldGroupDto>.FailureResponse("Failed to create FieldGroup.");
                }
                else
                {
                    return CommonResponse<FieldGroupDto>.SuccessResponse("FieldGroup created successfully.", mapped);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<FieldGroupDto>.FailureResponse($"An error occurred while creating FieldGroup: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <exception cref="KeyNotFoundException">FieldGroup not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<FieldGroupDto>> UpdateAsync(Guid id, CreateFieldGroupRequest request, Guid userid)
        {
            try
            {
                var filedGroup = await _repository.GetByIdAsync(id);
                if (filedGroup == null)
                {
                    return CommonResponse<FieldGroupDto>.FailureResponse("FieldGroup not found.");
                }

                filedGroup.GroupName = request.groupName;
                filedGroup.Description = request.description;
                filedGroup.UpdatedAt = DateTime.UtcNow;
                filedGroup.UpdatedBy = userid;
                filedGroup.DisplayOrder = request.displayOrder;

                var mapped = Map(await _repository.UpdateAsync(filedGroup));
                if (mapped == null)
                {
                    return CommonResponse<FieldGroupDto>.FailureResponse("Failed to update FieldGroup.");
                }
                else
                {
                    return CommonResponse<FieldGroupDto>.SuccessResponse("FieldGroup updated successfully.", mapped);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<FieldGroupDto>.FailureResponse($"An error occurred while updating FieldGroup: {ex.Message}");
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
                bool res = await _repository.DeleteAsync(id);
                if (!res)
                {
                    return CommonResponse<bool>.FailureResponse("Failed to delete FieldGroup.");
                }
                else
                {
                    return CommonResponse<bool>.SuccessResponse("FieldGroup deleted successfully.", true);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<bool>.FailureResponse($"An error occurred while deleting FieldGroup: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by name asynchronous.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repository.ExistsByNameAsync(name);
        }

        /// <summary>
        /// Gets the by name asynchronous.
        /// </summary>
        /// <param name="groupId">The identifier.</param>
        /// <returns>FieldGroup.</returns>
        public async Task<CommonResponse<FieldGroupFieldsResponseDto>> GetFieldsByGroupIdAsync(Guid groupId)
        {
            try
            {
                var fieldGroup = await _repository.GetFieldsByGroupIdAsync(groupId);

                if (fieldGroup != null)
                {
                    return CommonResponse<FieldGroupFieldsResponseDto>
                        .SuccessResponse("Fetching fields by FieldGroupId successful", fieldGroup);
                }
                else
                {
                    return CommonResponse<FieldGroupFieldsResponseDto>
                        .FailureResponse("FieldGroup not found");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse<FieldGroupFieldsResponseDto>
                    .FailureResponse($"An error occurred while fetching fields: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered field group with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated FieldGroupDto result.</returns>
        public async Task<CommonResponse<PagedResult<FieldGroupDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered FieldGroup. Filters => Active:{Active}, " +
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

                // ── display name
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.GroupName.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<FieldGroupDto>>.SuccessResponse(
                        "No FieldGroup found.",
                        new PagedResult<FieldGroupDto>(
                            new List<FieldGroupDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.GroupName)
                                             : query.OrderBy(x => x.GroupName),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var fieldGrp = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = fieldGrp.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} FieldGroup",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<FieldGroupDto>>.SuccessResponse(
                    "FieldGroup fetched successfully.",
                    new PagedResult<FieldGroupDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered FieldGroup.");
                return CommonResponse<PagedResult<FieldGroupDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<FieldGroupDto> Map(IEnumerable<FieldGroup> fieldgroup)
        {
            return fieldgroup.Select(Map).ToList();
        }

        private static FieldGroupDto Map(FieldGroup c) =>
          new FieldGroupDto
          {
              GroupName = c.GroupName,
              Description = c.Description,
              DisplayOrder = c.DisplayOrder
          };
    }
}
