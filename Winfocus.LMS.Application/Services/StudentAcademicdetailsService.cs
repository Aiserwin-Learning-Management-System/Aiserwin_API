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
        public async Task<CommonResponse<StudentAcademicdetailsDto>> CreateAsync(StudentAcademicdetailsRequest request)
        {
            var country = await _countryRepository.GetByIdAsync(request.countryId);
            if (country == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Country not found");
            }

            if (!country.IsActive)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Cannot create with inactive country");
            }

            var state = await _stateRepository.GetByIdAsync(request.stateId);
            if (state == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("State not found");
            }

            var modeOfStudy = await _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            if (modeOfStudy == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Mode of study not found");
            }

            var center = await _centerRepository.GetByIdAsync(request.centerId);
            if (center == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Center not found");
            }

            var syllabus = await _syllabusRepository.GetByIdAsync(request.syllabusId);
            if (syllabus == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Syllabus not found");
            }

            var grade = await _gradeRepository.GetByIdAsync(request.gradeId);
            if (grade == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Grade not found");
            }

            var stream = await _streamRepository.GetByIdAsync(request.streamId);
            if (stream == null)
            {
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
                PreferredBatchTimeId = request.preferredbatchtimeId,
                PastYearPerformance = request.pastyearperformance,
                PastSchoolName = request.pastschoolname,
                PastSchoolLocation = request.pastschoollocation,
                Emirates = request.emirates,
                SubjectId = request.subjectId,
            };
            var today = DateTime.UtcNow.Date;
            var currentYear = await _yearRepository.GetByDateAsync(today);

            if (currentYear != null)
            {
                academicDetails.AcademicYearId = currentYear.Id;
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
        public async Task<CommonResponse<StudentAcademicdetailsDto>> UpdateAsync(Guid id, StudentAcademicdetailsRequest request)
        {
            _logger.LogInformation("Updating StudentAcademicDetails Id: {Id}", id);
            var studentacademic = await _repository.GetByIdAsync(id);
            if (studentacademic == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Student academic details not found");
            }
            var country = await _countryRepository.GetByIdAsync(request.countryId);
            if (country == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Country not found");
            }

            if (!country.IsActive)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Cannot create with inactive country");
            }

            var state = await _stateRepository.GetByIdAsync(request.stateId);
            if (state == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("State not found");
            }

            var modeOfStudy = await _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            if (modeOfStudy == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Mode of study not found");
            }

            var center = await _centerRepository.GetByIdAsync(request.centerId);
            if (center == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Center not found");
            }

            var syllabus = await _syllabusRepository.GetByIdAsync(request.syllabusId);
            if (syllabus == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Syllabus not found");
            }

            var grade = await _gradeRepository.GetByIdAsync(request.gradeId);
            if (grade == null)
            {
                return CommonResponse<StudentAcademicdetailsDto>
                    .FailureResponse("Grade not found");
            }

            var stream = await _streamRepository.GetByIdAsync(request.streamId);
            if (stream == null)
            {
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
                PreferredBatchTimeId = request.preferredbatchtimeId,
                PastYearPerformance = request.pastyearperformance,
                PastSchoolName = request.pastschoolname,
                PastSchoolLocation = request.pastschoollocation,
                Emirates = request.emirates,
                SubjectId = request.subjectId,
                Id = id,
            };

            var updated = await _repository.UpdateAsync(academicDetails);

            _logger.LogInformation(
                "Student academic details updated successfully. Id: {Id}",
                id);

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
            _logger.LogInformation("Deleting state Id: {StateId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation(
           "State deleted successfully. StateId: {StateId}",
           id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        public async Task<StudentDocumentsDto> AddUploadedDocuments(StudentUploaddocumentsRequest request)
        {

            var photoPath = await _fileStorageService
            .SaveFileAsync(request.studentphoto, "Photos");

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

            return MapDocs(document);
        }

        /// <summary>
        /// updates the asynchronous.
        /// </summary>
        /// <param name="studentId">Student  details identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        public async Task<CommonResponse<StudentDocumentsDto>> UpdateUploadedDocuments(Guid studentId, StudentUploaddocumentsRequest request)
        {
            var studentdocuments = await _repository.DocsGetByIdAsync(studentId);
            if (studentdocuments == null)
            {
                return CommonResponse<StudentDocumentsDto>
                    .FailureResponse("Student document details not found");
            }

            var photoPath = await _fileStorageService
            .SaveFileAsync(request.studentphoto, "Photos");

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

            await _repository.UpdateUploadedDocuments(document);

            var dto = MapDocs(document);

            return CommonResponse<StudentDocumentsDto>
                .SuccessResponse("Student documents details created successfully", dto);
        }


        /// <inheritdoc/>
        public async Task AddCoursesAsync(
            Guid studentId,
            List<Guid> courseIds)
        {
            var courses = courseIds.Select(courseId =>
                new StudentAcademicCouses
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CourseId = courseId,
                }).ToList();

            await _repository.AddCourseRangeAsync(courses);
        }

        /// <inheritdoc/>
        public async Task UpdateCoursesAsync(
          Guid studentId,
          List<Guid> courseIds)
        {
            var courses = courseIds.Select(courseId =>
                new StudentAcademicCouses
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CourseId = courseId,
                }).ToList();

            await _repository.UpdateCourseRangeAsync(courses);
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
            var batchtimingmtfs = batchtimingmtfid.Select(batchtimeid =>
                new StudentBatchTimingMTF
                {
                    StudentId = studentId,
                    BatchTimingMTFId = batchtimeid,
                }).ToList();

            await _repository.UpdateBatchtimingmtfRangeAsync(batchtimingmtfs);
        }

        /// <inheritdoc/>
        public async Task AddBatchTimingSaturdaysAsync(
          Guid studentId,
          List<Guid> batchtimingsaturdayids)
        {
            var batchtimingsaturdays = batchtimingsaturdayids.Select(batchtimingsaturdayid =>
                new StudentBatchTimingSaturday
                {
                    StudentId = studentId,
                    BatchTimingSaturdayId = batchtimingsaturdayid,
                }).ToList();

            await _repository.AddBatchtimingsaturdayRangeAsync(batchtimingsaturdays);
        }

        /// <inheritdoc/>
        public async Task UpdateBatchTimingSaturdaysAsync(
          Guid studentId,
          List<Guid> batchtimingsaturdayids)
        {
            var batchtimingsaturdays = batchtimingsaturdayids.Select(batchtimingsaturdayid =>
                new StudentBatchTimingSaturday
                {
                    StudentId = studentId,
                    BatchTimingSaturdayId = batchtimingsaturdayid,
                }).ToList();

            await _repository.UpdateBatchtimingsaturdayRangeAsync(batchtimingsaturdays);
        }

        /// <inheritdoc/>
        public async Task AddBatchTimingSundaysAsync(
          Guid studentId,
          List<Guid> batchtimingsundayids)
        {
            var batchtimingsundays = batchtimingsundayids.Select(batchtimingsundayid =>
                new StudentBatchTimingSunday
                {
                    StudentId = studentId,
                    BatchTimingSundayId = batchtimingsundayid,
                }).ToList();

            await _repository.AddBatchtimingsundayRangeAsync(batchtimingsundays);
        }

        /// <inheritdoc/>
        public async Task UpdateBatchTimingSundaysAsync(
          Guid studentId,
          List<Guid> batchtimingsundayids)
        {
            var batchtimingsundays = batchtimingsundayids.Select(batchtimingsundayid =>
                new StudentBatchTimingSunday
                {
                    StudentId = studentId,
                    BatchTimingSundayId = batchtimingsundayid,
                }).ToList();

            await _repository.UpdateBatchtimingsundayRangeAsync(batchtimingsundays);
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
