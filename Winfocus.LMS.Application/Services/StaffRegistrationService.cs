namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Registration;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Handles staff registration submissions with full validation,
    /// file uploads, and atomic persistence.
    /// </summary>
    public sealed class StaffRegistrationService : IStaffRegistrationService
    {
        private readonly IStaffRegistrationRepository _repository;
        private readonly IFieldValueValidatorService _validator;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<StaffRegistrationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRegistrationService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="fileStorage">The file storage.</param>
        /// <param name="logger">The logger.</param>
        public StaffRegistrationService(
            IStaffRegistrationRepository repository,
            IFieldValueValidatorService validator,
            IFileStorageService fileStorage,
            ILogger<StaffRegistrationService> logger)
        {
            _repository = repository;
            _validator = validator;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<RegistrationResponseDto>> SubmitAsync(
            SubmitRegistrationRequest request, Guid userId)
        {
            try
            {
                var form = await _repository.GetFormWithFieldsAsync(request.FormId);

                if (form == null)
                {
                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse("Registration form not found.");
                }

                if (!form.IsActive)
                {
                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse("This registration form is currently inactive.");
                }

                if (request.Values == null || !request.Values.Any())
                {
                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse("At least one field value must be submitted.");
                }

                var formFieldMap = form.FormFields.ToDictionary(
                    rff => rff.FieldId,
                    rff => rff);

                var submittedFieldIds = request.Values
                    .Select(v => v.FieldId)
                    .ToHashSet();

                var unknownFields = submittedFieldIds
                    .Where(id => !formFieldMap.ContainsKey(id))
                    .ToList();

                if (unknownFields.Any())
                {
                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse(
                            $"Unknown fields submitted: {string.Join(", ", unknownFields)}. " +
                            "These fields do not belong to this form.");
                }

                var duplicateFieldIds = request.Values
                                          .GroupBy(v => v.FieldId)
                                          .Where(g => g.Count() > 1)
                                          .Select(g => g.Key)
                                          .ToList();

                if (duplicateFieldIds.Any())
                {
                    var dupNames = duplicateFieldIds
                        .Where(id => formFieldMap.ContainsKey(id))
                        .Select(id => formFieldMap[id].FormField.DisplayLabel);

                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse(
                            $"Duplicate fields: {string.Join(", ", dupNames)}");
                }

                var errors = new Dictionary<string, List<string>>();

                var requiredFormFields = form.FormFields
                    .Where(rff => rff.IsRequired)
                    .ToList();

                foreach (var requiredField in requiredFormFields)
                {
                    var fieldDef = requiredField.FormField;
                    var submitted = request.Values
                        .FirstOrDefault(v => v.FieldId == requiredField.FieldId);

                    if (submitted == null)
                    {
                        AddError(
                                 errors,
                                 fieldDef.FieldName,
                                 $"{fieldDef.DisplayLabel} is required.");
                        continue;
                    }

                    // File fields: check file presence
                    if (fieldDef.FieldType == FieldType.FileUpload)
                    {
                        if (submitted.File == null || submitted.File.Length == 0)
                        {
                            AddError(
                                errors,
                                fieldDef.FieldName,
                                $"{fieldDef.DisplayLabel} file is required.");
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(submitted.Value))
                    {
                        AddError(
                                 errors,
                                 fieldDef.FieldName,
                                 $"{fieldDef.DisplayLabel} is required.");
                    }
                }

                foreach (var input in request.Values)
                {
                    if (!formFieldMap.TryGetValue(input.FieldId, out var regFormField))
                    {
                        continue;
                    }

                    var fieldDef = regFormField.FormField;

                    if (fieldDef.FieldType == FieldType.FileUpload)
                    {
                        continue;
                    }

                    if (errors.ContainsKey(fieldDef.FieldName))
                    {
                        continue;
                    }

                    var fieldErrors = _validator.Validate(
                        fieldDef,
                        input.Value,
                        regFormField.IsRequired);

                    if (fieldErrors.Any())
                    {
                        errors[fieldDef.FieldName] = fieldErrors;
                    }
                }

                if (errors.Any())
                {
                    var errorDetails = errors.Select(e =>
                        $"{e.Key}: {string.Join(", ", e.Value)}");
                    var errorMessage = "Validation failed. " +
                        string.Join("; ", errorDetails);

                    _logger.LogWarning(
                                       "Registration validation failed for FormId {FormId}. Errors: {Errors}",
                                       request.FormId,
                                       errorMessage);

                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse(errorMessage);
                }

                var registrationId = Guid.NewGuid();
                var uploadedFiles = new List<string>();
                var values = new List<StaffRegistrationValue>();

                try
                {
                    foreach (var input in request.Values)
                    {
                        var fieldDef = formFieldMap[input.FieldId].FormField;
                        string? storedValue;

                        if (fieldDef.FieldType == FieldType.FileUpload
                            && input.File != null)
                        {
                            storedValue = await _fileStorage.UploadAsync(
                                input.File,
                                registrationId.ToString());

                            uploadedFiles.Add(storedValue);
                        }
                        else
                        {
                            storedValue = input.Value;
                        }

                        values.Add(new StaffRegistrationValue
                        {
                            FieldId = input.FieldId,
                            FieldName = fieldDef.FieldName,
                            Value = storedValue,
                        });
                    }

                    var registration = new StaffRegistration
                    {
                        Id = registrationId,
                        FormId = request.FormId,
                        StaffCategoryId = form.StaffCategoryId,
                        Status = RegistrationStatus.Submitted,
                        SubmittedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        Values = values,
                    };

                    await _repository.AddAsync(registration);

                    _logger.LogInformation(
                        "Registration {RegistrationId} submitted for Form {FormId} " +
                        "by User {UserId} with {ValueCount} values",
                        registrationId, request.FormId, userId, values.Count);

                    // ── 9. Return success ────────────────────────
                    return CommonResponse<RegistrationResponseDto>.SuccessResponse(
                        "Registration submitted successfully.",
                        new RegistrationResponseDto
                        {
                            Id = registration.Id,
                            FormId = form.Id,
                            FormName = form.FormName,
                            StaffCategory = form.StaffCategory.Name,
                            Status = registration.Status.ToString(),
                            SubmittedAt = registration.SubmittedAt,
                            CreatedAt = registration.CreatedAt,
                        });
                }
                catch (InvalidOperationException ex)
                    when (ex.Message.Contains("File size") ||
                          ex.Message.Contains("extension"))
                {
                    await CleanupUploadedFiles(uploadedFiles);

                    return CommonResponse<RegistrationResponseDto>
                        .FailureResponse($"File upload failed: {ex.Message}");
                }
                catch (Exception)
                {
                    await CleanupUploadedFiles(uploadedFiles);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error submitting registration for FormId {FormId}", request.FormId);

                return CommonResponse<RegistrationResponseDto>
                    .FailureResponse(
                        $"An error occurred while submitting registration: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<RegistrationDetailDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var registration = await _repository.GetByIdWithDetailsAsync(id);

                if (registration == null)
                {
                    return CommonResponse<RegistrationDetailDto>
                        .FailureResponse("Registration not found.");
                }

                var detail = new RegistrationDetailDto
                {
                    Id = registration.Id,
                    FormId = registration.FormId,
                    FormName = registration.RegistrationForm.FormName,
                    StaffCategory = registration.StaffCategory.Name,
                    Status = registration.Status.ToString(),
                    Remarks = registration.Remarks,
                    SubmittedAt = registration.SubmittedAt,
                    CreatedAt = registration.CreatedAt,
                    Values = registration.Values.Select(v =>
                    {
                        var isFile = v.FormField.FieldType == FieldType.FileUpload;

                        return new RegistrationValueDetailDto
                        {
                            FieldId = v.FieldId,
                            FieldName = v.FieldName,
                            DisplayLabel = v.FormField.DisplayLabel,
                            FieldType = v.FormField.FieldType.ToString(),
                            Value = v.Value,
                            FileUrl = isFile && !string.IsNullOrEmpty(v.Value)
                                ? _fileStorage.GetFileUrl(v.Value)
                                : null,
                        };
                    })
                    .OrderBy(v => v.FieldName)
                    .ToList(),
                };

                return CommonResponse<RegistrationDetailDto>.SuccessResponse(
                    "Registration fetched successfully.", detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching registration {RegistrationId}",
                    id);

                return CommonResponse<RegistrationDetailDto>
                    .FailureResponse(
                        $"An error occurred while fetching registration: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<CommonResponse<PagedResult<RegistrationResponseDto>>> GetFilteredAsync(
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

                if (request.Status.HasValue)
                {
                    var status = (RegistrationStatus)request.Status.Value;
                    query = query.Where(sr => sr.Status == status);
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
                        sr.RegistrationForm.FormName.ToLower().Contains(search) ||
                        sr.StaffCategory.Name.ToLower().Contains(search));
                }

                // ── Total count ──────────────────────────────────
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<RegistrationResponseDto>>
                        .SuccessResponse(
                            "No registrations found.",
                            new PagedResult<RegistrationResponseDto>(
                                new List<RegistrationResponseDto>(),
                                0, request.Limit, request.Offset));
                }

                // ── Sorting ──────────────────────────────────────
                var isDesc = request.SortOrder
                    .Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy?.ToLower() switch
                {
                    "formname" => isDesc
                        ? query.OrderByDescending(sr => sr.RegistrationForm.FormName)
                        : query.OrderBy(sr => sr.RegistrationForm.FormName),

                    "staffcategory" => isDesc
                        ? query.OrderByDescending(sr => sr.StaffCategory.Name)
                        : query.OrderBy(sr => sr.StaffCategory.Name),

                    "status" => isDesc
                        ? query.OrderByDescending(sr => sr.Status)
                        : query.OrderBy(sr => sr.Status),

                    "submittedat" => isDesc
                        ? query.OrderByDescending(sr => sr.SubmittedAt)
                        : query.OrderBy(sr => sr.SubmittedAt),

                    _ => isDesc
                        ? query.OrderByDescending(sr => sr.CreatedAt)
                        : query.OrderBy(sr => sr.CreatedAt),
                };

                // ── Pagination ───────────────────────────────────
                var registrations = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = registrations.Select(MapToResponse).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} registrations",
                    dtoList.Count,
                    totalCount);

                return CommonResponse<PagedResult<RegistrationResponseDto>>
                    .SuccessResponse(
                        "Registrations fetched successfully.",
                        new PagedResult<RegistrationResponseDto>(
                            dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered registrations.");

                return CommonResponse<PagedResult<RegistrationResponseDto>>
                    .FailureResponse(
                        $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// UpdateStatusAsync.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CommonResponse<string>> UpdateStatusAsync(Guid id, UpdateRegistrationStatusDto dto)
        {
            try
            {
                var registration = await _repository.GetByIdWithDetailsAsync(id);

                if (registration == null)
                {
                    return CommonResponse<string>
                        .FailureResponse("Staff registration not found.");
                }

                ValidateStatusTransition(registration.Status, dto.Status);

                registration.Status = dto.Status;
                registration.Remarks = dto.Remarks;

                if (dto.Status == RegistrationStatus.Submitted)
                {
                    registration.SubmittedAt = DateTime.UtcNow;
                }

                await _repository.UpdateStatusAsync(registration);

                return CommonResponse<string>.SuccessResponse(
                    "Registration status updated successfully.", dto.Remarks);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating registration status {RegistrationId}",
                    id);

                return CommonResponse<string>.FailureResponse(
                    $"An error occurred while updating registration status: {ex.Message}");
            }
        }

        private void ValidateStatusTransition(
            RegistrationStatus currentStatus,
            RegistrationStatus newStatus)
        {
            if (currentStatus == RegistrationStatus.Draft &&
                newStatus != RegistrationStatus.Submitted)
            {
                throw new Exception("Draft can only move to Submitted");
            }

            if (currentStatus == RegistrationStatus.Submitted &&
                newStatus != RegistrationStatus.Approved &&
                newStatus != RegistrationStatus.Rejected)
            {
                throw new Exception("Submitted can only move to Approved or Rejected");
            }

            if (currentStatus == RegistrationStatus.Approved ||
                currentStatus == RegistrationStatus.Rejected)
            {
                throw new Exception("Final status cannot be changed");
            }
        }

        private static RegistrationResponseDto MapToResponse(StaffRegistration sr) =>
            new ()
            {
                Id = sr.Id,
                FormId = sr.FormId,
                FormName = sr.RegistrationForm.FormName,
                StaffCategory = sr.StaffCategory.Name,
                Status = sr.Status.ToString(),
                Remarks = sr.Remarks,
                SubmittedAt = sr.SubmittedAt,
                CreatedAt = sr.CreatedAt,
            };

        private static void AddError(
            Dictionary<string, List<string>> errors,
            string fieldName,
            string message)
        {
            if (!errors.ContainsKey(fieldName))
            {
                errors[fieldName] = new List<string>();
            }
            errors[fieldName].Add(message);
        }

        private async Task CleanupUploadedFiles(List<string> filePaths)
        {
            foreach (var path in filePaths)
            {
                try
                {
                    await _fileStorage.DeleteAsync(path);
                    _logger.LogInformation("Cleaned up file: {Path}", path);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Failed to cleanup file during rollback: {Path}",
                        path);
                }
            }
        }
    }
}
