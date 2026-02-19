namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
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
                RegistrationStatus = request.RegistrationStatus,
                Isscholershipstudent = request.IsScholershipStudent,
            };

            var created = await _repository.AddAsync(student);
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
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="modeId">The mode identifier.</param>
        /// <param name="centreId">The centre identifier.</param>
        /// <param name="batchId">The batch identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="registrationStatus">The registration status.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>StudentDto.</returns>
        public async Task<IReadOnlyList<StudentDto>> GetFilteredAsync(
        Guid? countryId,
        Guid? stateId,
        Guid? modeId,
        Guid? centreId,
        Guid? batchId,
        Guid? gradeId,
        Guid? courseId,
        DateTime? startDate,
        DateTime? endDate,
        RegistrationStatus? registrationStatus,
        string? searchText,
        int limit,
        int offset,
        string sortBy,
        string sortOrder)
        {
            _logger.LogInformation("Calling repository for student filter with SortBy={SortBy}, SortOrder={SortOrder}", sortBy, sortOrder);

            var students = await _repository.GetFilteredAsync(
                countryId,
                stateId,
                modeId,
                centreId,
                batchId,
                gradeId,
                courseId,
                startDate,
                endDate,
                registrationStatus,
                searchText,
                limit,
                offset,
                sortBy,
                sortOrder);

            var dtos = students.Select(MapToDto).ToList();

            _logger.LogInformation("Mapped {Count} student entities to DTOs", dtos.Count);

            return dtos;
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
                StudentAcademicId = entity.StudentAcademicDetailsId,
                AcademicDetails = new StudentAcademicdetailsDto
                {
                    CountryId = entity.AcademicDetails.CountryId,
                    StateId = entity.AcademicDetails.StateId,
                    CenterId = entity.AcademicDetails.CenterId,
                    GradeId = entity.AcademicDetails.GradeId,
                    StreamId = entity.AcademicDetails.StreamId,
                    SubjectId = entity.AcademicDetails.SubjectId,
                    PastYearPerformance = entity.AcademicDetails.PastYearPerformance,
                    PastSchoolName = entity.AcademicDetails.PastSchoolName,
                    PastSchoolLocation = entity.AcademicDetails.PastSchoolLocation,
                    Emirates = entity.AcademicDetails.Emirates,
                },

                StudentPersonalId = entity.StudentPersonalDetailsId,
                PersonalDetails = new StudentPersonaldetailsdto
                {
                    FullName = entity.StudentPersonalDetails.FullName,
                    EmailAddress = entity.StudentPersonalDetails.EmailAddress,
                    DOB = entity.StudentPersonalDetails.DOB,
                    MobileWhatsapp = entity.StudentPersonalDetails.MobileWhatsapp,
                    MobileBotim = entity.StudentPersonalDetails.MobileBotim,
                    MobileComera = entity.StudentPersonalDetails.MobileComera,
                    AreaName = entity.StudentPersonalDetails.AreaName,
                    DistrictOrLocation = entity.StudentPersonalDetails.DistrictOrLocation,
                    Emirates = entity.StudentPersonalDetails.Emirates,
                },

                StudentDocumentsId = entity.StudentDocumentsId,
                StudentDocuments = new StudentDocumentsDto
                {
                    StudentPhoto = entity.StudentDocuments.StudentPhotoPath,
                    StudentSignature = entity.StudentDocuments.StudentSignaturePath,
                    IsAcceptedAgreement = entity.StudentDocuments.IsAcceptedAgreement,
                    IsAcceptedTermsAndConditions = entity.StudentDocuments.IsAcceptedTermsAndConditions,
                },

                Courses = entity.StudentAcademicCouses
                    .Select(c => new CourseDto
                    {
                        Id = c.CourseId,
                        CourseName = c.Course.CourseName,
                        CourseCode = c.Course.CourseCode,
                    }).ToList(),
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
         Courses = c.StudentAcademicCouses?
            .Select(x => new CourseDto
            {
                Id = x.CourseId,
                CourseName = x.Course.CourseName,
                CourseCode = x.Course.CourseCode,
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
