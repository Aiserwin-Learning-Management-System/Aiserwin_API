using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Registration;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business logic implementation for managing registration forms.
    /// This service interacts with the repository layer to perform
    /// database operations while enforcing application business rules.
    /// </summary>
    public class RegistrationFormService : IRegistrationFormService
    {
        /// <summary>
        /// Repository used for accessing registration form data.
        /// </summary>
        private readonly IRegistrationFormRepository _repository;
        private readonly ILogger<RegistrationFormService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFormService"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository responsible for registration form persistence.
        /// </param>
        /// <param name="logger">Logger.</param>
        public RegistrationFormService(IRegistrationFormRepository repository, ILogger<RegistrationFormService> logger)
        {
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<Guid>> CreateAsync(CreateRegistrationFormDto dto)
        {
            try
            {
                _logger.LogInformation("Creating registration form for StaffCategoryId: {StaffCategoryId}", dto.StaffCategoryId);

                // Check if form already exists for this StaffCategoryId
                var existingForm = await _repository.GetByStaffCategoryIdAsync(dto.StaffCategoryId);

                if (existingForm != null)
                {
                    _logger.LogWarning("Registration form already exists for StaffCategoryId: {StaffCategoryId}", dto.StaffCategoryId);

                    return CommonResponse<Guid>.FailureResponse(
                        "Registration form already exists for this staff category");
                }

                var form = new RegistrationForm
                {
                    Id = Guid.NewGuid(),
                    StaffCategoryId = dto.StaffCategoryId,
                    FormName = dto.FormName,
                    Description = dto.Description,
                    IsActive = true
                };

                await _repository.AddAsync(form);

                var formGroups = new List<RegistrationFormGroup>();
                var formFields = new List<RegistrationFormField>();

                if (dto.Groups != null)
                {
                    foreach (var group in dto.Groups)
                    {
                        var formGroup = new RegistrationFormGroup
                        {
                            Id = Guid.NewGuid(),
                            FormId = form.Id,
                            FieldGroupId = group.FieldGroupId,
                            DisplayOrder = group.DisplayOrder
                        };

                        formGroups.Add(formGroup);
                    }
                }

                if (dto.StandaloneFields != null)
                {
                    foreach (var field in dto.StandaloneFields)
                    {
                        formFields.Add(new RegistrationFormField
                        {
                            Id = Guid.NewGuid(),
                            FormId = form.Id,
                            FieldId = field.FieldId,
                            DisplayOrder = field.DisplayOrder,
                            IsRequired = field.IsRequired
                        });
                    }
                }

                if (formGroups.Any())
                    await _repository.AddGroupsAsync(formGroups);

                if (formFields.Any())
                    await _repository.AddFieldsAsync(formFields);

                _logger.LogInformation("Registration form created successfully. FormId: {FormId}", form.Id);

                return CommonResponse<Guid>.SuccessResponse(
                    "Registration form created successfully", form.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating registration form for StaffCategoryId: {StaffCategoryId}", dto.StaffCategoryId);

                return CommonResponse<Guid>.FailureResponse(
                    "An error occurred while creating the registration form.");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<RegistrationFormResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching registration form for Id: {Id}", id);

                var form = await _repository.GetByIdAsync(id);

                if (form == null)
                {
                    _logger.LogWarning("Registration form not found for Id: {Id}", id);
                    return CommonResponse<RegistrationFormResponseDto>.FailureResponse("Registration form not found");
                }

                var mappedData = Map(form);

                _logger.LogInformation("Registration form data fetched successfully for Id: {Id}", id);

                return CommonResponse<RegistrationFormResponseDto>.SuccessResponse(
                    "Registration form fetched successfully",
                    mappedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching registration form for Id: {Id}", id);

                return CommonResponse<RegistrationFormResponseDto>.FailureResponse(
                    "An error occurred while fetching the registration form");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<List<RegistrationFormResponseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all registration forms");

                var forms = await _repository.GetAllAsync();

                var data = forms.Select(Map).ToList();

                _logger.LogInformation("Registration forms fetched successfully. Count: {Count}", data.Count);

                return CommonResponse<List<RegistrationFormResponseDto>>.SuccessResponse(
                    "Registration forms fetched successfully",
                    data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching registration forms");

                return CommonResponse<List<RegistrationFormResponseDto>>.FailureResponse(
                    "An error occurred while fetching registration forms");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<Guid>> UpdateAsync(Guid id, CreateRegistrationFormDto dto)
        {
            try
            {
                _logger.LogInformation("Updating registration form for Id: {Id}", id);

                var form = await _repository.GetByIdAsync(id);

                if (form == null)
                {
                    _logger.LogWarning("Registration form not found for Id: {Id}", id);

                    return CommonResponse<Guid>.FailureResponse("Registration form not found");
                }

                // Check if another form already uses this StaffCategoryId
                var existingForm = await _repository.GetByStaffCategoryIdAsync(dto.StaffCategoryId);

                if (existingForm != null && existingForm.Id != id)
                {
                    _logger.LogWarning(
                        "Registration form already exists for StaffCategoryId: {StaffCategoryId}",
                        dto.StaffCategoryId);

                    return CommonResponse<Guid>.FailureResponse(
                        "Registration form already exists for this staff category");
                }

                // Update basic details
                form.FormName = dto.FormName;
                form.Description = dto.Description;

                await _repository.UpdateAsync(form);

                // Remove existing groups and fields
                await _repository.DeleteGroupsByFormIdAsync(id);
                await _repository.DeleteFieldsByFormIdAsync(id);

                var formGroups = new List<RegistrationFormGroup>();
                var formFields = new List<RegistrationFormField>();

                // Add groups
                if (dto.Groups != null)
                {
                    foreach (var group in dto.Groups)
                    {
                        formGroups.Add(new RegistrationFormGroup
                        {
                            Id = Guid.NewGuid(),
                            FormId = form.Id,
                            FieldGroupId = group.FieldGroupId,
                            DisplayOrder = group.DisplayOrder
                        });
                    }
                }

                // Add standalone fields
                if (dto.StandaloneFields != null)
                {
                    foreach (var field in dto.StandaloneFields)
                    {
                        formFields.Add(new RegistrationFormField
                        {
                            Id = Guid.NewGuid(),
                            FormId = form.Id,
                            FieldId = field.FieldId,
                            DisplayOrder = field.DisplayOrder,
                            IsRequired = field.IsRequired
                        });
                    }
                }

                if (formGroups.Any())
                    await _repository.AddGroupsAsync(formGroups);

                if (formFields.Any())
                    await _repository.AddFieldsAsync(formFields);

                _logger.LogInformation("Registration form updated successfully. Id: {Id}", form.Id);

                return CommonResponse<Guid>.SuccessResponse(
                    "Registration form updated successfully",
                    form.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating registration form for Id: {Id}", id);

                return CommonResponse<Guid>.FailureResponse(
                    "An error occurred while updating the registration form");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting registration form for Id: {Id}", id);

                var form = await _repository.GetByIdAsync(id);

                if (form == null)
                {
                    _logger.LogWarning("Registration form not found for Id: {Id}", id);

                    return CommonResponse<bool>.FailureResponse("Registration form not found");
                }

                bool result = await _repository.DeleteAsync(id);

                _logger.LogInformation("Registration form deleted successfully for Id: {Id}", id);

                return CommonResponse<bool>.SuccessResponse(
                    "Registration form deleted successfully",
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting registration form for Id: {Id}", id);

                return CommonResponse<bool>.FailureResponse(
                    "An error occurred while deleting the registration form");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<RegistrationFormResponseDto>>> GetFilteredAsync(
            StaffRegistrationFilterRequest request)
        {
            try
            {
                var query = _repository.Query();

                // ── Filters ──────────────────────────────────────
                if (request.StaffCategoryId.HasValue)
                {
                    query = query.Where(sr =>
                        sr.StaffCategoryId == request.StaffCategoryId.Value);
                }

                if (request.StartDate.HasValue)
                {
                    query = query.Where(sr =>
                        sr.CreatedAt >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    query = query.Where(sr =>
                        sr.CreatedAt <= request.EndDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var search = request.SearchText.Trim().ToLower();
                    query = query.Where(sr =>
                        sr.FormName.ToLower().Contains(search) ||
                        sr.StaffCategory.Name.ToLower().Contains(search));
                }

                // ── Total count ──────────────────────────────────
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<RegistrationFormResponseDto>>
                        .SuccessResponse(
                            "No registrations found.",
                            new PagedResult<RegistrationFormResponseDto>(
                                new List<RegistrationFormResponseDto>(),
                                0, request.Limit, request.Offset));
                }

                // ── Sorting ──────────────────────────────────────
                var isDesc = request.SortOrder
                    .Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy?.ToLower() switch
                {
                    "formname" => isDesc
                        ? query.OrderByDescending(sr => sr.FormName)
                        : query.OrderBy(sr => sr.FormName),

                    "staffcategory" => isDesc
                        ? query.OrderByDescending(sr => sr.StaffCategory.Name)
                        : query.OrderBy(sr => sr.StaffCategory.Name),


                    _ => isDesc
                        ? query.OrderByDescending(sr => sr.CreatedAt)
                        : query.OrderBy(sr => sr.CreatedAt),
                };

                // ── Pagination ───────────────────────────────────
                var registrations = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = registrations.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} registrations",
                    dtoList.Count,
                    totalCount);

                return CommonResponse<PagedResult<RegistrationFormResponseDto>>
                    .SuccessResponse(
                        "Registration form fetched successfully.",
                        new PagedResult<RegistrationFormResponseDto>(
                            dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered registration form.");

                return CommonResponse<PagedResult<RegistrationFormResponseDto>>
                    .FailureResponse(
                        $"An error occurred: {ex.Message}");
            }
        }

        private static RegistrationFormResponseDto Map(RegistrationForm c)
        {
            var sections = new List<FormSectionDto>();

            // GROUP SECTIONS
            if (c.FormGroups != null && c.FormGroups.Any())
            {
                var groupSections = c.FormGroups.OrderBy(g => g.DisplayOrder).
                    Select(g => new FormSectionDto
                    {
                        Type = "group",
                        GroupId = g.FieldGroupId,
                        GroupName = g.FieldGroup?.GroupName,
                        DisplayOrder = g.DisplayOrder
                    })
                    .ToList();

                sections.AddRange(groupSections);
            }

            // STANDALONE FIELDS
            var standaloneFields = c.FormFields
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FormFieldResponseDto
                {
                    FieldId = f.FieldId,
                    FieldName = f.FormField?.FieldName,
                    DisplayLabel = f.FormField?.DisplayLabel,
                    FieldType = f.FormField?.FieldType.ToString(),
                    IsRequired = f.IsRequired,
                    DisplayOrder = f.DisplayOrder,

                    Options = f.FormField?.FieldOptions?
                        .OrderBy(o => o.DisplayOrder)
                        .Select(o => new FieldOptionDto
                        {
                            Id = o.Id,
                            OptionValue = o.OptionValue,
                            DisplayOrder = o.DisplayOrder
                        }).ToList()
                }).ToList();

            if (standaloneFields.Any())
            {
                sections.Add(new FormSectionDto
                {
                    Type = "standalone",
                    GroupId = null,
                    GroupName = "Additional Fields",
                    DisplayOrder = 99,
                    Formfields = standaloneFields
                });
            }

            return new RegistrationFormResponseDto
            {
                Id = c.Id,
                StaffCategoryId = c.StaffCategoryId,
                FormName = c.FormName,
                IsActive = c.IsActive,

                StaffCategory = c.StaffCategory == null ? null : new StaffCategoryDto
                {
                    Id = c.StaffCategory.Id,
                    Name = c.StaffCategory.Name,
                    Description = c.StaffCategory.Description
                },

                Sections = sections.OrderBy(s => s.DisplayOrder).ToList()
            };
        }
    }
}
