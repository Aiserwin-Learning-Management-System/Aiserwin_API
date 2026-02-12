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
        public async Task<IReadOnlyList<SyllabusDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all syllabuses");
            var syllabuses = await _repository.GetAllAsync();
            return syllabuses.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SyllabusDto.</returns>
        public async Task<SyllabusDto?> GetByIdAsync(Guid id)
        {
            var syllabus = await _repository.GetByIdAsync(id);
            return syllabus == null ? null : Map(syllabus);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>SyllabusDto.</returns>
        /// <exception cref="InvalidOperationException">syllabus code already exists.</exception>
        public async Task<SyllabusDto> CreateAsync(SyllabusRequest request)
        {
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                throw new InvalidOperationException("Syllabus code already exists");
            }

            var syllabus = new Syllabus
            {
                SyllabusName = request.name,
                SyllabusCode = request.code,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                CenterId = request.centerid,
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
        public async Task UpdateAsync(Guid id, SyllabusRequest request)
        {
            var syllabus = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("syllabus not found");

            syllabus.SyllabusName = request.name;
            syllabus.SyllabusCode = request.code;
            syllabus.UpdatedBy = request.userId;
            syllabus.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(syllabus);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
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
         SyllabusName = c.SyllabusName,
         SyllabusCode = c.SyllabusCode,
         CenterId = c.CenterId,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedBy = c.UpdatedBy,
         UpdatedAt = c.UpdatedAt,
         Center = c.Center == null ? null : new CenterDto1
         {
             Id = c.Center.Id,
             Name = c.Center.Name,
             Type = c.Center.CenterType,
             CountryId = c.Center.CountryId,
             Country =  null
         }
     };

    }
}
