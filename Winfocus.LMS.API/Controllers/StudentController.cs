namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Azure.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Auth;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data;

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
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="studentService">The student service.</param>
        /// <param name="studentAcademicdetailsService">The studentacademic details service.</param>
        /// <param name="studentPersonaldetailsService">The studentpersonal details service.</param>
        /// <param name="authService">The student auth service.</param>
        /// <param name="dbContext">The application database context.</param>
        public StudentController(IStudentService studentService, IStudentAcademicdetailsService studentAcademicdetailsService, IStudentPersonaldetailsService studentPersonaldetailsService, IAuthService authService, AppDbContext dbContext)
        {
            _studentService = studentService;
            _studentAcademicdetailsService = studentAcademicdetailsService;
            _studentPersonaldetailsService = studentPersonaldetailsService;
            _authService = authService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StudentDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin,CenterAdmin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll()
        {
            var stateId = StateId;
            var countryId = CountryId;
            var centerId = CenterId;
            return Ok(await _studentService.GetAllAsync(countryId, stateId, centerId));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>StudentDto list.</returns>
        [HttpGet("discountrequestlist")]
        public async Task<ActionResult<IReadOnlyList<StudentDto>>> DiscountRequestStudents()
        {
            return Ok(await _studentService.DiscountRequestStudents());
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDto.</returns>
        [HttpPost]
        public async Task<CommonResponse<StudentDto>> Create([FromForm] StudentRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var personalDetails = await _studentPersonaldetailsService.CreateAsync(request.personaldetails);
                if (!personalDetails.Success || personalDetails.Data == null)
                {
                    await transaction.RollbackAsync();
                    return CommonResponse<StudentDto>.FailureResponse(personalDetails.Message);
                }
                var academicDetails = await _studentAcademicdetailsService.CreateAsync(request.academicdetails);
                if (!academicDetails.Success || academicDetails.Data == null)
                {
                    await transaction.RollbackAsync();
                    return CommonResponse<StudentDto>.FailureResponse(academicDetails.Message);
                }

                var uploaddocDetails = await _studentAcademicdetailsService.AddUploadedDocuments(request.docdetails);
                if (uploaddocDetails == null)
                {
                    await transaction.RollbackAsync();
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
                    await transaction.RollbackAsync();
                    return CommonResponse<StudentDto>.FailureResponse("Failed to create student.");
                }

                await _studentAcademicdetailsService.AddCoursesAsync(created.Id, request.academicdetails.courseId);
                await _studentAcademicdetailsService.AddBatchTimingMTFsAsync(created.Id, request.academicdetails.batchTimingMTFIds);
                await _studentAcademicdetailsService.AddBatchTimingSaturdaysAsync(created.Id, request.academicdetails.batchTimingstaurdayIds);
                await _studentAcademicdetailsService.AddBatchTimingSundaysAsync(created.Id, request.academicdetails.batchTimingSundayIds);

                await transaction.CommitAsync();
                return CommonResponse<StudentDto>.SuccessResponse("Student details", created);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return CommonResponse<StudentDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="studentId">The identifier.</param>
        /// <returns>StudentDto by id.</returns>
        [HttpGet("{studentId:guid}")]
        public async Task<CommonResponse<StudentDto>> Get(Guid studentId)
        {
            var student = await _studentService.GetByIdsAsync(studentId, CountryId, StateId, CenterId);
            if (student == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("Student not found");
            }

            return CommonResponse<StudentDto>.SuccessResponse("Student details", student);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="userId">The identifier.</param>
        /// <returns>StudentDto by id.</returns>
        [HttpGet("by-user/{userId:guid}")]
        public async Task<CommonResponse<StudentDto>> GetByUserId(Guid userId)
        {
            var student = await _studentService.GetByUserIdsAsync(userId, CountryId, StateId, CenterId);
            if (student == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("Student not found");
            }

            return CommonResponse<StudentDto>.SuccessResponse("Student details", student);
        }

        /// <summary>
        /// Gets the filtered.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Filtered StudentDto list.</returns>
        [Authorize(Roles = "Admin,SuperAdmin,CountryAdmin,CenterAdmin")]
        [HttpGet("student-filter")]
        public async Task<ActionResult<PagedResult<StudentDto>>> GetFiltered([FromQuery] StudentFilterRequest request)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            if (!isSuperAdmin)
            {
                if (request.CountryId.HasValue && request.CountryId != Guid.Empty &&
                    request.CountryId != CountryId)
                {
                    return Forbid("You are not allowed to access this country data.");
                }

                if (request.StateId.HasValue && request.StateId != Guid.Empty &&
                    request.StateId != StateId)
                {
                    return Forbid("You are not allowed to access this state data.");
                }

                if (request.CentreId.HasValue && request.CentreId != Guid.Empty &&
                    request.CentreId != CenterId)
                {
                    return Forbid("You are not allowed to access this center data.");
                }

                request.CountryId = CountryId;

                if (StateId != Guid.Empty)
                    request.StateId = StateId;

                if (CenterId != Guid.Empty)
                    request.CentreId = CenterId;
            }

            var result = await _studentService.GetFilteredAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Authorize(Policy = "CanDeleteStudent")]
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
        public async Task<CommonResponse<StudentDto>> Update(Guid id, [FromForm] StudentRequest request)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return CommonResponse<StudentDto>.FailureResponse("Student not found");
            }

            var personalDetails = await _studentPersonaldetailsService.UpdateAsync(student.StudentPersonalId, request.personaldetails);
            if (!personalDetails.Success || personalDetails.Data == null)
            {
                return CommonResponse<StudentDto>.FailureResponse(personalDetails.Message);
            }

            var academicDetails = await _studentAcademicdetailsService.UpdateAsync(student.StudentAcademicId, request.academicdetails);
            if (!academicDetails.Success || academicDetails.Data == null)
            {
                return CommonResponse<StudentDto>.FailureResponse(academicDetails.Message);
            }

            var uploaddocDetails = await _studentAcademicdetailsService.UpdateUploadedDocuments(student.StudentDocumentsId, request.docdetails);
            if (uploaddocDetails == null)
            {
                return CommonResponse<StudentDto>.FailureResponse(" ");
            }

            await _studentAcademicdetailsService.UpdateCoursesAsync(student.Id, request.academicdetails.courseId);
            await _studentAcademicdetailsService.UpdateBatchTimingMTFsAsync(student.Id, request.academicdetails.batchTimingMTFIds);
            await _studentAcademicdetailsService.UpdateBatchTimingSaturdaysAsync(student.Id, request.academicdetails.batchTimingstaurdayIds);
            await _studentAcademicdetailsService.UpdateBatchTimingSundaysAsync(student.Id, request.academicdetails.batchTimingSundayIds);
            var updatedstudent = await _studentService.GetByIdAsync(id);
            return CommonResponse<StudentDto>.SuccessResponse("Student details", updatedstudent);
            //return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        /// <summary>
        /// student confirm.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
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
                    new List<string> { "Student" },
                    student.AcademicDetails.CountryId,
                    student.AcademicDetails.CenterId,
                    null);

                var result = await _authService.RegisterAsync(obj);

                if (result != null && result.userId != Guid.Empty)
                {
                    await _studentService.LinkUserAsync(id, result.userId);
                }
            }

            return response;
        }

        /// <summary>
        /// request for manual discount access.
        /// </summary>
        /// <param name="studentid">The identifier.</param>
        /// <returns>result.</returns>
        [HttpPut("requestfordiscount/{studentid:guid}")]
        public async Task<CommonResponse<bool>> Requestfordiscount(Guid studentid)
        {
            return await _studentService.Requestfordiscount(studentid);
        }
    }
}
