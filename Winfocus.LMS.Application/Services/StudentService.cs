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
{
    /// <summary>
    /// Provides business operations for <see cref="Student"/> entities.
    /// </summary>
    public sealed class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger<StateService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StudentService(IStudentRepository repository, ILogger<StateService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StateDto.</returns>
        public async Task<IReadOnlyList<StudentDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto.</returns>
        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            return student == null ? null : Map(student);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StudentDto> CreateAsync(StudentDto request)
        {
            var student = new Student
            {
                StudentAcademicDetailsId = request.StudentAcademicId,
                StudentDocumentsId = request.StudentDocumentsId,
                StudentPersonalDetailsId = request.StudentPersonalId,
                CreatedBy = request.Userid,
                CreatedAt = DateTime.UtcNow,
                Status = request.Status,
                RegistrationNumber = request.RegistraionNumber,
            };

            var created = await _repository.AddAsync(student);
            return Mapstud(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Student not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, StudentDto request)
        {
            var student = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Student not found");

            student.StudentAcademicDetailsId = request.StudentAcademicId;
            student.StudentDocumentsId = request.StudentDocumentsId;
            student.StudentPersonalDetailsId = request.StudentPersonalId;
            student.UpdatedBy = request.Userid;
            student.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(student);
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

        private static StudentDto Map(Student c) =>
     new StudentDto
     {
         Id = c.Id,
         AcademicDetails = new StudentAcademicdetailsDto
         {
             Id = c.StudentAcademicDetailsId,
             CountryId = c.AcademicDetails.CountryId,
             Country = new CountryDto1
             {
                 Id = c.AcademicDetails.CountryId,
                 Name = c.AcademicDetails.Country.Name,
             },
             StateId = c.AcademicDetails.StateId,
             State = new StateDto
             {
                 Id = c.AcademicDetails.StateId,
                 StateName = c.AcademicDetails.State.StateName,
             },
             ModeOfStudyId = c.AcademicDetails.ModeOfStudyId,
             ModeOfStudy = new ModeOfStudyDto
             {
                 Id = c.AcademicDetails.ModeOfStudyId,
                 ModeName = c.AcademicDetails.ModeOfStudy.ModeName,
             },
             CenterId = c.AcademicDetails.CenterId,
             Center = new CenterDto1
              {
                    Id = c.AcademicDetails.CenterId,
                    Name = c.AcademicDetails.Center.Name,
              },
             SyllabusId = c.AcademicDetails.SyllabusId,
             Syllabus = new SyllabusDto
                {
                    Id = c.AcademicDetails.SyllabusId,
                    SyllabusName = c.AcademicDetails.Syllabus.SyllabusName,
                },
             GradeId = c.AcademicDetails.GradeId,
             Grade = new GradeDto
              {
                    Id = c.AcademicDetails.GradeId,
                    GradeName = c.AcademicDetails.Grade.GradeName,
              },
             StreamId = c.AcademicDetails.StreamId,
             Stream = new StreamDto
                {
                        Id = c.AcademicDetails.StreamId,
                        StreamName = c.AcademicDetails.Stream.StreamName,
                },
             SubjectId = c.AcademicDetails.SubjectId,
             Subject = new SubjectDto
               {
                        Id = c.AcademicDetails.SubjectId,
                        SubjectName = c.AcademicDetails.Subject.SubjectName,
               },
             PastYearPerformance = c.AcademicDetails.PastYearPerformance,
             PastSchoolLocation = c.AcademicDetails.PastSchoolLocation,
             PastSchoolName = c.AcademicDetails.PastSchoolName,
             Emirates = c.AcademicDetails.Emirates,
         },
         PersonalDetails = new StudentPersonaldetailsdto
         {
             Id = c.StudentPersonalDetailsId,
             FullName = c.StudentPersonalDetails.FullName,
             EmailAddress = c.StudentPersonalDetails.EmailAddress,
             DOB = c.StudentPersonalDetails.DOB,
             MobileWhatsapp = c.StudentPersonalDetails.MobileWhatsapp,
             MobileBotim = c.StudentPersonalDetails.MobileBotim,
             MobileComera = c.StudentPersonalDetails.MobileComera,
             AreaName = c.StudentPersonalDetails.AreaName,
             DistrictOrLocation = c.StudentPersonalDetails.DistrictOrLocation,
             Emirates = c.StudentPersonalDetails.Emirates,
         },
         StudentDocuments = new StudentDocumentsDto
         {
             Id = c.StudentDocumentsId,
             StudentPhoto = c.StudentDocuments.StudentPhotoPath,
             StudentSignature = c.StudentDocuments.StudentSignaturePath,
             IsAcceptedAgreement = c.StudentDocuments.IsAcceptedAgreement,
             IsAcceptedTermsAndConditions = c.StudentDocuments.IsAcceptedTermsAndConditions,
         },
         RegistraionNumber = c.RegistrationNumber,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedAt = c.UpdatedAt,
         UpdatedBy = c.UpdatedBy,
     };

        private static StudentDto Mapstud(Student c) =>
 new StudentDto
 {
     Id = c.Id,
     StudentAcademicId = c.StudentAcademicDetailsId,
     StudentPersonalId = c.StudentPersonalDetailsId,
     StudentDocumentsId = c.StudentDocumentsId,
     RegistraionNumber = c.RegistrationNumber,
     CreatedBy = c.CreatedBy,
     CreatedAt = c.CreatedAt,
     UpdatedAt = c.UpdatedAt,
     UpdatedBy = c.UpdatedBy,
 };
    }
}
