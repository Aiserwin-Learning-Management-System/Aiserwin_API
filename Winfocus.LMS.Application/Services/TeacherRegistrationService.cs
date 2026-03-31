using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Teacher;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business logic for managing teacher registrations.
    /// </summary>
    public class TeacherRegistrationService : ITeacherRegistrationService
    {
        private readonly ITeacherRegistrationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRegistrationService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for persistence operations.</param>
        public TeacherRegistrationService(ITeacherRegistrationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new teacher registration from the provided request.
        /// </summary>
        /// <param name="request">Request DTO containing teacher details.</param>
        /// <returns>CommonResponse containing the created teacher DTO on success.</returns>
        public async Task<CommonResponse<TeacherRegistrationDto>> CreateAsync(TeacherRegistrationRequest request)
        {
            if (request == null)
            {
                return CommonResponse<TeacherRegistrationDto>.FailureResponse("Invalid request.");
            }

            var entity = new TeacherRegistration
            {
                EmploymentTypeId = request.EmploymentTypeId,
                WorkMode = (Winfocus.LMS.Domain.Enums.WorkMode)request.WorkMode,
                DateOfJoining = request.DateOfJoining,
                ReportingManagerId = request.ReportingManagerId,
                FullName = request.FullName,
                Gender = (Winfocus.LMS.Domain.Enums.Gender)request.Gender,
                DateOfBirth = request.DateOfBirth,
                Nationality = request.Nationality,
                MobileNumber = request.MobileNumber,
                EmailAddress = request.EmailAddress,
                EmergencyContactNumber = request.EmergencyContactNumber,
                Address = request.Address,
                PermanentAddress = request.PermanentAddress,
                IsWillingToWorkWeekends = request.IsWillingToWorkWeekends,
                HasInternetAndSystemAvailability = request.HasInternetAndSystemAvailability,
                IsTermsAccepted = request.IsTermsAccepted,
                DeclarationDate = request.DeclarationDate,
                ProfessionalDetail = request.ProfessionalDetail == null ? null : new TeacherProfessionalDetail
                {
                    HighestQualification = request.ProfessionalDetail.HighestQualification,
                    University = request.ProfessionalDetail.University,
                    YearOfPassing = request.ProfessionalDetail.YearOfPassing,
                    HasTeachingCertification = request.ProfessionalDetail.HasTeachingCertification,
                    AdditionalCourses = request.ProfessionalDetail.AdditionalCourses,
                    TotalTeachingExperience = request.ProfessionalDetail.TotalTeachingExperience,
                    HasOnlineTeachingExperience = request.ProfessionalDetail.HasOnlineTeachingExperience,
                    HasOfflineTeachingExperience = request.ProfessionalDetail.HasOfflineTeachingExperience,
                    IsAvailableForDemoClass = request.ProfessionalDetail.IsAvailableForDemoClass,
                    ComputerLiteracy = (Winfocus.LMS.Domain.Enums.ComputerLiteracy)request.ProfessionalDetail.ComputerLiteracy
                },
                Schedule = request.Schedule == null ? null : new TeacherSchedule
                {
                    Availabilities = request.Schedule.Availabilities == null ? new System.Collections.Generic.List<TeacherAvailability>() : request.Schedule.Availabilities.Select(a => new TeacherAvailability
                    {
                        Day = (System.DayOfWeek)a.Day,
                        StartTime = TimeSpan.Parse(a.StartTime),
                        EndTime = TimeSpan.Parse(a.EndTime)
                    }).ToList()
                },
                Documents = request.Documents == null ? null : new TeacherDocumentInfo
                {
                    PhotoPath = request.Documents.PhotoPath,
                    IdCardPath = request.Documents.IdCardPath
                },
                WorkHistory = request.WorkHistory == null ? new System.Collections.Generic.List<TeacherWorkHistory>() : request.WorkHistory.Select(w => new TeacherWorkHistory
                {
                    Duration = w.Duration,
                    JobProfile = w.JobProfile,
                    Institution = w.Institution,
                    Location = w.Location,
                    ReasonForLeaving = w.ReasonForLeaving,
                    EmploymentStatus = w.EmploymentStatus
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
            };

            // generate a simple employee id - keep deterministic and unique enough for now
            entity.EmployeeId = $"TCH-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var created = await _repository.AddAsync(entity);

            var dto = new TeacherRegistrationDto
            {
                Id = created.Id,
                EmployeeId = created.EmployeeId,
                FullName = created.FullName,
                EmailAddress = created.EmailAddress,
                MobileNumber = created.MobileNumber,
                DateOfBirth = created.DateOfBirth,
                DateOfJoining = created.DateOfJoining,
                Nationality = created.Nationality,
                EmergencyContactNumber = created.EmergencyContactNumber,
                PermanentAddress = created.PermanentAddress,
                IsWillingToWorkWeekends = created.IsWillingToWorkWeekends,
                HasInternetAndSystemAvailability = created.HasInternetAndSystemAvailability,
                IsTermsAccepted = created.IsTermsAccepted,
                DeclarationDate = created.DeclarationDate,
                Status = (int)created.Status,
                AdministrativeRemarks = created.AdministrativeRemarks,
                ReportingManagerId = created.ReportingManagerId,
            };

            return CommonResponse<TeacherRegistrationDto>.SuccessResponse("Teacher registered successfully.", dto);
        }

        /// <summary>
        /// Gets a teacher registration by identifier.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>CommonResponse containing the teacher DTO when found.</returns>
        public async Task<CommonResponse<TeacherRegistrationDto>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return CommonResponse<TeacherRegistrationDto>.FailureResponse("Teacher not found.");
            }

            var dto = new TeacherRegistrationDto
            {
                Id = entity.Id,
                EmployeeId = entity.EmployeeId,
                FullName = entity.FullName,
                EmailAddress = entity.EmailAddress,
                MobileNumber = entity.MobileNumber,
                DateOfBirth = entity.DateOfBirth,
                IsTermsAccepted = entity.IsTermsAccepted,
                //IsDeclarationAccepted = entity.IsDeclarationAccepted,
            };

            return CommonResponse<TeacherRegistrationDto>.SuccessResponse("Success", dto);
        }

        /// <summary>
        /// Retrieves all teacher registrations.
        /// </summary>
        /// <returns>CommonResponse containing list of teacher DTOs.</returns>
        public async Task<CommonResponse<List<TeacherRegistrationDto>>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            var dtos = list.Select(x => new TeacherRegistrationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FullName = x.FullName,
                EmailAddress = x.EmailAddress,
                MobileNumber = x.MobileNumber,
                DateOfBirth = x.DateOfBirth,
                IsTermsAccepted = x.IsTermsAccepted,
                //IsDeclarationAccepted = x.IsDeclarationAccepted,
            }).ToList();

            return CommonResponse<List<TeacherRegistrationDto>>.SuccessResponse("Success", dtos);
        }

        /// <summary>
        /// Updates an existing teacher registration.
        /// </summary>
        /// <param name="id">Identifier of the teacher to update.</param>
        /// <param name="request">Request DTO containing updated values.</param>
        /// <returns>CommonResponse containing the updated teacher DTO.</returns>
        public async Task<CommonResponse<TeacherRegistrationDto>> UpdateAsync(Guid id, TeacherRegistrationRequest request)
        {
            if (request == null)
                return CommonResponse<TeacherRegistrationDto>.FailureResponse("Invalid request.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return CommonResponse<TeacherRegistrationDto>.FailureResponse("Teacher not found.");

            existing.EmploymentTypeId = request.EmploymentTypeId;
            existing.WorkMode = (Winfocus.LMS.Domain.Enums.WorkMode)request.WorkMode;
            existing.DateOfJoining = request.DateOfJoining;
            existing.ReportingManagerId = request.ReportingManagerId;
            existing.FullName = request.FullName;
            existing.Gender = (Winfocus.LMS.Domain.Enums.Gender)request.Gender;
            existing.DateOfBirth = request.DateOfBirth;
            existing.Nationality = request.Nationality;
            existing.MobileNumber = request.MobileNumber;
            existing.EmailAddress = request.EmailAddress;
            existing.EmergencyContactNumber = request.EmergencyContactNumber;
            existing.Address = request.Address;
            existing.PermanentAddress = request.PermanentAddress;
            existing.IsWillingToWorkWeekends = request.IsWillingToWorkWeekends;
            existing.HasInternetAndSystemAvailability = request.HasInternetAndSystemAvailability;
            existing.IsTermsAccepted = request.IsTermsAccepted;
            existing.DeclarationDate = request.DeclarationDate;

            // Map nested objects if provided
            if (request.ProfessionalDetail != null)
            {
                existing.ProfessionalDetail = new TeacherProfessionalDetail
                {
                    HighestQualification = request.ProfessionalDetail.HighestQualification,
                    University = request.ProfessionalDetail.University,
                    YearOfPassing = request.ProfessionalDetail.YearOfPassing,
                    HasTeachingCertification = request.ProfessionalDetail.HasTeachingCertification,
                    AdditionalCourses = request.ProfessionalDetail.AdditionalCourses,
                    TotalTeachingExperience = request.ProfessionalDetail.TotalTeachingExperience,
                    HasOnlineTeachingExperience = request.ProfessionalDetail.HasOnlineTeachingExperience,
                    HasOfflineTeachingExperience = request.ProfessionalDetail.HasOfflineTeachingExperience,
                    IsAvailableForDemoClass = request.ProfessionalDetail.IsAvailableForDemoClass,
                    ComputerLiteracy = (Winfocus.LMS.Domain.Enums.ComputerLiteracy)request.ProfessionalDetail.ComputerLiteracy
                };
            }

            if (request.Schedule != null)
            {
                existing.Schedule = new TeacherSchedule
                {
                    Availabilities = request.Schedule.Availabilities == null ? new System.Collections.Generic.List<TeacherAvailability>() : request.Schedule.Availabilities.Select(a => new TeacherAvailability
                    {
                        Day = (System.DayOfWeek)a.Day,
                        StartTime = TimeSpan.Parse(a.StartTime),
                        EndTime = TimeSpan.Parse(a.EndTime)
                    }).ToList()
                };
            }

            if (request.Documents != null)
            {
                existing.Documents = new TeacherDocumentInfo
                {
                    PhotoPath = request.Documents.PhotoPath,
                    IdCardPath = request.Documents.IdCardPath,
                };
            }

            if (request.WorkHistory != null)
            {
                existing.WorkHistory = request.WorkHistory.Select(w => new TeacherWorkHistory
                {
                    Duration = w.Duration,
                    JobProfile = w.JobProfile,
                    Institution = w.Institution,
                    Location = w.Location,
                    ReasonForLeaving = w.ReasonForLeaving,
                    EmploymentStatus = w.EmploymentStatus,
                }).ToList();
            }

            var updated = await _repository.UpdateAsync(existing);
            if (updated == null)
                return CommonResponse<TeacherRegistrationDto>.FailureResponse("Failed to update teacher.");

            var dto = new TeacherRegistrationDto
            {
                Id = updated.Id,
                EmployeeId = updated.EmployeeId,
                FullName = updated.FullName,
                EmailAddress = updated.EmailAddress,
                MobileNumber = updated.MobileNumber,
                DateOfBirth = updated.DateOfBirth,
                IsTermsAccepted = updated.IsTermsAccepted,
                //IsDeclarationAccepted = updated.IsDeclarationAccepted,
            };

            return CommonResponse<TeacherRegistrationDto>.SuccessResponse("Teacher updated successfully.", dto);
        }

        /// <summary>
        /// Performs a soft delete of the teacher registration identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Identifier of the teacher to soft delete.</param>
        /// <returns>CommonResponse indicating success or failure.</returns>
        public async Task<CommonResponse<bool>> SoftDeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return CommonResponse<bool>.FailureResponse("Teacher not found.");

            var result = await _repository.SoftDeleteAsync(id);
            if (!result)
                return CommonResponse<bool>.FailureResponse("Failed to delete teacher.");

            return CommonResponse<bool>.SuccessResponse("Teacher deleted successfully.", true);
        }
    }
}
