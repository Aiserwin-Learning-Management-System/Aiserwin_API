namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Handles student CRUD operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IStudentAcademicdetailsService _studentAcademicdetailsService;
        private readonly IStudentPersonaldetailsService _studentPersonaldetailsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="studentService">The student service.</param>
        /// <param name="studentAcademicdetailsService">The studentacademic details service.</param>
        /// <param name="studentPersonaldetailsService">The studentpersonal details service.</param>
        public StudentController(IStudentService studentService, IStudentAcademicdetailsService studentAcademicdetailsService, IStudentPersonaldetailsService studentPersonaldetailsService)
        {
            _studentService = studentService;
            _studentAcademicdetailsService = studentAcademicdetailsService;
            _studentPersonaldetailsService = studentPersonaldetailsService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StudentDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll()
            => Ok(await _studentService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDto.</returns>
        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create([FromForm] StudentRequest request)
        {
            var academicDetails = await _studentAcademicdetailsService.CreateAsync(request.academicdetails);
            if (!academicDetails.Success || academicDetails.Data == null)
            {
                return BadRequest(academicDetails.Message);
            }

            var personalDetails = await _studentPersonaldetailsService.CreateAsync(request.personaldetails);
            if (!personalDetails.Success || personalDetails.Data == null)
            {
                return BadRequest(personalDetails.Message);
            }

            var uploaddocDetails = await _studentAcademicdetailsService.AddUploadedDocuments(request.docdetails);
            if (uploaddocDetails == null)
            {
                return BadRequest();
            }

            StudentDto studentDto = new StudentDto
            {
                StudentAcademicId = academicDetails.Data.Id,
                AcademicDetails = academicDetails.Data,
                StudentPersonalId = personalDetails.Data.Id,
                PersonalDetails = personalDetails.Data,
                StudentDocumentsId = uploaddocDetails.Id,
                StudentDocuments = uploaddocDetails,
                Userid = UserId,
                RegistrationStatus = RegistrationStatus.Draft,
                RegistraionNumber = request.academicdetails.registraionNumber,
            };

            var created = await _studentService.CreateAsync(studentDto);
            if (created == null)
            {
                return BadRequest("Failed to create student.");
            }

            await _studentAcademicdetailsService.AddCoursesAsync(created.Id, request.academicdetails.courseId);
            await _studentAcademicdetailsService.AddBatchTimingMTFsAsync(created.Id, request.academicdetails.batchTimingMTFIds);
            await _studentAcademicdetailsService.AddBatchTimingSaturdaysAsync(created.Id, request.academicdetails.batchTimingstaurdayIds);
            await _studentAcademicdetailsService.AddBatchTimingSundaysAsync(created.Id, request.academicdetails.batchTimingSundayIds);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StudentDto>> Get(Guid id)
        {
            var result = await _studentService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Gets the filtered.
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
        /// <returns>Filtered StudentDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("filter")]
        public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetFiltered(
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
        int limit = 20,
        int offset = 0,
        string sortBy = "FullName",
        string sortOrder = "asc")
        {
            var students = await _studentService.GetFilteredAsync(
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

            return Ok(students);
        }
    }
}
