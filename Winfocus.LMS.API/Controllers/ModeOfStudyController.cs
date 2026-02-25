using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Winfocus.LMS.API.Controllers
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ModeOfStudyController : BaseController
    {
        private readonly IModeOfStudyService _modeofstudyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeOfStudyController"/> class.
        /// </summary>
        /// <param name="modeofstudyService">The modeofstudy service.</param>
        public ModeOfStudyController(IModeOfStudyService modeofstudyService)
        {
            _modeofstudyService = modeofstudyService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>ModeOfStudyDto list.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ModeOfStudyDto>>> GetAll()
            => Ok(await _modeofstudyService.GetAllAsync());

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<CommonResponse<ModeOfStudyDto>> Create(
            ModeOfStudyRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var created = await _modeofstudyService.CreateAsync(updatedRequest);
            if (created == null)
            {
                return CommonResponse<ModeOfStudyDto>.FailureResponse("Failed to create mode of study.");
            }
            else
            {
                return CommonResponse<ModeOfStudyDto>.SuccessResponse("Mode of study created successfully.", created);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("{id:guid}")]
        public async Task<CommonResponse<ModeOfStudyDto>> Get(Guid id)
        {
            var result = await _modeofstudyService.GetByIdAsync(id);
            return result;
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>result.</returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<CommonResponse<ModeOfStudyDto>> Update(
            Guid id,
            ModeOfStudyRequest request)
        {
            var updatedRequest = request with
            {
                userId = UserId
            };
            var result = await _modeofstudyService.UpdateAsync(id, updatedRequest);
            if (result == null)
            {
                return CommonResponse<ModeOfStudyDto>.FailureResponse("Failed to update mode of study.");
            }
            else
            {
                return CommonResponse<ModeOfStudyDto>.SuccessResponse("Mode of study updated successfully.", result);
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="stateid">The identifier.</param>
        /// <returns>ModeOfStudyDto by id.</returns>
        [HttpGet("by-state/{stateid:guid}")]
        public async Task<CommonResponse<List<ModeOfStudyDto>>> GetByStateId(Guid stateid)
        {
            var result = await _modeofstudyService.GetByStateIdAsync(stateid);
            if (result == null)
            {
                return CommonResponse<List<ModeOfStudyDto>>.FailureResponse("Mode of study not found for the given state.");
            }
            else
            {
                return CommonResponse<List<ModeOfStudyDto>>.SuccessResponse("fetching Mode of study for the given state.", result);
            }
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
            bool result = await _modeofstudyService.DeleteAsync(id);
            if (result)
            {
                return CommonResponse<bool>.SuccessResponse("Mode of study deleted successfully.", true);
            }
            else
            {
                return CommonResponse<bool>.FailureResponse("Failed to delete Mode of study.");
            }
        }
    }
}
