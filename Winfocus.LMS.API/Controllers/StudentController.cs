namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Azure.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;
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
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="studentService">The student service.</param>
        /// <param name="studentAcademicdetailsService">The studentacademic details service.</param>
        /// <param name="studentPersonaldetailsService">The studentpersonal details service.</param>
        /// <param name="authService">The student auth service.</param>
        public StudentController(IStudentService studentService, IStudentAcademicdetailsService studentAcademicdetailsService, IStudentPersonaldetailsService studentPersonaldetailsService, IAuthService authService)
        {
            _studentService = studentService;
            _studentAcademicdetailsService = studentAcademicdetailsService;
            _studentPersonaldetailsService = studentPersonaldetailsService;
            _authService = authService;
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
        public async Task<CommonResponse<StudentDto>> Create([FromForm] StudentRequest request)
        {
            var academicDetails = await _studentAcademicdetailsService.CreateAsync(request.academicdetails);
            if (!academicDetails.Success || academicDetails.Data == null)
            {
                return CommonResponse<StudentDto>.FailureResponse(academicDetails.Message);
            }

            var personalDetails = await _studentPersonaldetailsService.CreateAsync(request.personaldetails);
            if (!personalDetails.Success || personalDetails.Data == null)
            {
                return CommonResponse<StudentDto>.FailureResponse(personalDetails.Message);
            }

            var uploaddocDetails = await _studentAcademicdetailsService.AddUploadedDocuments(request.docdetails);
            if (uploaddocDetails == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("cannot upload documents!");
            }

            StudentDto studentDto = new StudentDto
            {
                StudentAcademicId = academicDetails.Data.Id,
                AcademicDetails = academicDetails.Data,
                StudentPersonalId = personalDetails.Data.Id,
                PersonalDetails = personalDetails.Data,
                StudentDocumentsId = uploaddocDetails.Id,
                StudentDocuments = uploaddocDetails,
                IsScholershipStudent = request.isscholershipstudent,
                //Userid = UserId,
                RegistrationStatus = RegistrationStatus.Draft,
            };

            var created = await _studentService.CreateAsync(studentDto);
            if (created == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("Failed to create student.");
            }

            await _studentAcademicdetailsService.AddCoursesAsync(created.Id, request.academicdetails.courseId);
            await _studentAcademicdetailsService.AddBatchTimingMTFsAsync(created.Id, request.academicdetails.batchTimingMTFIds);
            await _studentAcademicdetailsService.AddBatchTimingSaturdaysAsync(created.Id, request.academicdetails.batchTimingstaurdayIds);
            await _studentAcademicdetailsService.AddBatchTimingSundaysAsync(created.Id, request.academicdetails.batchTimingSundayIds);

            return CommonResponse<StudentDto>.SuccessResponse("Student details", created);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<StudentDto>> Get(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("Student not found");
            }

            return CommonResponse<StudentDto>.SuccessResponse("Student details", student);
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

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<CommonResponse<bool>> Delete(Guid id)
        {
            return await _studentService.DeleteAsync(id);
        }

        /// <summary>
        /// student confirm.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [HttpPost("{id:guid}/confirm")]
        public async Task<CommonResponse<bool>> StudentConfirm(Guid id)
        {
            return await _studentService.StudentConfirm(id);
        }

        /// <summary>
        /// Updates an existing student with academic, personal and document details.
        /// </summary>
        /// <param name="id">Unique identifier of the student.</param>
        /// <param name="request">Student update request containing academic, personal and document details.</param>
        /// <returns>Updated student details.</returns>
        /// <response code="200">Student updated successfully.</response>
        /// <response code="404">Student not found.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<StudentDto>> Update(Guid id, [FromForm] StudentRequest request)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                {
                return NotFound("Student not found.");
             }
            var academicDetails = await _studentAcademicdetailsService.UpdateAsync(student.StudentAcademicId, request.academicdetails);
            if (!academicDetails.Success || academicDetails.Data == null)
            {
                return BadRequest(academicDetails.Message);
            }

            var personalDetails = await _studentPersonaldetailsService.UpdateAsync(student.StudentPersonalId, request.personaldetails);
            if (!personalDetails.Success || personalDetails.Data == null)
            {
                return BadRequest(personalDetails.Message);
            }

            var uploaddocDetails = await _studentAcademicdetailsService.UpdateUploadedDocuments(student.StudentDocumentsId, request.docdetails);
            if (uploaddocDetails == null)
            {
                return BadRequest();
            }

            await _studentAcademicdetailsService.UpdateCoursesAsync(student.Id, request.academicdetails.courseId);
            await _studentAcademicdetailsService.UpdateBatchTimingMTFsAsync(student.Id, request.academicdetails.batchTimingMTFIds);
            await _studentAcademicdetailsService.UpdateBatchTimingSaturdaysAsync(student.Id, request.academicdetails.batchTimingstaurdayIds);
            await _studentAcademicdetailsService.UpdateBatchTimingSundaysAsync(student.Id, request.academicdetails.batchTimingSundayIds);

            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        /// <summary>
        /// student confirm.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [HttpPost("{id:guid}/approve")]
        public async Task<CommonResponse<bool>> StudentApprove(Guid id)
        {
            var response = await _studentService.StudentApprove(id);
            var student = await _studentService.GetByIdAsync(id);

            if (response.Success && student != null)
            {
                var username = student.PersonalDetails.FullName
                    ?.Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault();

                RegisterRequestDto obj = new RegisterRequestDto(
                    username,
                    student.PersonalDetails.EmailAddress,
                    "Test@123",
                    new List<string> { "Student" }
                );

                var result = await _authService.RegisterAsync(obj);
            }

            return response;
        }
    }
}
