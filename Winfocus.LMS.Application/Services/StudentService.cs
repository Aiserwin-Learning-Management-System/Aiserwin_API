namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

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
            _logger.LogInformation("Fetching all students.");

            var students = await _repository.GetAllAsync();

            var result = students.Select(Map).ToList();

            _logger.LogInformation(
                "Fetched {Count} students successfully.",
                result.Count);

            return result;

        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto.</returns>
        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation(
                "Fetching student by Id: {Id}", id);

            var student = await _repository.GetByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning(
                    "Student not found. Id: {Id}", id);

                return null;
            }

            _logger.LogInformation(
                "Student fetched successfully. Id: {Id}", id);

            return Map(student);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StudentDto> CreateAsync(StudentDto request)
        {
            _logger.LogInformation(
                "Creating student record. AcademicId: {AcademicId}, UserId: {UserId}",
                request.StudentAcademicId,
                request.Userid);
            var student = new Student
            {
                StudentAcademicDetailsId = request.StudentAcademicId,
                StudentDocumentsId = request.StudentDocumentsId,
                StudentPersonalDetailsId = request.StudentPersonalId,
                CreatedBy = request.Userid,
                CreatedAt = DateTime.UtcNow,
                RegistrationStatus = request.RegistrationStatus,
                Isscholershipstudent = request.IsScholershipStudent,
            };

            var created = await _repository.AddAsync(student);
            _logger.LogInformation(
               "Student created successfully. StudentId: {StudentId}",
               created.Id);
            return MapCreate(created);
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
            _logger.LogInformation(
              "Updating student. StudentId: {StudentId}", id);

            var student = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Student not found");

            student.StudentAcademicDetailsId = request.StudentAcademicId;
            student.StudentDocumentsId = request.StudentDocumentsId;
            student.StudentPersonalDetailsId = request.StudentPersonalId;
            student.UpdatedBy = request.Userid;
            student.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(student);

            _logger.LogInformation(
                "Student updated successfully. StudentId: {StudentId}", id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDto.</returns>
        public async Task<PagedResult<StudentDto>> GetFilteredAsync(StudentFilterRequest request)
        {
            _logger.LogDebug("Starting student filter service for SearchText: {Search}", request.SearchText);

            try
            {
                // 1. Get the Paged Entities from Repository
                var pagedStudents = await _repository.GetFilteredAsync(request);

                // 2. Map the Entities to DTOs using your existing method
                var dtos = pagedStudents.items.Select(MapToDto).ToList();

                _logger.LogInformation("Successfully mapped {Count} students to DTOs.", dtos.Count);

                // 3. Wrap the DTOs back into a PagedResult for the Frontend
                return new PagedResult<StudentDto>(
                    dtos,
                    pagedStudents.totalCount,
                    request.Limit,
                    request.Offset
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering and mapping students.");
                throw;
            }
        }

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> StudentConfirm(Guid id)
        {
            return await _repository.StudentConfirm(id);
        }

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> StudentApprove(Guid id)
        {
            _logger.LogInformation(
             "approve student. StudentId: {StudentId}", id);
            return await _repository.StudentApprove(id);
        }

        private StudentDto MapToDto(Student entity)
        {
            return new StudentDto
            {
                Id = entity.Id,
                Userid = entity.StudentPersonalDetailsId,
                RegistrationStatus = entity.RegistrationStatus,
                RegistraionNumber = entity.RegistrationNumber,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,

                StudentAcademicId = entity.StudentAcademicDetailsId,
                AcademicDetails = new StudentAcademicdetailsDto
                {
                    Id = entity.StudentAcademicDetailsId,
                    CountryId = entity.AcademicDetails.CountryId,
                    Country = entity.AcademicDetails.Country != null ? new CountryDto
                    {
                        Id = entity.AcademicDetails.CountryId,
                        Name = entity.AcademicDetails.Country.Name,
                    } : null!,
                    StateId = entity.AcademicDetails.StateId,
                    State = entity.AcademicDetails.State != null ? new StateDto
                    {
                        Id = entity.AcademicDetails.StateId,
                        Name = entity.AcademicDetails.State.Name,
                    } : null!,
                    ModeOfStudyId = entity.AcademicDetails.ModeOfStudyId,
                    ModeOfStudy = entity.AcademicDetails.ModeOfStudy != null ? new ModeOfStudyDto
                    {
                        Id = entity.AcademicDetails.ModeOfStudyId,
                        Name = entity.AcademicDetails.ModeOfStudy.Name,
                    } : null!,
                    CenterId = entity.AcademicDetails.CenterId,
                    Center = entity.AcademicDetails.Center != null ? new CenterDto
                    {
                        Id = entity.AcademicDetails.CenterId,
                        Name = entity.AcademicDetails.Center.Name,
                    } : null!,
                    SyllabusId = entity.AcademicDetails.SyllabusId,
                    Syllabus = entity.AcademicDetails.Syllabus != null ? new SyllabusDto
                    {
                        Id = entity.AcademicDetails.SyllabusId,
                        Name = entity.AcademicDetails.Syllabus.Name,
                    } : null!,
                    GradeId = entity.AcademicDetails.GradeId,
                    Grade = entity.AcademicDetails.Grade != null ? new GradeDto
                    {
                        Id = entity.AcademicDetails.GradeId,
                        Name = entity.AcademicDetails.Grade.Name,
                    } : null!,
                    StreamId = entity.AcademicDetails.StreamId,
                    Stream = entity.AcademicDetails.Stream != null ? new StreamDto
                    {
                        Id = entity.AcademicDetails.StreamId,
                        Name = entity.AcademicDetails.Stream.Name,
                    } : null!,
                    SubjectId = entity.AcademicDetails.SubjectId,
                    Subject = entity.AcademicDetails.Subject != null ? new SubjectDto
                    {
                        Id = entity.AcademicDetails.SubjectId,
                        Name = entity.AcademicDetails.Subject.Name,
                    } : null!,
                    PastYearPerformance = entity.AcademicDetails.PastYearPerformance,
                    PastSchoolName = entity.AcademicDetails.PastSchoolName,
                    PastSchoolLocation = entity.AcademicDetails.PastSchoolLocation,
                    Emirates = entity.AcademicDetails.Emirates,
                },

                StudentPersonalId = entity.StudentPersonalDetailsId,
                PersonalDetails = new StudentPersonaldetailsdto
                {
                    Id = entity.StudentPersonalDetailsId,
                    FullName = entity.StudentPersonalDetails.FullName,
                    EmailAddress = entity.StudentPersonalDetails.EmailAddress,
                    DOB = entity.StudentPersonalDetails.DOB,
                    MobileWhatsapp = entity.StudentPersonalDetails.MobileWhatsapp,
                    MobileBotim = entity.StudentPersonalDetails.MobileBotim,
                    MobileComera = entity.StudentPersonalDetails.MobileComera,
                    AreaName = entity.StudentPersonalDetails.AreaName,
                    DistrictOrLocation = entity.StudentPersonalDetails.DistrictOrLocation,
                    Emirates = entity.StudentPersonalDetails.Emirates,
                    Gender = entity.StudentPersonalDetails.Gender,
                },

                StudentDocumentsId = entity.StudentDocumentsId,
                StudentDocuments = new StudentDocumentsDto
                {
                    Id = entity.StudentDocumentsId,
                    StudentPhoto = entity.StudentDocuments.StudentPhotoPath,
                    StudentSignature = entity.StudentDocuments.StudentSignaturePath,
                    IsAcceptedAgreement = entity.StudentDocuments.IsAcceptedAgreement,
                    IsAcceptedTermsAndConditions = entity.StudentDocuments.IsAcceptedTermsAndConditions,
                },

                Courses = entity.StudentAcademicCouses?
                    .Select(c => new CourseDto
                    {
                        Id = c.CourseId,
                        Name = c.Course.Name,
                    }).ToList() ?? new List<CourseDto>(),
            };
        }

        private static StudentDto Map(Student c) =>
     new StudentDto
     {
         Id = c.Id,
         AcademicDetails = new StudentAcademicdetailsDto
         {
             Id = c.StudentAcademicDetailsId,
             CountryId = c.AcademicDetails.CountryId,
             Country = new CountryDto
             {
                 Id = c.AcademicDetails.CountryId,
                 Name = c.AcademicDetails.Country.Name,
             },
             StateId = c.AcademicDetails.StateId,
             State = new StateDto
             {
                 Id = c.AcademicDetails.StateId,
                 Name = c.AcademicDetails.State.Name,
             },
             ModeOfStudyId = c.AcademicDetails.ModeOfStudyId,
             ModeOfStudy = new ModeOfStudyDto
             {
                 Id = c.AcademicDetails.ModeOfStudyId,
                 Name = c.AcademicDetails.ModeOfStudy.Name,
             },
             CenterId = c.AcademicDetails.CenterId,
             Center = new CenterDto
             {
                 Id = c.AcademicDetails.CenterId,
                 Name = c.AcademicDetails.Center.Name,
             },
             SyllabusId = c.AcademicDetails.SyllabusId,
             Syllabus = new SyllabusDto
             {
                 Id = c.AcademicDetails.SyllabusId,
                 Name = c.AcademicDetails.Syllabus.Name,
             },
             GradeId = c.AcademicDetails.GradeId,
             Grade = new GradeDto
             {
                 Id = c.AcademicDetails.GradeId,
                 Name = c.AcademicDetails.Grade.Name,
             },
             StreamId = c.AcademicDetails.StreamId,
             Stream = new StreamDto
             {
                 Id = c.AcademicDetails.StreamId,
                 Name = c.AcademicDetails.Stream.Name,
             },
             SubjectId = c.AcademicDetails.SubjectId,
             Subject = new SubjectDto
             {
                 Id = c.AcademicDetails.SubjectId,
                 Name = c.AcademicDetails.Subject.Name,
             },
             PastYearPerformance = c.AcademicDetails.PastYearPerformance,
             PastSchoolLocation = c.AcademicDetails.PastSchoolLocation,
             PastSchoolName = c.AcademicDetails.PastSchoolName,
             Emirates = c.AcademicDetails.Emirates,
             AcademicYearId = c.AcademicDetails.AcademicYearId,
             PreferredBatchTime = c.AcademicDetails.PreferredTime,
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
             Gender = c.StudentPersonalDetails.Gender,
         },
         StudentDocuments = new StudentDocumentsDto
         {
             Id = c.StudentDocumentsId,
             StudentPhoto = c.StudentDocuments.StudentPhotoPath,
             StudentSignature = c.StudentDocuments.StudentSignaturePath,
             IsAcceptedAgreement = c.StudentDocuments.IsAcceptedAgreement,
             IsAcceptedTermsAndConditions = c.StudentDocuments.IsAcceptedTermsAndConditions,
         },
         Courses = c.StudentAcademicCouses?
            .Select(x => new CourseDto
            {
                Id = x.CourseId,
                Name = x.Course.Name,
            }).ToList() ?? new List<CourseDto>(),
         BatchTimingMTFs = c.StudentBatchTimingMTFs?
            .Select(x => new BatchTimingMTFDto
            {
                Id = x.BatchTimingMTFId,
                BatchTime = x.BatchTimingMTF.BatchTime.ToString("dd/MM/yyyy hh:mm tt"),
            }).ToList() ?? new List<BatchTimingMTFDto>(),
         BatchTimingSaturdays = c.StudentBatchTimingSaturdays?
            .Select(x => new BatchTimingSaturdayDto
            {
                Id = x.BatchTimingSaturdayId,
                BatchTime = x.BatchTimingSaturday.BatchTime.ToString("dd/MM/yyyy hh:mm tt"),
            }).ToList() ?? new List<BatchTimingSaturdayDto>(),
         BatchTimingSundays = c.StudentBatchTimingSundays?
            .Select(x => new BatchTimingSundayDto
            {
                Id = x.BatchTimingSundayId,
                BatchTime = x.BatchTimingSunday.BatchTime.ToString("dd/MM/yyyy hh:mm tt"),
            }).ToList() ?? new List<BatchTimingSundayDto>(),
         StudentAcademicId = c.StudentAcademicDetailsId,
         StudentDocumentsId = c.StudentDocumentsId,
         StudentPersonalId = c.StudentPersonalDetailsId,
         RegistraionNumber = c.RegistrationNumber,
         RegistrationStatus = c.RegistrationStatus,
         IsScholershipStudent = c.Isscholershipstudent,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedAt = c.UpdatedAt,
         UpdatedBy = c.UpdatedBy,
     };

        private static StudentDto MapCreate(Student c) =>
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
