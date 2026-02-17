using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;

namespace Winfocus.LMS.API.Controllers
{
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
                Status = "Draft",
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
    }
}
