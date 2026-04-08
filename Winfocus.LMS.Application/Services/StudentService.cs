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
        private readonly IDoubtClearingRepository _doubtClearingRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService" /> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="doubtClearingRepository">doubtClearingRepository.</param>
        /// <param name="stateRepository">Repository for state/emirate lookups.</param>
        /// <param name="fileStorageService">The file storage service.</param>
        public StudentService(
            IStudentRepository repository,
            ILogger<StateService> logger,
            IDoubtClearingRepository doubtClearingRepository,
            IStateRepository stateRepository,
            IFileStorageService fileStorageService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _doubtClearingRepository = doubtClearingRepository ?? throw new ArgumentNullException(nameof(doubtClearingRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="countryid">The countryid.</param>
        /// <param name="stateid">The stateid.</param>
        /// <param name="centerid">The centerid.</param>
        /// <returns>StateDto.</returns>
        public async Task<IReadOnlyList<StudentDto>> GetAllAsync(Guid? countryid, Guid? stateid, Guid? centerid)
        {
            _logger.LogInformation("Fetching all students.");

            var students = await _repository.GetAllAsync();
            List<DoubtClearing>? doubtClear;

            // Apply filters
            if (countryid.HasValue && countryid != Guid.Empty)
            {
                students = students.Where(s => s.AcademicDetails.CountryId == countryid.Value).ToList();
            }

            if (stateid.HasValue && stateid != Guid.Empty)
            {
                students = students.Where(s => s.AcademicDetails.StateId == stateid.Value).ToList();
            }

            if (centerid.HasValue && centerid != Guid.Empty)
            {
                students = students.Where(s => s.AcademicDetails.CenterId == centerid.Value).ToList();
            }

            var result = students.Select(s => Map(s, null)).ToList();

            _logger.LogInformation(
                "Fetched {Count} students successfully.",
                result.Count);

            return result;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StateDto.</returns>
        public async Task<IReadOnlyList<StudentDto>> DiscountRequestStudents()
        {
            _logger.LogInformation("Fetching all discount requested students.");

            var students = await _repository.DiscountRequestStudents();

            var result = students.Select(s => Map(s, null)).ToList();

            _logger.LogInformation(
                "Fetched {Count} students successfully.",
                result.Count);

            return result;
        }

        /// <summary>
        /// Gets a student by the Student entity's primary key.
        /// </summary>
        /// <param name="id">The student entity identifier.</param>
        /// <returns>StudentDto if found; otherwise null.</returns>
        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching student by StudentId: {Id}", id);

            Student? student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student not found. StudentId: {Id}", id);
                return null;
            }

            return await MapWithEmiratesAsync(student);
        }

        /// <summary>
        /// Gets a student by StudentId with optional scope filtering.
        /// Used by admin endpoints where scope (country/state/center) is enforced.
        /// </summary>
        /// <param name="id">The student entity identifier.</param>
        /// <param name="countryId">Optional country filter.</param>
        /// <param name="stateId">Optional state filter.</param>
        /// <param name="centerId">Optional center filter.</param>
        /// <returns>StudentDto if found and passes filters; otherwise null.</returns>
        public async Task<StudentDto?> GetByIdsAsync(Guid id, Guid? countryId, Guid? stateId, Guid? centerId)
        {
            _logger.LogInformation("Fetching student by StudentId with filters: {Id}", id);

            Student? student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student not found. StudentId: {Id}", id);
                return null;
            }

            if (!PassesScopeFilter(student, countryId, stateId, centerId))
            {
                _logger.LogWarning("Student does not match scope filters. StudentId: {Id}", id);
                return null;
            }

            return await MapWithEmiratesAsync(student);
        }

        /// <summary>
        /// Gets a student by UserId with optional scope filtering.
        /// Used when looking up a student by their linked auth user account.
        /// </summary>
        /// <param name="userId">The user identifier from auth system.</param>
        /// <param name="countryId">Optional country filter.</param>
        /// <param name="stateId">Optional state filter.</param>
        /// <param name="centerId">Optional center filter.</param>
        /// <returns>StudentDto if found and passes filters; otherwise null.</returns>
        public async Task<StudentDto?> GetByUserIdsAsync(Guid userId, Guid? countryId, Guid? stateId, Guid? centerId)
        {
            _logger.LogInformation("Fetching student by UserId: {UserId}", userId);

            Student? student = await _repository.GetByUserIdAsync(userId);
            if (student == null)
            {
                _logger.LogWarning("Student not found for UserId: {UserId}", userId);
                return null;
            }

            if (!PassesScopeFilter(student, countryId, stateId, centerId))
            {
                _logger.LogWarning("Student does not match scope filters. UserId: {UserId}", userId);
                return null;
            }

            return await MapWithEmiratesAsync(student);
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
        /// request for access.
        /// </summary>
        /// <param name="studentid">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> Requestfordiscount(Guid studentid)
        {
            return await _repository.Requestfordiscount(studentid);
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

        /// <inheritdoc/>
        public async Task LinkUserAsync(Guid studentId, Guid userId)
        {
            _logger.LogInformation(
                                   "Linking UserId {UserId} to StudentId {StudentId}",
                                   userId,
                                   studentId);

            var student = await _repository.GetByIdAsync(studentId);
            if (student == null)
            {
                _logger.LogWarning("Student not found for linking. Id: {Id}", studentId);
                return;
            }

            student.UserId = userId;
            student.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(student);

            _logger.LogInformation(
                "Successfully linked UserId {UserId} to StudentId {StudentId}",
                userId, 
                studentId);
        }

        /// <summary>
        /// Maps a student entity to DTO and populates emirates details.
        /// Reused by GetByIdAsync, GetByIdsAsync, and GetByUserIdsAsync.
        /// </summary>
        /// <param name="student">The student entity with navigations loaded.</param>
        /// <returns>The mapped DTO with emirates details populated.</returns>
        private async Task<StudentDto> MapWithEmiratesAsync(Student student)
        {
            Guid subjectId = student.StudentBatchTimingMTFs
                .Select(x => x.BatchTimingMTF.SubjectId)
                .FirstOrDefault();

            List<DoubtClearing>? doubtClear = await _doubtClearingRepository
                .GetBySubjectIdAsync(subjectId);

            StudentDto dto = Map(student, doubtClear);

            // Populate academic emirates.
            if (!string.IsNullOrWhiteSpace(student.AcademicDetails.Emirates)
                && Guid.TryParse(student.AcademicDetails.Emirates, out Guid academicEmiratesId))
            {
                State? emirateState = await _stateRepository
                    .GetByIdAsync(academicEmiratesId, student.AcademicDetails.CountryId);
                if (emirateState != null)
                {
                    dto.AcademicDetails.Emirates = MapStateToDto(emirateState);
                }
            }

            // Populate personal emirates.
            if (!string.IsNullOrWhiteSpace(student.StudentPersonalDetails.Emirates)
                && Guid.TryParse(student.StudentPersonalDetails.Emirates, out Guid personalEmiratesId))
            {
                State? emirateState = await _stateRepository
                    .GetByIdAsync(personalEmiratesId, student.AcademicDetails.CountryId);
                if (emirateState != null)
                {
                    dto.PersonalDetails.Emirates = MapStateToDto(emirateState);
                }
            }

            return dto;
        }

        /// <summary>
        /// Applies optional scope filters (country, state, center) on a student entity.
        /// Returns false if student does not match any provided filter.
        /// </summary>
        /// <param name="student">The student entity.</param>
        /// <param name="countryId">Optional country filter.</param>
        /// <param name="stateId">Optional state filter.</param>
        /// <param name="centerId">Optional center filter.</param>
        /// <returns>True if student passes all filters; otherwise false.</returns>
        private bool PassesScopeFilter(Student student, Guid? countryId, Guid? stateId, Guid? centerId)
        {
            if (countryId.HasValue && countryId != Guid.Empty
                && student.AcademicDetails.CountryId != countryId)
            {
                return false;
            }

            if (stateId.HasValue && stateId != Guid.Empty
                && student.AcademicDetails.StateId != stateId)
            {
                return false;
            }

            if (centerId.HasValue && centerId != Guid.Empty
                && student.AcademicDetails.CenterId != centerId)
            {
                return false;
            }

            return true;
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
                    BatchId = entity.AcademicDetails.BatchId,
                    PastYearPerformance = entity.AcademicDetails.PastYearPerformance,
                    PastSchoolName = entity.AcademicDetails.PastSchoolName,
                    PastSchoolLocation = entity.AcademicDetails.PastSchoolLocation,
                    EmiratesId = entity.AcademicDetails.Emirates,
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
                    EmiratesId = entity.StudentPersonalDetails.Emirates,
                    Gender = entity.StudentPersonalDetails.Gender,
                },

                StudentDocumentsId = entity.StudentDocumentsId,
                StudentDocuments = new StudentDocumentsDto
                {
                    Id = entity.StudentDocumentsId,
                    StudentPhoto = !string.IsNullOrEmpty(
                        entity.StudentDocuments.StudentPhotoPath)
                        ? _fileStorageService.GetFileUrl(
                            entity.StudentDocuments.StudentPhotoPath)
                        : null,

                    StudentSignature = !string.IsNullOrEmpty(
                        entity.StudentDocuments.StudentSignaturePath)
                        ? _fileStorageService.GetFileUrl(
                            entity.StudentDocuments.StudentSignaturePath)
                        : null,

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

        private static StateDto MapStateToDto(State state) =>
            new StateDto
            {
                Id = state.Id,
                Name = state.Name,
                CountryId = state.CountryId,
                Country = new CountryDto
                {
                    Id = state.CountryId,
                    Name = state.Country.Name,
                },
                ModeOfStudyId = state.ModeOfStudyId,
                ModeOfStudy = new ModeOfStudyDto
                {
                    Id = state.ModeOfStudyId,
                    Name = state.ModeOfStudy.Name,
                },
            };

        private StudentDto Map(Student c, List<DoubtClearing>? doubtClear) =>
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
             BatchId = c.AcademicDetails.BatchId,
             PastYearPerformance = c.AcademicDetails.PastYearPerformance,
             PastSchoolLocation = c.AcademicDetails.PastSchoolLocation,
             PastSchoolName = c.AcademicDetails.PastSchoolName,
             EmiratesId = c.AcademicDetails.Emirates,
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
             EmiratesId = c.StudentPersonalDetails.Emirates,
             Gender = c.StudentPersonalDetails.Gender,
         },
         StudentDocuments = new StudentDocumentsDto
         {
             Id = c.StudentDocumentsId,
             StudentPhoto = !string.IsNullOrEmpty(
                    c.StudentDocuments.StudentPhotoPath)
                    ? _fileStorageService.GetFileUrl(
                        c.StudentDocuments.StudentPhotoPath)
                    : null,
              StudentSignature = !string.IsNullOrEmpty(
                    c.StudentDocuments.StudentSignaturePath)
                    ? _fileStorageService.GetFileUrl(
                        c.StudentDocuments.StudentSignaturePath)
                    : null,
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
                BatchTime = DateTime.SpecifyKind(x.BatchTimingMTF.BatchTime, DateTimeKind.Utc),
                Subject = new SubjectDto
                {
                    Id = x.BatchTimingMTF.Subject.Id,
                    Name = x.BatchTimingMTF.Subject.Name
                }
            }).ToList() ?? new List<BatchTimingMTFDto>(),
         BatchTimingSaturdays = c.StudentBatchTimingSaturdays?
            .Select(x => new BatchTimingSaturdayDto
            {
                Id = x.BatchTimingSaturdayId,
                BatchTime = DateTime.SpecifyKind(x.BatchTimingSaturday.BatchTime, DateTimeKind.Utc),
            }).ToList() ?? new List<BatchTimingSaturdayDto>(),
         BatchTimingSundays = c.StudentBatchTimingSundays?
            .Select(x => new BatchTimingSundayDto
            {
                Id = x.BatchTimingSundayId,
                BatchTime = DateTime.SpecifyKind(x.BatchTimingSunday.BatchTime, DateTimeKind.Utc),
            }).ToList() ?? new List<BatchTimingSundayDto>(),
         DoubtClearingDtos = doubtClear?
    .Select(d => new DoubtClearingDto
    {
        Id = d.Id,
        ScheduleStartDate = d.ScheduleTime.ToString("dd/MM/yyyy hh:mm tt"),
        ScheduleEndDate = d.ScheduleEndTime.ToString("dd/MM/yyyy hh:mm tt"),
        SubjectId = d.SubjectId,
        Subject = new SubjectDto
        {
            Id = d.Subject.Id,
            Name = d.Subject.Name
        }
    }).ToList() ?? new List<DoubtClearingDto>(),
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
