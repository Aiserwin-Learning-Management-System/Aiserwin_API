using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

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
