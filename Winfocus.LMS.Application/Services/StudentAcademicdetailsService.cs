namespace Winfocus.LMS.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides business operations for <see cref="StudentAcademicdetails"/> entities.
    /// </summary>
    public sealed class StudentAcademicdetailsService : IStudentAcademicdetailsService
    {
        private readonly IStudentAcademicdetailsRepository _repository;
        private readonly ILogger<StudentAcademicdetailsService> _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IModeOfStudyRepository _modeOfStudyRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICentreRepository _centerRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IStreamRepository _streamRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IAcademicYearRepository _yearRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAcademicdetailsService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="countryRepository">countryRepository used for data access.</param>
        /// <param name="modeOfStudyRepository">modeOfStudyRepository used for data access.</param>
        /// <param name="stateRepository">stateRepository used for data access.</param>
        /// <param name="centerRepository">centerRepository used for data access.</param>
        /// <param name="syllabusRepository">syllabusRepository used for data access.</param>
        /// <param name="gradeRepository">gradeRepository used for data access.</param>
        /// <param name="streamRepository">streamRepository used for data access.</param>
        /// <param name="courseRepository">courseRepository used for data access.</param>
        /// <param name="subjectRepository">subjectRepository used for data access.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="fileStorageService">fileStorageService used for access service.</param>
        /// <param name="yearRepository">yearRepository.</param>
        public StudentAcademicdetailsService(IStudentAcademicdetailsRepository repository, ILogger<StudentAcademicdetailsService> logger, ICountryRepository countryRepository, IModeOfStudyRepository modeOfStudyRepository, IStateRepository stateRepository, ICentreRepository centerRepository, ISyllabusRepository syllabusRepository, IGradeRepository gradeRepository, IStreamRepository streamRepository, ICourseRepository courseRepository, ISubjectRepository subjectRepository, IFileStorageService fileStorageService, IAcademicYearRepository yearRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _countryRepository = countryRepository;
            _modeOfStudyRepository = modeOfStudyRepository;
            _stateRepository = stateRepository;
            _centerRepository = centerRepository;
            _syllabusRepository = syllabusRepository;
            _gradeRepository = gradeRepository;
            _streamRepository = streamRepository;
            _courseRepository = courseRepository;
            _subjectRepository = subjectRepository;
            _fileStorageService = fileStorageService;
            _yearRepository = yearRepository;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentAcademicdetailsDto.</returns>
        public async Task<IReadOnlyList<StudentAcademicdetailsDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Student Academic details");
            var studentAcademicDetails = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} students", studentAcademicDetails.Count());
            return studentAcademicDetails.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentAcademicdetailsDto.</returns>
        public async Task<StudentAcademicdetailsDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching avademic details by Id: {StudentacademicdetailsId}", id);
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
        public async Task<CommonResponse<StudentAcademicdetailsDto>> CreateAsync(StudentAcademicdetailsRequest request)
        {
            _logger.LogInformation(
                "Creating StudentAcademicDetails for CountryId: {CountryId}, StateId: {StateId}, CenterId: {CenterId}",
                request.countryId, request.stateId, request.centerId);

            var country = await _countryRepository.GetByIdAsync(request.countryId);
            if (country == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Country not found. CountryId: {CountryId}", request.countryId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Country not found");
            }

            if (!country.IsActive)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Country inactive. CountryId: {CountryId}", request.countryId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Cannot create with inactive country");
            }

            var state = await _stateRepository.GetByIdAsync(request.stateId);
            if (state == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: State not found. StateId: {StateId}", request.stateId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("State not found");
            }

            var modeOfStudy = await _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            if (modeOfStudy == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: ModeOfStudy not found. Id: {ModeOfStudyId}", request.modeOfStudyId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Mode of study not found");
            }

            var center = await _centerRepository.GetByIdAsync(request.centerId);
            if (center == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Center not found. CenterId: {CenterId}", request.centerId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Center not found");
            }

            var syllabus = await _syllabusRepository.GetByIdAsync(request.syllabusId);
            if (syllabus == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Syllabus not found. SyllabusId: {SyllabusId}", request.syllabusId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Syllabus not found");
            }

            var grade = await _gradeRepository.GetByIdAsync(request.gradeId);
            if (grade == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Grade not found. GradeId: {GradeId}", request.gradeId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Grade not found");
            }

            var stream = await _streamRepository.GetByIdAsync(request.streamId);
            if (stream == null)
            {
                _logger.LogWarning("CreateAcademicDetails failed: Stream not found. StreamId: {StreamId}", request.streamId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Stream not found");
            }

            var academicDetails = new StudentAcademicDetails
            {
                CountryId = request.countryId,
                StateId = request.stateId,
                ModeOfStudyId = request.modeOfStudyId,
                CenterId = request.centerId,
                SyllabusId = request.syllabusId,
                GradeId = request.gradeId,
                StreamId = request.streamId,
                BatchId = request.batchId,
                PreferredTime = request.preferredtime,
                PastYearPerformance = request.pastyearperformance,
                PastSchoolName = request.pastschoolname,
                PastSchoolLocation = request.pastschoollocation,
                Emirates = request.emirates,
                SubjectId = request.subjectId,
            };

            var today = DateTime.UtcNow.Date;

            _logger.LogInformation("Fetching AcademicYear for Date: {Date}", today);

            var currentYear = await _yearRepository.GetByDateAsync(today);

            if (currentYear != null)
            {
                academicDetails.AcademicYearId = currentYear.Id;

                _logger.LogInformation(
                    "AcademicYear assigned. AcademicYearId: {AcademicYearId}",
                    currentYear.Id);
            }
            else
            {
                _logger.LogWarning("No AcademicYear found for Date: {Date}", today);
            }

            var created = await _repository.AddAsync(academicDetails);

            _logger.LogInformation(
                "Student academic details created successfully. Id: {Id}",
                created.Id);

            var dto = Map(created);

            return CommonResponse<StudentAcademicdetailsDto>
                .SuccessResponse("Student academic details created successfully", dto);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">State not found.</exception>
        /// <returns>task.</returns>
        public async Task<CommonResponse<StudentAcademicdetailsDto>> UpdateAsync(
      Guid id,
      StudentAcademicdetailsRequest request)
        {
            _logger.LogInformation(
                "Updating StudentAcademicDetails started. Id: {Id}", id);

            var studentacademic = await _repository.GetByIdAsync(id);

            if (studentacademic == null)
            {
                _logger.LogWarning(
                    "Update failed. StudentAcademicDetails not found. Id: {Id}", id);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Student academic details not found");
            }

            var country = await _countryRepository.GetByIdAsync(request.countryId);
            if (country == null)
            {
                _logger.LogWarning(
                    "Update failed. Country not found. CountryId: {CountryId}",
                    request.countryId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Country not found");
            }

            if (!country.IsActive)
            {
                _logger.LogWarning(
                    "Update failed. Country inactive. CountryId: {CountryId}",
                    request.countryId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Cannot create with inactive country");
            }

            var state = await _stateRepository.GetByIdAsync(request.stateId);
            if (state == null)
            {
                _logger.LogWarning(
                    "Update failed. State not found. StateId: {StateId}",
                    request.stateId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("State not found");
            }

            var modeOfStudy = await _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            if (modeOfStudy == null)
            {
                _logger.LogWarning(
                    "Update failed. ModeOfStudy not found. Id: {ModeOfStudyId}",
                    request.modeOfStudyId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Mode of study not found");
            }

            var center = await _centerRepository.GetByIdAsync(request.centerId);
            if (center == null)
            {
                _logger.LogWarning(
                    "Update failed. Center not found. CenterId: {CenterId}",
                    request.centerId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Center not found");
            }

            var syllabus = await _syllabusRepository.GetByIdAsync(request.syllabusId);
            if (syllabus == null)
            {
                _logger.LogWarning(
                    "Update failed. Syllabus not found. SyllabusId: {SyllabusId}",
                    request.syllabusId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Syllabus not found");
            }

            var grade = await _gradeRepository.GetByIdAsync(request.gradeId);
            if (grade == null)
            {
                _logger.LogWarning(
                    "Update failed. Grade not found. GradeId: {GradeId}",
                    request.gradeId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Grade not found");
            }

            var stream = await _streamRepository.GetByIdAsync(request.streamId);
            if (stream == null)
            {
                _logger.LogWarning(
                    "Update failed. Stream not found. StreamId: {StreamId}",
                    request.streamId);

                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Stream not found");
            }

            // ✅ UPDATE EXISTING ENTITY (IMPORTANT)
            studentacademic.CountryId = request.countryId;
            studentacademic.StateId = request.stateId;
            studentacademic.ModeOfStudyId = request.modeOfStudyId;
            studentacademic.CenterId = request.centerId;
            studentacademic.SyllabusId = request.syllabusId;
            studentacademic.GradeId = request.gradeId;
            studentacademic.StreamId = request.streamId;
            studentacademic.BatchId = request.batchId;
            studentacademic.PreferredTime = request.preferredtime;
            studentacademic.PastYearPerformance = request.pastyearperformance;
            studentacademic.PastSchoolName = request.pastschoolname;
            studentacademic.PastSchoolLocation = request.pastschoollocation;
            studentacademic.Emirates = request.emirates;
            studentacademic.SubjectId = request.subjectId;

            var updated = await _repository.UpdateAsync(studentacademic);

            _logger.LogInformation(
                "StudentAcademicDetails updated successfully. Id: {Id}",
                updated.Id);

            var dto = Map(updated);

            return CommonResponse<StudentAcademicdetailsDto>
                .SuccessResponse("Student academic details updated successfully", dto);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting academicdeatails Id: {id}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "academicdeatails deleted successfully. Id: {id}",
           id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        public async Task<StudentDocumentsDto> AddUploadedDocuments(
       StudentUploaddocumentsRequest request)
        {
            _logger.LogInformation(
                "Uploading student documents started.");

            try
            {
                // Upload photo
                _logger.LogInformation("Saving student photo...");
                var photoPath = await _fileStorageService
                    .SaveFileAsync(request.studentphoto, "Photos");

                // Upload signature
                _logger.LogInformation("Saving student signature...");
                var signaturePath = await _fileStorageService
                    .SaveFileAsync(request.signature, "Signatures");

                var document = new StudentDocuments
                {
                    Id = Guid.NewGuid(),
                    StudentPhotoPath = photoPath,
                    StudentSignaturePath = signaturePath,
                    IsAcceptedAgreement = request.isAcceptedAgreement,
                    IsAcceptedTermsAndConditions = request.isAcceptedTermsAndConditions,
                };

                await _repository.AddUploadedDocuments(document);

                _logger.LogInformation(
                    "Student documents uploaded successfully. DocumentId: {DocumentId}",
                    document.Id);

                return MapDocs(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while uploading student documents.");

                throw; // let global exception middleware handle response
            }
        }

        /// <summary>
        /// updates the asynchronous.
        /// </summary>
        /// <param name="studentId">Student  details identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        public async Task<CommonResponse<StudentDocumentsDto>> UpdateUploadedDocuments(
      Guid studentId,
      StudentUploaddocumentsRequest request)
        {
            _logger.LogInformation(
                "Updating student documents started. StudentId: {StudentId}",
                studentId);

            var studentdocuments = await _repository.DocsGetByIdAsync(studentId);

            if (studentdocuments == null)
            {
                _logger.LogWarning(
                    "Update failed. Student document details not found. StudentId: {StudentId}",
                    studentId);

                return CommonResponse<StudentDocumentsDto>
                    .FailureResponse("Student document details not found");
            }

            _logger.LogInformation(
                "Saving student photo. StudentId: {StudentId}",
                studentId);

            var photoPath = await _fileStorageService
                .SaveFileAsync(request.studentphoto, "Photos");

            _logger.LogInformation(
                "Saving student signature. StudentId: {StudentId}",
                studentId);

            var signaturePath = await _fileStorageService
                .SaveFileAsync(request.signature, "Signatures");

            var document = new StudentDocuments
            {
                Id = studentId,
                StudentPhotoPath = photoPath,
                StudentSignaturePath = signaturePath,
                IsAcceptedAgreement = request.isAcceptedAgreement,
                IsAcceptedTermsAndConditions = request.isAcceptedTermsAndConditions,
            };

            _logger.LogInformation(
                "Updating student document record in database. StudentId: {StudentId}",
                studentId);

            await _repository.UpdateUploadedDocuments(document);

            _logger.LogInformation(
                "Student documents updated successfully. StudentId: {StudentId}",
                studentId);

            var dto = MapDocs(document);

            return CommonResponse<StudentDocumentsDto>
                .SuccessResponse("Student documents details created successfully", dto);
        }

        /// <inheritdoc/>
        public async Task AddCoursesAsync(
            Guid studentId,
            List<Guid> courseIds)
        {
            _logger.LogInformation(
       "Adding courses started. StudentId: {StudentId}, CourseCount: {CourseCount}",
       studentId,
       courseIds?.Count ?? 0);

            var courses = courseIds.Select(courseId =>
                new StudentAcademicCouses
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CourseId = courseId,
                }).ToList();
            _logger.LogInformation(
        "Prepared {CourseCount} course records for StudentId: {StudentId}",
        courses.Count,
        studentId);

            await _repository.AddCourseRangeAsync(courses);

            _logger.LogInformation(
        "Courses added successfully for StudentId: {StudentId}",
        studentId);
        }

        /// <inheritdoc/>
        public async Task UpdateCoursesAsync(
          Guid studentId,
          List<Guid> courseIds)
        {
            _logger.LogInformation(
       "Updating courses started. StudentId: {StudentId}, CourseCount: {CourseCount}",
       studentId,
       courseIds?.Count ?? 0);
            var courses = courseIds.Select(courseId =>
                new StudentAcademicCouses
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CourseId = courseId,
                }).ToList();

            _logger.LogInformation(
       "Prepared {CourseCount} updated course records for StudentId: {StudentId}",
       courses.Count,
       studentId);

            await _repository.UpdateCourseRangeAsync(courses);

            _logger.LogInformation(
       "Courses updated successfully for StudentId: {StudentId}",
       studentId);
        }

        /// <inheritdoc/>
        public async Task AddBatchTimingMTFsAsync(
            Guid studentId,
            List<Guid> batchtimingmtfid)
        {
            var batchtimingmtfs = batchtimingmtfid.Select(batchtimeid =>
                new StudentBatchTimingMTF
                {
                    StudentId = studentId,
                    BatchTimingMTFId = batchtimeid,
                }).ToList();

            await _repository.AddBatchtimingmtfRangeAsync(batchtimingmtfs);
        }

        /// <inheritdoc/>
        public async Task UpdateBatchTimingMTFsAsync(
            Guid studentId,
            List<Guid> batchtimingmtfid)
        {
            _logger.LogInformation(
      "Adding MTF batch timings started. StudentId: {StudentId}, BatchTimingCount: {Count}",
      studentId,
      batchtimingmtfid?.Count ?? 0);

            var batchtimingmtfs = batchtimingmtfid.Select(batchtimeid =>
                new StudentBatchTimingMTF
                {
                    StudentId = studentId,
                    BatchTimingMTFId = batchtimeid,
                }).ToList();

            _logger.LogInformation(
       "Prepared {Count} MTF batch timing records for StudentId: {StudentId}",
       batchtimingmtfs.Count,
       studentId);

            await _repository.UpdateBatchtimingmtfRangeAsync(batchtimingmtfs);

            _logger.LogInformation(
     "MTF batch timings added successfully for StudentId: {StudentId}",
     studentId);
        }

        /// <inheritdoc/>
        public async Task AddBatchTimingSaturdaysAsync(
          Guid studentId,
          List<Guid> batchtimingsaturdayids)
        {
            _logger.LogInformation(
      "Adding Saturday batch timings started. StudentId: {StudentId}, BatchTimingCount: {Count}",
      studentId,
      batchtimingsaturdayids?.Count ?? 0);
            var batchtimingsaturdays = batchtimingsaturdayids.Select(batchtimingsaturdayid =>
                new StudentBatchTimingSaturday
                {
                    StudentId = studentId,
                    BatchTimingSaturdayId = batchtimingsaturdayid,
                }).ToList();

            _logger.LogInformation(
       "Prepared {Count} Saturday batch timing records for StudentId: {StudentId}",
       batchtimingsaturdays.Count,
       studentId);

            await _repository.AddBatchtimingsaturdayRangeAsync(batchtimingsaturdays);

            _logger.LogInformation(
       "Saturday batch timings added successfully for StudentId: {StudentId}",
       studentId);
        }

        /// <inheritdoc/>
        public async Task UpdateBatchTimingSaturdaysAsync(
          Guid studentId,
          List<Guid> batchtimingsaturdayids)
        {
            _logger.LogInformation(
       "Updating Saturday batch timings. StudentId: {StudentId}, Count: {Count}",
       studentId,
       batchtimingsaturdayids?.Count ?? 0);
            var batchtimingsaturdays = batchtimingsaturdayids.Select(batchtimingsaturdayid =>
                new StudentBatchTimingSaturday
                {
                    StudentId = studentId,
                    BatchTimingSaturdayId = batchtimingsaturdayid,
                }).ToList();

            await _repository.UpdateBatchtimingsaturdayRangeAsync(batchtimingsaturdays);

            _logger.LogInformation(
        "Saturday batch timings updated successfully. StudentId: {StudentId}",
        studentId);
        }

        /// <inheritdoc/>
        public async Task AddBatchTimingSundaysAsync(
          Guid studentId,
          List<Guid> batchtimingsundayids)
        {

            _logger.LogInformation(
      "Adding Sunday batch timings. StudentId: {StudentId}, Count: {Count}",
      studentId,
      batchtimingsundayids?.Count ?? 0);
            var batchtimingsundays = batchtimingsundayids.Select(batchtimingsundayid =>
                new StudentBatchTimingSunday
                {
                    StudentId = studentId,
                    BatchTimingSundayId = batchtimingsundayid,
                }).ToList();

            await _repository.AddBatchtimingsundayRangeAsync(batchtimingsundays);

            _logger.LogInformation(
        "Sunday batch timings added successfully. StudentId: {StudentId}",
        studentId);
        }

        /// <inheritdoc/>
        public async Task UpdateBatchTimingSundaysAsync(
          Guid studentId,
          List<Guid> batchtimingsundayids)
        {
            _logger.LogInformation(
               "Updating Sunday batch timings. StudentId: {StudentId}, Count: {Count}",
               studentId,
               batchtimingsundayids?.Count ?? 0);

            var batchtimingsundays = batchtimingsundayids.Select(batchtimingsundayid =>
                new StudentBatchTimingSunday
                {
                    StudentId = studentId,
                    BatchTimingSundayId = batchtimingsundayid,
                }).ToList();

            await _repository.UpdateBatchtimingsundayRangeAsync(batchtimingsundays);

            _logger.LogInformation(
              "Sunday batch timings updated successfully. StudentId: {StudentId}",
              studentId);
        }

        private static StudentDocumentsDto MapDocs(StudentDocuments c) =>
    new StudentDocumentsDto
    {
        Id = c.Id,
        CreatedBy = c.CreatedBy,
        CreatedAt = c.CreatedAt,
        UpdatedBy = c.UpdatedBy,
        UpdatedAt = c.UpdatedAt,
        StudentPhoto = c.StudentPhotoPath,
        StudentSignature = c.StudentSignaturePath,
        IsAcceptedAgreement = c.IsAcceptedAgreement,
        IsAcceptedTermsAndConditions = c.IsAcceptedTermsAndConditions,
    };

        private static StudentAcademicdetailsDto Map(StudentAcademicDetails c) =>
     new StudentAcademicdetailsDto
     {
         Id = c.Id,
         CountryId = c.CountryId,
         StateId = c.StateId,
         ModeOfStudyId = c.ModeOfStudyId,
         CenterId = c.CenterId,
         SyllabusId = c.SyllabusId,
         GradeId = c.GradeId,
         StreamId = c.StreamId,

         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedBy = c.UpdatedBy,
         UpdatedAt = c.UpdatedAt,
     };
    }
}
