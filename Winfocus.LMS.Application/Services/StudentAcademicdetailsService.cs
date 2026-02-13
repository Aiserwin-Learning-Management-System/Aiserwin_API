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
        private readonly IStudentAcademicdeatilsRepository _repository;
        private readonly ILogger<StateService> _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IModeOfStudyRepository _modeOfStudyRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICentreRepository _centerRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IStreamRepository _streamRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubjectRepository _subjectRepository;

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
        public StudentAcademicdetailsService(IStudentAcademicdeatilsRepository repository, ILogger<StateService> logger, ICountryRepository countryRepository, IModeOfStudyRepository modeOfStudyRepository, IStateRepository stateRepository, ICentreRepository centerRepository, ISyllabusRepository syllabusRepository, IGradeRepository gradeRepository, IStreamRepository streamRepository, ICourseRepository courseRepository, ISubjectRepository subjectRepository)
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
            var countryTask = _countryRepository.GetByIdAsync(request.countryId);
            var stateTask = _stateRepository.GetByIdAsync(request.stateId);
            var modeOfStudyTask = _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            var centerTask = _centerRepository.GetByIdAsync(request.centerId);
            var syllabusTask = _syllabusRepository.GetByIdAsync(request.syllabusId);
            var gradeTask = _gradeRepository.GetByIdAsync(request.gradeId);
            var streamTask = _streamRepository.GetByIdAsync(request.streamId);

            await Task.WhenAll(
                countryTask,
                stateTask,
                modeOfStudyTask,
                centerTask,
                syllabusTask,
                gradeTask,
                streamTask
            );

            var country = await countryTask ?? throw new InvalidOperationException("Country not found");
            if (!country.IsActive)
                throw new InvalidOperationException("Cannot create with inactive country");

            _ = await stateTask ?? throw new InvalidOperationException("State not found");
            _ = await modeOfStudyTask ?? throw new InvalidOperationException("Mode of study not found");
            _ = await centerTask ?? throw new InvalidOperationException("Center not found");
            _ = await syllabusTask ?? throw new InvalidOperationException("Syllabus not found");
            _ = await gradeTask ?? throw new InvalidOperationException("Grade not found");
            _ = await streamTask ?? throw new InvalidOperationException("Stream not found");

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
            };

            var created = await _repository.AddAsync(academicDetails);

            _logger.LogInformation(
                "Student academic details created successfully. Id: {Id}",
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
            _logger.LogInformation("Updating StudentAcademicDetails Id: {Id}", id);

            var academicDetails = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Student academic details not found");

            var countryTask = _countryRepository.GetByIdAsync(request.countryId);
            var stateTask = _stateRepository.GetByIdAsync(request.stateId);
            var modeOfStudyTask = _modeOfStudyRepository.GetByIdAsync(request.modeOfStudyId);
            var centerTask = _centerRepository.GetByIdAsync(request.centerId);
            var syllabusTask = _syllabusRepository.GetByIdAsync(request.syllabusId);
            var gradeTask = _gradeRepository.GetByIdAsync(request.gradeId);
            var streamTask = _streamRepository.GetByIdAsync(request.streamId);

            await Task.WhenAll(
                countryTask,
                stateTask,
                modeOfStudyTask,
                centerTask,
                syllabusTask,
                gradeTask,
                streamTask
            );

            var country = await countryTask ?? throw new InvalidOperationException("Country not found");

            if (!country.IsActive)
                throw new InvalidOperationException("Cannot update with inactive country");

            _ = await stateTask ?? throw new InvalidOperationException("State not found");
            _ = await modeOfStudyTask ?? throw new InvalidOperationException("Mode of study not found");
            _ = await centerTask ?? throw new InvalidOperationException("Center not found");
            _ = await syllabusTask ?? throw new InvalidOperationException("Syllabus not found");
            _ = await gradeTask ?? throw new InvalidOperationException("Grade not found");
            _ = await streamTask ?? throw new InvalidOperationException("Stream not found");


            academicDetails.CountryId = request.countryId;
            academicDetails.StateId = request.stateId;
            academicDetails.ModeOfStudyId = request.modeOfStudyId;
            academicDetails.CenterId = request.centerId;
            academicDetails.SyllabusId = request.syllabusId;
            academicDetails.GradeId = request.gradeId;
            academicDetails.StreamId = request.streamId;
            academicDetails.BatchId = request.batchId;
            academicDetails.PreferredBatchTimeId = request.preferredbatchtimeId;
            academicDetails.PastYearPerformance = request.pastyearperformance;
            academicDetails.PastSchoolName = request.pastschoolname;
            academicDetails.PastSchoolLocation = request.pastschoollocation;
            academicDetails.Emirates = request.emirates;

            academicDetails.UpdatedBy = request.userid;
            academicDetails.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(academicDetails);

            _logger.LogInformation(
                "Student academic details updated successfully. Id: {Id}",
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


        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StudentDocumentsDto> AddUploadedDocuments(StudentUploaddocumetsRequest request)
        {
            var studentDocuments = new StudentDocuments
            {
                StudentPhotoPath = request.studentphoto,
                StudentSignaturePath = request.signature,
                IsAcceptedAgreement = request.isAcceptedAgreement,
                IsAcceptedTermsAndConditions = request.isAcceptedTersms,
            };

            var created = await _repository.AddUploadedDocuments(studentDocuments);

            _logger.LogInformation(
                "Student details created successfully. Id: {Id}",
                created.Id);

            return MapDocs(created);
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
