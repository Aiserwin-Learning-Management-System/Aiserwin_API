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
    /// GradeService.
    /// </summary>
    public sealed class GradeService
    {
        private readonly IGradeRepository _repository;
        private readonly ILogger<GradeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public GradeService(
            IGradeRepository repository,
            ILogger<GradeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>GradeDto.</returns>
        public async Task<IReadOnlyList<GradeDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all syllabuses");
            var grades = await _repository.GetAllAsync();
            return grades.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<GradeDto?> GetByIdAsync(Guid id)
        {
            var grades = await _repository.GetByIdAsync(id);
            return grades == null ? null : Map(grades);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        /// <exception cref="InvalidOperationException">grade code already exists.</exception>
        public async Task<GradeDto> CreateAsync(GradeRequest request)
        {
            if (await _repository.ExistsByCodeAsync(request.code))
            {
                throw new InvalidOperationException("Grade code already exists");
            }

            var grades = new Grade
            {
                GradeName = request.name,
                GradeCode = request.code,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(grades);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Grade not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, GradeRequest request)
        {
            var grade = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Grade not found");

            grade.GradeName = request.name;
            grade.GradeCode = request.code;
            grade.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(grade);
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
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<GradeDto?> GetBySyllabusIdAsync(Guid syllabusid)
        {
            var grades = await _repository.GetBySyllabusIdAsync(syllabusid);
            return grades == null ? null : Map(grades);
        }

        private static GradeDto Map(Grade c) =>
           new GradeDto
           {
               Id = c.Id,
               GradeName = c.GradeName,
               GradeCode = c.GradeCode,
               SyllabusId = c.SyllabusId,
           };

    }
}
