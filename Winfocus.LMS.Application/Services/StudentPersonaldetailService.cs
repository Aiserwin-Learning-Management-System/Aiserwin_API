using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="StudentPersonaldetails"/> entities.
    /// </summary>
    public sealed class StudentPersonaldetailService : IStudentPersonaldetailsService
    {
        private readonly IStudentPersonaldetailsRepository _repository;
        private readonly ILogger<StateService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentPersonaldetailService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StudentPersonaldetailService(IStudentPersonaldetailsRepository repository, ILogger<StateService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentPersonaldetailsdto.</returns>
        public async Task<IReadOnlyList<StudentPersonaldetailsdto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Student personal details");
            var studentPersonalDetails = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} states", studentPersonalDetails.Count());
            return studentPersonalDetails.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentPersonaldetailsdto.</returns>
        public async Task<StudentPersonaldetailsdto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching state by Id: {StudentpersonaldetailsId}", id);
            var studentPersonalDetails = await _repository.GetByIdAsync(id);
            _logger.LogInformation("studentPersonalDetails fetched successfully for Id: {StudentacademicdetailsId}", id);
            return studentPersonalDetails == null ? null : Map(studentPersonalDetails);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentPersonaldetailsdto.</returns>
        public async Task<StudentPersonaldetailsdto> CreateAsync(StudentPersonaldetailsRequest request)
        {
            var studentPersonalDetails = new StudentPersonalDetails
            {
                FullName = request.fullname,
                EmailAddress = request.emailaddress,
                DOB = request.dob,
                MobileBotim = request.mobilebotim,
                MobileWhatsapp = request.mobilewhatsapp,
                MobileComera = request.mobilecomera,
                AreaName = request.areaname,
                DistrictOrLocation = request.districtorlocation,
                Emirates = request.emirates,
            };

            var created = await _repository.AddAsync(studentPersonalDetails);
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
        public async Task UpdateAsync(Guid id, StudentPersonaldetailsRequest request)
        {
            _logger.LogInformation("Updating StudentPersonaldetails Id: {StudentPersonaldetails}", id);
            var studentPersonal = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("StudentPersonaldetails not found");

            studentPersonal.FullName = request.fullname;
            studentPersonal.EmailAddress = request.emailaddress;
            studentPersonal.DOB = request.dob;
            studentPersonal.MobileBotim = request.mobilebotim;
            studentPersonal.MobileWhatsapp = request.mobilewhatsapp;
            studentPersonal.MobileComera = request.mobilecomera;
            studentPersonal.AreaName = request.areaname;
            studentPersonal.DistrictOrLocation = request.districtorlocation;
            studentPersonal.Emirates = request.emirates;

            await _repository.UpdateAsync(studentPersonal);
            _logger.LogInformation(
           "studentPersonal updated successfully. studentPersonalId: {studentPersonalId}",
           id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting studentPersonal Id: {studentPersonalId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "studentPersonal deleted successfully. studentPersonalId: {studentPersonalId}",
           id);
        }

        private static StudentPersonaldetailsdto Map(StudentPersonalDetails c) =>
    new StudentPersonaldetailsdto
    {
        Id = c.Id,
        FullName = c.FullName,
        EmailAddress = c.EmailAddress,
        DOB = c.DOB,
        MobileBotim = c.MobileBotim,
        MobileComera = c.MobileComera,
        MobileWhatsapp = c.MobileWhatsapp,
        AreaName = c.AreaName,
        DistrictOrLocation = c.DistrictOrLocation,
        Emirates = c.Emirates,

        CreatedBy = c.CreatedBy,
        CreatedAt = c.CreatedAt,
        UpdatedBy = c.UpdatedBy,
        UpdatedAt = c.UpdatedAt,
    };
    }
}
