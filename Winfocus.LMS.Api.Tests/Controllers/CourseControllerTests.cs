namespace Winfocus.LMS.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Winfocus.LMS.API.Controllers;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Xunit;

    /// <summary>
    /// Unit tests for CourseController to validate HTTP responses, proper delegation to service layer,
    /// and correct handling of success and failure scenarios.
    /// </summary>
    public sealed class CourseControllerTests
    {
        /// <summary>
        /// Mock instance of the ICourseService for testing.
        /// </summary>
        private readonly Mock<ICourseService> _mockService;

        /// <summary>
        /// Mock instance of ILogger for testing logging behavior.
        /// </summary>
        private readonly Mock<ILogger<CourseController>> _mockLogger;

        /// <summary>
        /// Instance of CourseController under test.
        /// </summary>
        private readonly CourseController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseControllerTests"/> class.
        /// Sets up mock dependencies and controller instance for testing.
        /// </summary>
        public CourseControllerTests()
        {
            try
            {
                _mockService = new Mock<ICourseService>();
                _mockLogger = new Mock<ILogger<CourseController>>();
                _controller = new CourseController(_mockService.Object);
            }
            catch (Exception ex)
            {
                // Log initialization failure
                Console.WriteLine($"[ERROR] Failed to initialize CourseControllerTests: {ex.Message}");
                throw;
            }
        }

        #region GetAll Tests

        /// <summary>
        /// Tests that GetAll returns 200 OK with a list of courses when service returns success response.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAll_WhenCoursesExist_ReturnsOkWithCoursesList()
        {
            try
            {
                // Arrange
                var expectedCourses = new List<CourseDto>
                {
                    new CourseDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mathematics 101",
                        GradeId = Guid.NewGuid(),
                        CourseDescription = "Introduction to Mathematics",
                        CourseUrl = "https://example.com/math101",
                        MaxStudent = 30,
                        AcademicYear = Guid.NewGuid(),
                        Status = "Active",
                        Subject = new SubjectDto
                        {
                            Id = Guid.NewGuid(),
                            Name = "Mathematics",
                        },
                    },
                };

                var response = CommonResponse<List<CourseDto>>.SuccessResponse(
                    "Courses retrieved successfully",
                    expectedCourses);

                _mockService
                    .Setup(s => s.GetAllAsync())
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetAll();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Single(returnedResponse.Data);
                Assert.Equal("Mathematics 101", returnedResponse.Data[0].Name);

                _mockService.Verify(s => s.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAll_WhenCoursesExist_ReturnsOkWithCoursesList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAll returns 200 OK with an empty list when no courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAll_WhenNoCoursesExist_ReturnsOkWithEmptyList()
        {
            try
            {
                // Arrange
                var response = CommonResponse<List<CourseDto>>.SuccessResponse(
                    "Courses retrieved successfully",
                    new List<CourseDto>());

                _mockService
                    .Setup(s => s.GetAllAsync())
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetAll();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Empty(returnedResponse.Data);

                _mockService.Verify(s => s.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAll_WhenNoCoursesExist_ReturnsOkWithEmptyList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAll handles service exceptions gracefully.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAll_WhenServiceThrowsException_ThrowsException()
        {
            try
            {
                // Arrange
                _mockService
                    .Setup(s => s.GetAllAsync())
                    .ThrowsAsync(new InvalidOperationException("Database connection failed"));

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());

                _mockService.Verify(s => s.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAll_WhenServiceThrowsException_ThrowsException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Get By Id Tests

        /// <summary>
        /// Tests that Get returns 200 OK with course details when course exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Get_WhenCourseExists_ReturnsOkWithCourse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var expectedCourse = new CourseDto
                {
                    Id = courseId,
                    Name = "Physics 101",
                    GradeId = Guid.NewGuid(),
                    CourseDescription = "Introduction to Physics",
                    CourseUrl = "https://example.com/physics101",
                    MaxStudent = 25,
                    AcademicYear = Guid.NewGuid(),
                    Status = "Active",
                    Subject = new SubjectDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "Physics",
                    },
                };

                var response = CommonResponse<CourseDto>.SuccessResponse(
                    "Course retrieved successfully",
                    expectedCourse);

                _mockService
                    .Setup(s => s.GetByIdAsync(courseId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Get(courseId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<CourseDto>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Equal(courseId, returnedResponse.Data.Id);
                Assert.Equal("Physics 101", returnedResponse.Data.Name);

                _mockService.Verify(s => s.GetByIdAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Get_WhenCourseExists_ReturnsOkWithCourse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that Get returns failure response when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Get_WhenCourseDoesNotExist_ReturnsOkWithFailureResponse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var response = CommonResponse<CourseDto>.FailureResponse("Course not found");

                _mockService
                    .Setup(s => s.GetByIdAsync(courseId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Get(courseId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedResponse = Assert.IsType<CommonResponse<CourseDto>>(okResult.Value);
                Assert.False(returnedResponse.Success);
                Assert.Equal("Course not found", returnedResponse.Message);

                _mockService.Verify(s => s.GetByIdAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Get_WhenCourseDoesNotExist_ReturnsOkWithFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that Get handles invalid GUID gracefully.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Get_WhenInvalidGuid_ServiceIsStillCalled()
        {
            try
            {
                // Arrange
                var invalidId = Guid.Empty;
                var response = CommonResponse<CourseDto>.FailureResponse("Course not found");

                _mockService
                    .Setup(s => s.GetByIdAsync(invalidId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Get(invalidId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.NotNull(okResult);

                _mockService.Verify(s => s.GetByIdAsync(invalidId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Get_WhenInvalidGuid_ServiceIsStillCalled failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByStream Tests

        /// <summary>
        /// Tests that GetByStream returns 200 OK with courses when courses exist for the stream.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStream_WhenCoursesExist_ReturnsOkWithCoursesList()
        {
            try
            {
                // Arrange
                var streamId = Guid.NewGuid();
                var expectedCourses = new List<CourseDto>
                {
                    new CourseDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chemistry 101",
                        StreamId = streamId,
                        Subject = new SubjectDto { Id = Guid.NewGuid(), Name = "Chemistry" },
                    },
                };

                var response = CommonResponse<List<CourseDto>>.SuccessResponse(
                    "Courses retrieved successfully",
                    expectedCourses);

                _mockService
                    .Setup(s => s.GetByStreamAsync(streamId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetByStream(streamId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Single(returnedResponse.Data);

                _mockService.Verify(s => s.GetByStreamAsync(streamId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStream_WhenCoursesExist_ReturnsOkWithCoursesList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByStream returns failure response when no courses found for stream.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStream_WhenNoCoursesFound_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var streamId = Guid.NewGuid();
                var response = CommonResponse<List<CourseDto>>.FailureResponse(
                    "No courses found for the given stream");

                _mockService
                    .Setup(s => s.GetByStreamAsync(streamId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetByStream(streamId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.False(returnedResponse.Success);

                _mockService.Verify(s => s.GetByStreamAsync(streamId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStream_WhenNoCoursesFound_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetBySubject Tests

        /// <summary>
        /// Tests that GetBySubject returns 200 OK with courses when courses exist for the subject.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubject_WhenCoursesExist_ReturnsOkWithCoursesList()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var expectedCourses = new List<CourseDto>
                {
                    new CourseDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "Biology 101",
                        Subject = new SubjectDto { Id = subjectId, Name = "Biology" },
                    },
                };

                var response = CommonResponse<List<CourseDto>>.SuccessResponse(
                    "Courses retrieved successfully",
                    expectedCourses);

                _mockService
                    .Setup(s => s.GetBySubjectAsync(subjectId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetBySubject(subjectId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Single(returnedResponse.Data);

                _mockService.Verify(s => s.GetBySubjectAsync(subjectId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubject_WhenCoursesExist_ReturnsOkWithCoursesList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetBySubject returns failure response when no courses found for subject.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubject_WhenNoCoursesFound_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var response = CommonResponse<List<CourseDto>>.FailureResponse(
                    "No courses found for the given subject");

                _mockService
                    .Setup(s => s.GetBySubjectAsync(subjectId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.GetBySubject(subjectId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedResponse = Assert.IsType<CommonResponse<List<CourseDto>>>(okResult.Value);
                Assert.False(returnedResponse.Success);

                _mockService.Verify(s => s.GetBySubjectAsync(subjectId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubject_WhenNoCoursesFound_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Create Tests

        /// <summary>
        /// Tests that Create returns 200 OK with created course when request is valid.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Create_WhenValidRequest_ReturnsOkWithCreatedCourse()
        {
            try
            {
                // Arrange
                var request = new CourseRequest(
                    coursename: "Computer Science 101",
                    subjectid: Guid.NewGuid(),
                    gradeid: Guid.NewGuid(),
                    cousedescription: "Introduction to Computer Science",
                    courseurl: "https://example.com/cs101",
                    maxstudent: 40,
                    academicyear: Guid.NewGuid(),
                    status: "Active");

                var createdCourse = new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Name = request.coursename,
                    CourseDescription = request.cousedescription,
                    Subject = new SubjectDto { Id = request.subjectid, Name = "Computer Science" },
                };

                var response = CommonResponse<CourseDto>.SuccessResponse(
                    "Course created successfully",
                    createdCourse);

                _mockService
                    .Setup(s => s.CreateAsync(It.IsAny<CourseRequest>()))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Create(request);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);

                var returnedResponse = Assert.IsType<CommonResponse<CourseDto>>(okResult.Value);
                Assert.True(returnedResponse.Success);
                Assert.Equal(request.coursename, returnedResponse.Data.Name);

                _mockService.Verify(s => s.CreateAsync(It.IsAny<CourseRequest>()), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Create_WhenValidRequest_ReturnsOkWithCreatedCourse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that Create handles null request appropriately.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Create_WhenNullRequest_ServiceThrowsArgumentNullException()
        {
            try
            {
                // Arrange
                _mockService
                    .Setup(s => s.CreateAsync(null!))
                    .ThrowsAsync(new ArgumentNullException("request"));

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Create(null!));

                _mockService.Verify(s => s.CreateAsync(null!), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Create_WhenNullRequest_ServiceThrowsArgumentNullException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Update Tests

        /// <summary>
        /// Tests that Update returns 204 No Content when update is successful.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Update_WhenValidRequest_ReturnsNoContent()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var request = new CourseRequest(
                    coursename: "Updated Course Name",
                    subjectid: Guid.NewGuid(),
                    gradeid: Guid.NewGuid(),
                    cousedescription: "Updated description",
                    courseurl: "https://example.com/updated",
                    maxstudent: 35,
                    academicyear: Guid.NewGuid(),
                    status: "Active");

                var updatedCourse = new CourseDto
                {
                    Id = courseId,
                    Name = request.coursename,
                };

                var response = CommonResponse<CourseDto>.SuccessResponse(
                    "Course updated successfully",
                    updatedCourse);

                _mockService
                    .Setup(s => s.UpdateAsync(courseId, It.IsAny<CourseRequest>()))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Update(courseId, request);

                // Assert
                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);

                _mockService.Verify(s => s.UpdateAsync(courseId, It.IsAny<CourseRequest>()), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Update_WhenValidRequest_ReturnsNoContent failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that Update throws KeyNotFoundException when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Update_WhenCourseNotFound_ThrowsKeyNotFoundException()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var request = new CourseRequest(
                    coursename: "Test Course",
                    subjectid: Guid.NewGuid(),
                    gradeid: Guid.NewGuid(),
                    cousedescription: "Test",
                    courseurl: "https://test.com",
                    maxstudent: 30,
                    academicyear: Guid.NewGuid(),
                    status: "Active");

                _mockService
                    .Setup(s => s.UpdateAsync(courseId, It.IsAny<CourseRequest>()))
                    .ThrowsAsync(new KeyNotFoundException("Course not found"));

                // Act & Assert
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _controller.Update(courseId, request));

                _mockService.Verify(s => s.UpdateAsync(courseId, It.IsAny<CourseRequest>()), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Update_WhenCourseNotFound_ThrowsKeyNotFoundException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Delete Tests

        /// <summary>
        /// Tests that Delete returns 204 No Content when deletion is successful.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Delete_WhenCourseExists_ReturnsNoContent()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var response = CommonResponse<bool>.SuccessResponse(
                    "Course deleted successfully",
                    true);

                _mockService
                    .Setup(s => s.DeleteAsync(courseId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Delete(courseId);

                // Assert
                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);

                _mockService.Verify(s => s.DeleteAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Delete_WhenCourseExists_ReturnsNoContent failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that Delete returns no content even when course is not found (idempotent).
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task Delete_WhenCourseNotFound_ReturnsNoContent()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var response = CommonResponse<bool>.FailureResponse(
                    "Course not found or could not be deleted");

                _mockService
                    .Setup(s => s.DeleteAsync(courseId))
                    .ReturnsAsync(response);

                // Act
                var result = await _controller.Delete(courseId);

                // Assert
                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);

                _mockService.Verify(s => s.DeleteAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Delete_WhenCourseNotFound_ReturnsNoContent failed: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
