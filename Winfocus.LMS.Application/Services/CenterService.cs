using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Centre"/> entities.
    /// </summary>
    public sealed class CentreService : ICentreService
    {
        private readonly ICentreRepository _repository;
        private readonly ILogger<CentreService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger instance.</param>
        public CentreService(ICentreRepository repository, ILogger<CentreService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CeneterDto.</returns>
        public async Task<CommonResponse<List<CentreDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Ceneters");
            var centres = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} centres", centres.Count());
            var res = centres.Select(Map).ToList();
            if (res.Any())
            {
                return CommonResponse<List<CentreDto>>.SuccessResponse("fetching all centers", res);
            }
            else
            {
                return CommonResponse<List<CentreDto>>.FailureResponse("no centers found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CenterDto.</returns>
        public async Task<CommonResponse<CentreDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching centre by Id: {CentreId}", id);
            var centre = Map(await _repository.GetByIdAsync(id));
            if (centre == null)
            {
                _logger.LogWarning("Centre not found for Id: {CentreId}", id);
                return CommonResponse<CentreDto>.FailureResponse("no centers found");
            }
            else
            {
                _logger.LogInformation("Centre fetched successfully for Id: {CentreId}", id);
                return CommonResponse<CentreDto>.SuccessResponse("fetching all centers", centre);
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CenterDto.</returns>
        /// <exception cref="InvalidOperationException">Center code already exists.</exception>
        public async Task<CommonResponse<CentreDto>> CreateAsync(CenterRequestDto request)
        {
            var centre = new Centre
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                ModeOfStudyId = request.modeofstudy,
            };

            var created = await _repository.AddAsync(centre);
            _logger.LogInformation("Centre created successfully. CentreId: {CentreId}", created.Id);
            var mapped = Map(created);
            if (mapped == null)
            {
                return CommonResponse<CentreDto>.FailureResponse("Failed to create center");
            }
            else
            {
                return CommonResponse<CentreDto>.SuccessResponse("center created successfully", mapped);
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Center not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<CentreDto>> UpdateAsync(Guid id, CenterRequestDto request)
        {
            _logger.LogInformation("Updating centre Id: {CentreId}", id);
            var center = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Center not found");

            center.Name = request.name;
            center.UpdatedAt = DateTime.UtcNow;
            center.UpdatedBy = request.userId;

            var data = await _repository.UpdateAsync(center);
            _logger.LogInformation("Centre updated successfully. CentreId: {CentreId}", id);
            var mapped = Map(data);
            if (mapped == null)
            {
                return CommonResponse<CentreDto>.FailureResponse("Failed to create center");
            }
            else
            {
                return CommonResponse<CentreDto>.SuccessResponse("center created successfully", mapped);
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting centre Id: {CentreId}", id);
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets centre by mode of study and state.
        /// </summary>
        /// <param name="modeofid">Mode of study identifier.</param>
        /// <param name="stateid">State identifier.</param>
        /// <returns>CentreDto if found; otherwise null.</returns>
        public async Task<CommonResponse<CentreDto>> GetByFilterAsync(Guid modeofid, Guid stateid)
        {
            _logger.LogInformation(
                "Fetching centre for ModeOfStudyId: {ModeOfStudyId}, StateId: {StateId}",
                modeofid, stateid);

            var centre = await _repository.GetByFilterAsync(modeofid, stateid);

            if (centre == null)
            {
                _logger.LogWarning(
                    "Centre not found for ModeOfStudyId: {ModeOfStudyId}, StateId: {StateId}",
                    modeofid, stateid);

                return CommonResponse<CentreDto>.FailureResponse("Not data found");
            }

            _logger.LogInformation(
                "Centre fetched successfully for ModeOfStudyId: {ModeOfStudyId}, StateId: {StateId}",
                modeofid, stateid);

            var mapped = Map(centre);
            return CommonResponse<CentreDto>.SuccessResponse("Centre fetched successfully", mapped);
        }

        private static CentreDto Map(Centre c)
        {
            return new CentreDto(
                c.Id,
                c.Name,
                c.CenterType.ToString()
            );
        }
    }
}
