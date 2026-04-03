using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Teacher;
using Microsoft.Extensions.Logging;
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
        private readonly Microsoft.Extensions.Logging.ILogger<TeacherRegistrationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRegistrationService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for persistence operations.</param>
        /// <param name="logger">Logger instance for the service.</param>
        public TeacherRegistrationService(ITeacherRegistrationRepository repository, Microsoft.Extensions.Logging.ILogger<TeacherRegistrationService> logger)
        {
            _repository = repository;
            _logger = logger;
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
                CountryId = request.CountryId,
                StateId = request.StateId,
                Pincode = request.Pincode,
                DistrictOrLocation = request.DistrictOrLocation,
                MobileNumber = request.MobileNumber,
                AlternativeMobileNumber = request.AlternativeMobileNumber,
                EmailAddress = request.EmailAddress,
                AlternativeEmailAddress = request.AlternativeEmailAddress,
                RefernceContactNumber = request.ReferenceContactNumber,
                RefernceContactName = request.EmergencyContactName,
                ResidentialAddress = request.ResidentialAddress,
                IsWillingToWorkWeekends = request.IsWillingToWorkWeekends,
                HasInternetAndSystemAvailability = request.HasInternetAndSystemAvailability,
                IsTermsAccepted = request.IsTermsAccepted,
                ContractDuration = request.ContractDuration,
                // ProfessionalDetail will be mapped below if provided
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
                    ProofType = request.Documents.ProofType.HasValue ? (Winfocus.LMS.Domain.Enums.IdProofType)request.Documents.ProofType.Value : default,
                    ProofNumber = request.Documents.ProofNumber
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

            // Map professional detail and its child collections (preferred subjects/grades, tools, syllabuses, languages)
            if (request.ProfessionalDetail != null)
            {
                var pd = new TeacherProfessionalDetail
                {
                    HighestQualification = request.ProfessionalDetail.HighestQualification,
                    TotalTeachingExperience = request.ProfessionalDetail.TotalTeachingExperience,
                    HasOnlineTeachingExperience = request.ProfessionalDetail.HasOnlineTeachingExperience,
                    HasOfflineTeachingExperience = request.ProfessionalDetail.HasOfflineTeachingExperience,
                    IsAvailableForDemoClass = request.ProfessionalDetail.IsAvailableForDemoClass,
                    ComputerLiteracy = (Winfocus.LMS.Domain.Enums.ComputerLiteracy)request.ProfessionalDetail.ComputerLiteracy
                };

                // Preferred subjects
                //if (request.PreferredSubjectIds != null)
                //{
                //    foreach (var sid in request.PreferredSubjectIds)
                //    {
                //        pd.PreferredSubjects.Add(new TeacherPreferredSubject { ExamSubjectId = sid });
                //    }
                //}

                // Preferred grades
                //if (request.PreferredGradeIds != null)
                //{
                //    foreach (var gid in request.PreferredGradeIds)
                //    {
                //        pd.PreferredGrades.Add(new TeacherPreferredGrade { ExamGradeId = gid });
                //    }
                //}

                // Preferred syllabuses
                //if (request.PreferredSyllabusIds != null)
                //{
                //    foreach (var syl in request.PreferredSyllabusIds)
                //    {
                //        pd.PreferredSyllabuses.Add(new TeacherSyllabus { ExamSyllabusId = syl });
                //    }
                //}

                // Languages
                if (request.Languages != null)
                {
                    foreach (var lang in request.Languages)
                    {
                        pd.TeacherLanguage.Add(new TeacherLanguage { Language = (Winfocus.LMS.Domain.Enums.LanguageProficiency)lang });
                    }
                }

                // Tools — create tool entries and link via TeacherTool
                if (request.Tools != null)
                {
                    foreach (var t in request.Tools)
                    {
                        var tool = new TeachingTools
                        {
                            Name = t.Name,
                            Description = t.Description
                        };

                        pd.TeacherTools.Add(new TeacherTool { Tool = tool });
                    }
                }

                entity.ProfessionalDetail = pd;
            }

            // Map schedule availabilities into ProfessionalDetail's availability and Schedule (already mapped earlier)
            if (request.Schedule != null && request.Schedule.Availabilities != null)
            {
                // Ensure Schedule exists
                if (entity.Schedule == null)
                {
                    entity.Schedule = new TeacherSchedule { Availabilities = new System.Collections.Generic.List<TeacherAvailability>() };
                }

                foreach (var a in request.Schedule.Availabilities)
                {
                    var avail = new TeacherAvailability
                    {
                        Day = (System.DayOfWeek)a.Day,
                        StartTime = TimeSpan.Parse(a.StartTime),
                        EndTime = TimeSpan.Parse(a.EndTime)
                    };

                    entity.Schedule.Availabilities.Add(avail);
                }
            }

            // Taught subjects and grades (if provided) — add to registration-level collections
            //if (request.TaughtSubjectIds != null)
            //{
            //    foreach (var ts in request.TaughtSubjectIds)
            //    {
            //        entity.SubjectsTaughtEarlier.Add(new TeacherTaughtSubject { ExamSubjectId = ts });
            //    }
            //}

            //if (request.TaughtGradeIds != null)
            //{
            //    foreach (var tg in request.TaughtGradeIds)
            //    {
            //        entity.GradesTaughtEarlier.Add(new TeacherTaughtGrade { ExamGradeId = tg });
            //    }
            //}

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
                IsWillingToWorkWeekends = created.IsWillingToWorkWeekends,
                HasInternetAndSystemAvailability = created.HasInternetAndSystemAvailability,
                IsTermsAccepted = created.IsTermsAccepted,
                Status = (int)created.Status,
                AdministrativeRemarks = created.AdministrativeRemarks,
                ReportingManagerId = created.ReportingManagerId,
                CountryId = created.CountryId,
                EmploymentTypeId = created.EmploymentTypeId,
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
                CountryId = entity.CountryId,
                EmploymentTypeId = entity.EmploymentTypeId,
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
                CountryId = x.CountryId,
                EmploymentTypeId = x.EmploymentTypeId,
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
            existing.CountryId = request.CountryId;
            existing.StateId = request.StateId;
            existing.Pincode = request.Pincode;
            existing.DistrictOrLocation = request.DistrictOrLocation;
            existing.MobileNumber = request.MobileNumber;
            existing.AlternativeMobileNumber = request.AlternativeMobileNumber;
            existing.EmailAddress = request.EmailAddress;
            existing.AlternativeEmailAddress = request.AlternativeEmailAddress;
            existing.RefernceContactNumber = request.ReferenceContactNumber;
            existing.RefernceContactName = request.EmergencyContactName;
            existing.ResidentialAddress = request.ResidentialAddress;
            existing.IsWillingToWorkWeekends = request.IsWillingToWorkWeekends;
            existing.HasInternetAndSystemAvailability = request.HasInternetAndSystemAvailability;
            existing.IsTermsAccepted = request.IsTermsAccepted;
            existing.ContractDuration = request.ContractDuration;

            // Map nested objects if provided
            if (request.ProfessionalDetail != null)
            {
                existing.ProfessionalDetail = new TeacherProfessionalDetail
                {
                    HighestQualification = request.ProfessionalDetail.HighestQualification,
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
                    ProofType = request.Documents.ProofType.HasValue ? (Winfocus.LMS.Domain.Enums.IdProofType)request.Documents.ProofType.Value : default,
                    ProofNumber = request.Documents.ProofNumber
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
        /// Gets filtered teacher registrations.
        /// </summary>
        /// <summary>
        /// Gets filtered teacher registrations.
        /// </summary>
        /// <param name="request">Filter request.</param>
        /// <returns>Paged list of teacher DTOs.</returns>
        public async Task<PagedResult<TeacherRegistrationDto>> GetFilteredAsync(TeacherFilterRequest request)
        {
            var paged = await _repository.GetFilteredAsync(request);

            var dtos = paged.items.Select(x => new TeacherRegistrationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FullName = x.FullName,
                EmailAddress = x.EmailAddress,
                MobileNumber = x.MobileNumber,
                DateOfBirth = x.DateOfBirth,
                Status = (int)x.Status
            }).ToList();

            return new PagedResult<TeacherRegistrationDto>(dtos, paged.totalCount, request.Limit, request.Offset);
        }

        /// <summary>
        /// Confirms a teacher registration (changes status to Submitted).
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>Operation result.</returns>
        public async Task<CommonResponse<bool>> TeacherConfirm(Guid id)
        {
            _logger.LogInformation("Confirm teacher. Id: {Id}", id);
            return await _repository.TeacherConfirm(id);
        }

        /// <summary>
        /// Approves a teacher registration (changes status to Approved).
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>Operation result.</returns>
        public async Task<CommonResponse<bool>> TeacherApprove(Guid id)
        {
            _logger.LogInformation("Approve teacher. Id: {Id}", id);
            return await _repository.TeacherApprove(id);
        }

        /// <inheritdoc/>
        public async Task LinkUserAsync(Guid teacherId, Guid userId)
        {
            _logger.LogInformation("Linking User {UserId} to Teacher {TeacherId}", userId, teacherId);
            var existing = await _repository.GetByIdAsync(teacherId);
            if (existing == null)
            {
                _logger.LogWarning("Teacher not found for linking: {Id}", teacherId);
                return;
            }

            // Teacher entity currently doesn't have a UserId field; if present set it; otherwise log and ignore.
            var prop = existing.GetType().GetProperty("UserId");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(existing, userId);
                existing.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(existing);
                _logger.LogInformation("Linked user to teacher: {TeacherId}", teacherId);
            }
            else
            {
                _logger.LogWarning("Teacher entity does not contain UserId property; skipping link.");
            }
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
