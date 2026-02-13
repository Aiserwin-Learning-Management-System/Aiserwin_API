using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{/// <summary>
 /// Provides business operations for <see cref="StudentAcademicdetails"/> entities.
 /// </summary>
    public sealed class StudentAcademicdetailsService : IStudentAcademicdetailsService
    {
        private readonly IStudentAcademicdeatilsRepository _repository;
        private readonly ILogger<StateService> _logger;
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAcademicdetailsService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="countryRepository">countryRepository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StudentAcademicdetailsService(IStudentAcademicdeatilsRepository repository, ILogger<StateService> logger, ICountryRepository countryRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentAcademicdetailsDto.</returns>
        public async Task<IReadOnlyList<StudentAcademicdetailsDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Student Academic details");
            var studentAcademicDetails = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} states", studentAcademicDetails.Count());
            return studentAcademicDetails.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentAcademicdetailsDto.</returns>
        public async Task<StudentAcademicdetailsDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching state by Id: {StudentacademicdetailsId}", id);
            var studentAcademicDetails = await _repository.GetByIdAsync(id);
            _logger.LogInformation("StudentAcademicdetails fetched successfully for Id: {StudentacademicdetailsId}", id);
            return studentAcademicDetails == null ? null : Map(studentAcademicDetails);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StudentAcademicdetailsDto> CreateAsync(StudentAcademicdetailsRequest request)
        {
            var country = await _countryRepository.GetByIdAsync(request.countryId);

            if (country == null)
            {
                throw new InvalidOperationException("Country not found");
            }

            if (!country.IsActive)
            {
                throw new InvalidOperationException("Cannot create with inactive country");
            }

            var modeofstudy = await _modeOfStudyRepository.GetByIdAsync(request.countryId);

            if (modeofstudy == null)
            {
                throw new InvalidOperationException("modeofstudy not found");
            }

            if (!modeofstudy.IsActive)
            {
                throw new InvalidOperationException("Cannot create with inactive modeofstudy");
            }

            var state = new StudentAcademicDetails
            {
                CountryId = request.countryId,
            };

            var created = await _repository.AddAsync(state);
            _logger.LogInformation(
           "State created successfully. StateId: {StateId}",
           created.Id);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">State not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, StudentAcademicdetailsRequest request)
        {
            _logger.LogInformation("Updating state Id: {StateId}", id);
            var state = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("State not found");

            state.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(state);
            _logger.LogInformation(
           "State updated successfully. StateId: {StateId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting state Id: {StateId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "State deleted successfully. StateId: {StateId}",
           id);
        }

        private static StudentAcademicdetailsDto Map(StudentAcademicDetails c) =>
     new StudentAcademicdetailsDto
     {
         Id = c.Id,
         CountryId = c.CountryId,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedAt = c.UpdatedAt,
         UpdatedBy = c.UpdatedBy,

     };
    }
}
