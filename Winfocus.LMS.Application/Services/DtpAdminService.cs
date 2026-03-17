namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// DtpAdminService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IDtpAdminService" />
    public sealed class DtpAdminService : IDtpAdminService
    {
        private readonly IDtpAdminRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<DtpAdminService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DtpAdminService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="fileStorage">The file storage.</param>
        /// <param name="logger">The logger.</param>
        public DtpAdminService(
            IDtpAdminRepository repo,
            IFileStorageService fileStorage,
            ILogger<DtpAdminService> logger)
        {
            _repo = repo;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        /// <summary>
        /// Gets column definitions from the active DTP registration form.
        /// </summary>
        /// <returns>
        /// List of columns with metadata for frontend table construction.
        /// </returns>
        public async Task<CommonResponse<OperatorColumnsResponseDto>> GetColumnsAsync()
        {
            try
            {
                var form = await _repo.GetDtpRegistrationFormAsync();

                if (form == null)
                {
                    return CommonResponse<OperatorColumnsResponseDto>
                        .FailureResponse(
                            "No active registration form found for DTP staff category.");
                }

                var columns = form.FormFields
                    .OrderBy(rff => rff.DisplayOrder)
                    .Select(rff => new OperatorColumnDto
                    {
                        FieldId = rff.FieldId,
                        FieldName = rff.FormField.FieldName,
                        DisplayLabel = rff.FormField.DisplayLabel,
                        FieldType = rff.FormField.FieldType.ToString(),
                        DisplayOrder = rff.DisplayOrder
                    })
                    .ToList();

                var response = new OperatorColumnsResponseDto
                {
                    Columns = columns,
                    FixedColumns = new List<string> { "slNo", "status", "actions" }
                };

                _logger.LogInformation(
                    "Returning {Count} dynamic columns for DTP operator list",
                    columns.Count);

                return CommonResponse<OperatorColumnsResponseDto>.SuccessResponse(
                    "Columns loaded successfully.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading DTP operator columns.");
                return CommonResponse<OperatorColumnsResponseDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets paginated operator list with dynamic values.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Paginated list of operators with dynamic column data.
        /// </returns>
        public async Task<CommonResponse<OperatorListResponseDto>> GetOperatorsAsync(
                                                                    DtpOperatorFilterRequest request)
        {
            try
            {
                var allRegistrations = await _repo.GetDtpRegistrationsAsync();

                if (!allRegistrations.Any())
                {
                    return CommonResponse<OperatorListResponseDto>.SuccessResponse(
                        "No DTP operators found.",
                        new OperatorListResponseDto
                        {
                            Data = new List<OperatorListItemDto>(),
                            TotalCount = 0,
                            PageNumber = 1,
                            PageSize = request.Limit
                        });
                }

                // ── Map to intermediate list ─────────────────────
                var operators = allRegistrations.Select(reg =>
                {
                    var values = new Dictionary<string, string?>(
                        StringComparer.OrdinalIgnoreCase);

                    foreach (var v in reg.Values)
                    {
                        var key = v.FieldName?.Trim().ToLower() ?? "";
                        if (!string.IsNullOrEmpty(key) && !values.ContainsKey(key))
                        {
                            var val = v.Value;
                            if (v.FormField?.FieldType == FieldType.FileUpload
                                && !string.IsNullOrEmpty(val))
                            {
                                val = _fileStorage.GetFileUrl(val);
                            }
                            values[key] = val;
                        }
                    }

                    var (status, color) = GetStatusInfo(reg.Status);

                    return new OperatorListItemDto
                    {
                        RegistrationId = reg.Id,
                        SlNo = 0,
                        Status = status,
                        StatusColor = color,
                        IsActive = reg.IsActive,
                        Values = values,
                        RegisteredAt = reg.CreatedAt
                    };
                }).ToList();

                // ── Filter by status ─────────────────────────────
                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    operators = operators
                        .Where(o => o.Status.Equals(
                            request.Status, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // ── Filter by active (uses PagedRequest.Active) ──
                if (request.Active.HasValue)
                {
                    operators = operators
                        .Where(o => o.IsActive == request.Active.Value)
                        .ToList();
                }

                // ── Filter by date range ─────────────────────────
                if (request.StartDate.HasValue)
                {
                    operators = operators
                        .Where(o => o.RegisteredAt >= request.StartDate.Value)
                        .ToList();
                }

                if (request.EndDate.HasValue)
                {
                    operators = operators
                        .Where(o => o.RegisteredAt <= request.EndDate.Value.Date.AddDays(1))
                        .ToList();
                }

                // ── Search across ALL dynamic values ─────────────
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var search = request.SearchText.Trim().ToLower();
                    operators = operators.Where(o =>
                        o.Values.Values.Any(v =>
                            v != null && v.ToLower().Contains(search))
                        || o.Status.ToLower().Contains(search)
                    ).ToList();
                }

                // ── Total count after filtering ──────────────────
                var totalCount = operators.Count;

                // ── Sort (uses PagedRequest.SortBy + SortOrder) ──
                operators = SortOperators(operators, request.SortBy, request.SortOrder);

                // ── Paginate (uses PagedRequest.Offset + Limit) ──
                var pagedOperators = operators
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList();

                // ── Set SlNo ─────────────────────────────────────
                for (int i = 0; i < pagedOperators.Count; i++)
                {
                    pagedOperators[i].SlNo = request.Offset + i + 1;
                }

                var pageNumber = (request.Offset / request.Limit) + 1;

                _logger.LogInformation(
                    "Returning {Count} of {Total} DTP operators. " +
                    "Offset {Offset}, Limit {Limit}",
                    pagedOperators.Count, totalCount,
                    request.Offset, request.Limit);

                return CommonResponse<OperatorListResponseDto>.SuccessResponse(
                    "Operators loaded successfully.",
                    new OperatorListResponseDto
                    {
                        Data = pagedOperators,
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = request.Limit
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading DTP operators.");
                return CommonResponse<OperatorListResponseDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Soft-deletes an operator registration.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <param name="adminUserId">The admin user ID performing the deletion.</param>
        /// <returns>
        /// True if deletion was successful, false otherwise.
        /// </returns>
        public async Task<CommonResponse<bool>> DeleteOperatorAsync(
            Guid registrationId, Guid adminUserId)
        {
            try
            {
                var registration = await _repo.GetRegistrationByIdAsync(registrationId);

                if (registration == null)
                {
                    return CommonResponse<bool>.FailureResponse(
                        "Operator registration not found.");
                }

                // Verify it's a DTP registration
                if (registration.StaffCategory == null ||
                    !registration.StaffCategory.Name.Contains(
                        "DTP", StringComparison.OrdinalIgnoreCase))
                {
                    return CommonResponse<bool>.FailureResponse(
                        "This registration is not a DTP operator.");
                }

                registration.IsActive = false;
                registration.IsDeleted = true;
                registration.UpdatedAt = DateTime.UtcNow;
                registration.UpdatedBy = adminUserId;

                await _repo.SaveChangesAsync();

                _logger.LogInformation(
                    "DTP operator registration {RegId} soft-deleted by admin {AdminId}",
                    registrationId, adminUserId);

                return CommonResponse<bool>.SuccessResponse(
                    "Operator registration deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error deleting operator registration {RegId}", registrationId);
                return CommonResponse<bool>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles operator active/inactive status.
        /// </summary>
        /// <param name="registrationId">The registration ID.</param>
        /// <param name="adminUserId">The admin user ID performing the toggle.</param>
        /// <returns>
        /// New status of the operator after toggle.
        /// </returns>
        public async Task<CommonResponse<OperatorToggleResponseDto>> ToggleOperatorAsync(
            Guid registrationId, Guid adminUserId)
        {
            try
            {
                var registration = await _repo.GetRegistrationByIdAsync(registrationId);

                if (registration == null)
                {
                    return CommonResponse<OperatorToggleResponseDto>
                        .FailureResponse("Operator registration not found.");
                }

                if (registration.StaffCategory == null ||
                    !registration.StaffCategory.Name.Contains(
                        "DTP", StringComparison.OrdinalIgnoreCase))
                {
                    return CommonResponse<OperatorToggleResponseDto>
                        .FailureResponse("This registration is not a DTP operator.");
                }

                // Toggle
                registration.IsActive = !registration.IsActive;
                registration.UpdatedAt = DateTime.UtcNow;
                registration.UpdatedBy = adminUserId;

                await _repo.SaveChangesAsync();

                var action = registration.IsActive ? "enabled" : "disabled";

                _logger.LogInformation(
                    "DTP operator {RegId} {Action} by admin {AdminId}",
                    registrationId, action, adminUserId);

                return CommonResponse<OperatorToggleResponseDto>.SuccessResponse(
                    $"Operator {action} successfully.",
                    new OperatorToggleResponseDto
                    {
                        RegistrationId = registrationId,
                        IsActive = registration.IsActive,
                        Message = $"Operator {action} successfully."
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error toggling operator {RegId}", registrationId);
                return CommonResponse<OperatorToggleResponseDto>
                    .FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the status information.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private static (string Status, string Color) GetStatusInfo(RegistrationStatus status)
        {
            return status switch
            {
                RegistrationStatus.Draft => ("Draft", "grey"),
                RegistrationStatus.Submitted => ("Submitted", "yellow"),
                RegistrationStatus.Pending => ("Pending", "orange"),
                RegistrationStatus.Approved => ("Approved", "green"),
                RegistrationStatus.Rejected => ("Rejected", "red"),
                _ => ("Unknown", "grey")
            };
        }

        private static List<OperatorListItemDto> SortOperators(
            List<OperatorListItemDto> operators,
            string? sortBy,
            string sortOrder)
        {
            var isDesc = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

            // Fixed column sorting
            if (string.IsNullOrWhiteSpace(sortBy) || sortBy.Equals(
                "registeredAt", StringComparison.OrdinalIgnoreCase))
            {
                return isDesc
                    ? operators.OrderByDescending(o => o.RegisteredAt).ToList()
                    : operators.OrderBy(o => o.RegisteredAt).ToList();
            }

            if (sortBy.Equals("status", StringComparison.OrdinalIgnoreCase))
            {
                return isDesc
                    ? operators.OrderByDescending(o => o.Status).ToList()
                    : operators.OrderBy(o => o.Status).ToList();
            }

            // Dynamic column sorting — sort by field value
            var fieldName = sortBy.Trim().ToLower();

            return isDesc
                ? operators.OrderByDescending(o =>
                    o.Values.GetValueOrDefault(fieldName, "") ?? "").ToList()
                : operators.OrderBy(o =>
                    o.Values.GetValueOrDefault(fieldName, "") ?? "").ToList();
        }
    }
}
