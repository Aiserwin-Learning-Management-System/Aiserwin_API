using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Centre"/> entities.
    /// </summary>
    public sealed class CentreService : ICenterService
    {
        private readonly ICentreRepository _repository;
        private readonly IModeOfStudyRepository _moderepository;
        private readonly ILogger<CentreService> _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="moderepository">moderepository instance.</param>
        /// <param name="countryRepository">countryRepository instance.</param>
        /// <param name="stateRepository">stateRepository instance.</param>
        public CentreService(ICentreRepository repository, ILogger<CentreService> logger, IModeOfStudyRepository moderepository, ICountryRepository countryRepository, IStateRepository stateRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _moderepository = moderepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CeneterDto.</returns>
        public async Task<CommonResponse<List<CenterDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all Ceneters");
                var centres = await _repository.GetAllAsync();
                _logger.LogInformation("Fetched {Count} centres", centres.Count());
                var res = centres.Select(Map).ToList();
                if (res.Any())
                {
                    return CommonResponse<List<CenterDto>>.SuccessResponse("fetching all centers", res);
                }
                else
                {
                    return CommonResponse<List<CenterDto>>.FailureResponse("no centers found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching centers.");
                return CommonResponse<List<CenterDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CenterDto.</returns>
        public async Task<CommonResponse<CenterDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching centre by Id: {CentreId}", id);
            var centre = Map(await _repository.GetByIdAsync(id));
            if (centre == null)
            {
                _logger.LogWarning("Centre not found for Id: {CentreId}", id);
                return CommonResponse<CenterDto>.FailureResponse("no centers found");
            }
            else
            {
                _logger.LogInformation("Centre fetched successfully for Id: {CentreId}", id);
                return CommonResponse<CenterDto>.SuccessResponse("fetching all centers", centre);
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        /// <exception cref="InvalidOperationException">Center code already exists.</exception>
        public async Task<CommonResponse<CenterDto>> CreateAsync(CenterRequestDto request)
        {
            var country = await _countryRepository.GetByIdAsync(request.countryid);
            if (country == null)
                return CommonResponse<CenterDto>.FailureResponse("Country not found");

            if (!country.IsActive)
                return CommonResponse<CenterDto>.FailureResponse("Cannot create with inactive country");

            var modeOfStudy = await _moderepository.GetByIdAsync(request.modeofstudyid);
            if (modeOfStudy == null)
                return CommonResponse<CenterDto>.FailureResponse("Mode of study not found");

            State? state = null;

            var modeName = modeOfStudy.Name.Trim().ToLower();

            if (modeName == "offline" || modeName == "hybrid")
            {
                if (request.stateid != Guid.Empty)
                    return CommonResponse<CenterDto>
                        .FailureResponse("State is required for Offline mode");

                state = await _stateRepository.GetByIdAsync(request.stateid);
                if (state == null)
                    return CommonResponse<CenterDto>
                        .FailureResponse("State not found");
            }

            if (modeName == "online")
            {
                request = request with { stateid = Guid.Empty };
            }

            var centre = new Center
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                ModeOfStudyId = request.modeofstudyid,
                CountryId = request.countryid,
                StateId = request.stateid,
            };

            var created = await _repository.AddAsync(centre);

            return CommonResponse<CenterDto>
                .SuccessResponse("Center created successfully", Map(created));
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Center not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<CenterDto>> UpdateAsync(Guid id, CenterRequestDto request)
        {
            _logger.LogInformation("Updating centre Id: {CentreId}", id);

            var center = await _repository.GetByIdAsync(id);
            if (center == null)
                return CommonResponse<CenterDto>.FailureResponse("Center not found");

            var country = await _countryRepository.GetByIdAsync(request.countryid);
            if (country == null)
                return CommonResponse<CenterDto>.FailureResponse("Country not found");

            if (!country.IsActive)
                return CommonResponse<CenterDto>.FailureResponse("Cannot update with inactive country");

            // 🔹 Mode validation
            var modeOfStudy = await _moderepository.GetByIdAsync(request.modeofstudyid);
            if (modeOfStudy == null)
                return CommonResponse<CenterDto>.FailureResponse("Mode of study not found");

            State? state = null;
            var modeName = modeOfStudy.Name.Trim().ToLower();

            if (modeName == "offline" || modeName == "hybrid")
            {
                if (request.stateid == Guid.Empty)
                    return CommonResponse<CenterDto>
                        .FailureResponse("State is required for Offline mode");

                state = await _stateRepository.GetByIdAsync(request.stateid);
                if (state == null)
                    return CommonResponse<CenterDto>
                        .FailureResponse("State not found");
            }

            if (modeName == "online")
            {
                request = request with { stateid = Guid.Empty };
            }

            center.Name = request.name;
            center.CountryId = request.countryid;
            center.ModeOfStudyId = request.modeofstudyid;
            center.StateId = request.stateid;
            center.UpdatedAt = DateTime.UtcNow;
            center.UpdatedBy = request.userId;

            var updated = await _repository.UpdateAsync(center);

            _logger.LogInformation("Centre updated successfully. CentreId: {CentreId}", id);

            return CommonResponse<CenterDto>
                .SuccessResponse("Center updated successfully", Map(updated));
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
                _logger.LogInformation("Deleting center Id: {Id}", id);
                var result = await _repository.DeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Center deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Center not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Center Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="countryId">country identifier.</param>
        /// <param name="modeOfStudyId">Mode of study identifier.</param>
        /// <param name="stateId">State identifier.</param>
        /// <returns>CentreDto if found; otherwise null.</returns>
        public async Task<CommonResponse<List<CenterDto>>> GetByFilterAsync(
      Guid? countryId,
      Guid? modeOfStudyId,
      Guid? stateId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching centres for CountryId: {CountryId}, ModeOfStudyId: {ModeOfStudyId}, StateId: {StateId}",
                    countryId, modeOfStudyId, stateId);

                var centres = await _repository.GetByFilterAsync(
                    countryId, modeOfStudyId, stateId);

                if (centres == null || !centres.Any())
                {
                    _logger.LogWarning(
                        "No centres found for CountryId: {CountryId}, ModeOfStudyId: {ModeOfStudyId}, StateId: {StateId}",
                        countryId, modeOfStudyId, stateId);

                    return CommonResponse<List<CenterDto>>
                        .FailureResponse("No data found");
                }

                var mapped = centres.Select(Map).ToList();

                _logger.LogInformation(
                    "Centres fetched successfully. Count: {Count}",
                    mapped.Count);

                return CommonResponse<List<CenterDto>>
                    .SuccessResponse("Centres fetched successfully", mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while fetching centres with filters");

                return CommonResponse<List<CenterDto>>
                    .FailureResponse("An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Gets filtered Center with pagination support.
        /// Search works on mode of study name, state Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated Center result.</returns>
        public async Task<CommonResponse<PagedResult<CenterDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Center. Filters => Active:{Active}, " +
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
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.modeOfStudy.Name.ToLower().Contains(searchTerm) ||
                        x.State.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<CenterDto>>.SuccessResponse(
                        "No mode of study found.",
                        new PagedResult<CenterDto>(
                            new List<CenterDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),
                    "modeofstudyname" => isDesc ? query.OrderByDescending(x => x.modeOfStudy.Name)
                                             : query.OrderBy(x => x.modeOfStudy.Name),

                    "statename" => isDesc ? query.OrderByDescending(x => x.State.Name)
                                             : query.OrderBy(x => x.State.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                            : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var center = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = center.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} center",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<CenterDto>>.SuccessResponse(
                    "center fetched successfully.",
                    new PagedResult<CenterDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered centers.");
                return CommonResponse<PagedResult<CenterDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static CenterDto Map(Center c) =>
         new CenterDto
         {
             Id = c.Id,
             Name = c.Name,
             CenterType = c.CenterType,
             IsActive = c.IsActive,
             modeOfStudy = c.modeOfStudy == null ? null : new ModeOfStudyDto
             {
                 Id = c.ModeOfStudyId,
                 Name = c.modeOfStudy.Name,
                 IsActive = c.modeOfStudy.IsActive,
             },
             Country = c.Country == null ? null : new CountryDto
             {
                 Id = c.Country.Id,
                 Name = c.Country.Name,
             },
             State = c.State == null ? null : new StateDto
             {
                 Id = c.State.Id,
                 Name = c.State.Name,
                 IsActive = c.State.IsActive,

             },
         };
    }
}
