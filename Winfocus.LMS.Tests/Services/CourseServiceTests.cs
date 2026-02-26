namespace Winfocus.LMS.Application.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Domain.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for CourseService to validate business logic, mapping, validation rules,
    /// and proper interaction with the repository layer.
    /// </summary>
    public sealed class CourseServiceTests
    {
        /// <summary>
        /// Mock instance of the ICourseRepository for testing.
        /// </summary>
        private readonly Mock<ICourseRepository> _mockRepository;

        /// <summary>
        /// Mock instance of ILogger for testing logging behavior.
        /// </summary>
        private readonly Mock<ILogger<CourseService>> _mockLogger;

        /// <summary>
        /// Instance of CourseService under test.
        /// </summary>
        private readonly CourseService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseServiceTests"/> class.
        /// Sets up mock dependencies and service instance for testing.
        /// </summary>
        public CourseServiceTests()
        {
            try
            {
                _mockRepository = new Mock<ICourseRepository>();
                _mockLogger = new Mock<ILogger<CourseService>>();
                _service = new CourseService(_mockRepository.Object, _mockLogger.Object);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize CourseServiceTests: {ex.Message}");
                throw;
            }
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Tests that GetAllAsync returns success response with courses when courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_WhenCoursesExist_ReturnsSuccessResponseWithCourses()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mathematics 101",
                        SubjectId = subjectId,
                        GradeId = Guid.NewGuid(),
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Subject = new Subject
                        {
                            Id = subjectId,
                            Name = "Mathematics",
                            IsActive = true,
                        },
                    },
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Physics 101",
                        SubjectId = subjectId,
                        GradeId = Guid.NewGuid(),
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Subject = new Subject
                        {
                            Id = subjectId,
                            Name = "Physics",
                            IsActive = true,
                        },
                    },
                };

                _mockRepository
                    .Setup(r => r.GetAllAsync())
                    .ReturnsAsync(courses);

                // Act
                var result = await _service.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Courses retrieved successfully", result.Message);
                Assert.NotNull(result.Data);
                Assert.Equal(2, result.Data.Count);
                Assert.Equal("Mathematics 101", result.Data[0].Name);
                Assert.Equal("Physics 101", result.Data[1].Name);

                _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_WhenCoursesExist_ReturnsSuccessResponseWithCourses failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync returns success response with empty list when no courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_WhenNoCoursesExist_ReturnsSuccessResponseWithEmptyList()
        {
            try
            {
                // Arrange
                var emptyCourses = new List<Course>();

                _mockRepository
                    .Setup(r => r.GetAllAsync())
                    .ReturnsAsync(emptyCourses);

                // Act
                var result = await _service.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Courses retrieved successfully", result.Message);
                Assert.NotNull(result.Data);
                Assert.Empty(result.Data);

                _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_WhenNoCoursesExist_ReturnsSuccessResponseWithEmptyList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync correctly maps entity to DTO.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_CorrectlyMapsEntitiesToDTOs()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var courseId = Guid.NewGuid();
                var gradeId = Guid.NewGuid();
                var academicYear = Guid.NewGuid();

                var courses = new List<Course>
                {
                    new Course
                    {
                        Id = courseId,
                        Name = "Chemistry 101",
                        SubjectId = subjectId,
                        GradeId = gradeId,
                        IsActive = true,
                        Subject = new Subject
                        {
                            Id = subjectId,
                            Name = "Chemistry",
                        },
                    },
                };

                _mockRepository
                    .Setup(r => r.GetAllAsync())
                    .ReturnsAsync(courses);

                // Act
                var result = await _service.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Single(result.Data);

                var dto = result.Data[0];
                Assert.Equal(courseId, dto.Id);
                Assert.Equal("Chemistry 101", dto.Name);
                Assert.Equal(gradeId, dto.GradeId);
                Assert.NotNull(dto.Subject);
                Assert.Equal(subjectId, dto.Subject.Id);
                Assert.Equal("Chemistry", dto.Subject.Name);

                _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_CorrectlyMapsEntitiesToDTOs failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync propagates repository exceptions.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            try
            {
                // Arrange
                _mockRepository
                    .Setup(r => r.GetAllAsync())
                    .ThrowsAsync(new InvalidOperationException("Database connection failed"));

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetAllAsync());

                _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_WhenRepositoryThrowsException_PropagatesException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Tests that GetByIdAsync returns success response when course exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenCourseExists_ReturnsSuccessResponse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var subjectId = Guid.NewGuid();

                var course = new Course
                {
                    Id = courseId,
                    Name = "Biology 101",
                    SubjectId = subjectId,
                    GradeId = Guid.NewGuid(),
                    IsActive = true,
                    Subject = new Subject
                    {
                        Id = subjectId,
                        Name = "Biology",
                    },
                };

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ReturnsAsync(course);

                // Act
                var result = await _service.GetByIdAsync(courseId);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Courses retrieved successfully", result.Message);
                Assert.NotNull(result.Data);
                Assert.Equal(courseId, result.Data.Id);
                Assert.Equal("Biology 101", result.Data.Name);

                _mockRepository.Verify(r => r.GetByIdAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenCourseExists_ReturnsSuccessResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdAsync returns failure response when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenCourseDoesNotExist_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ReturnsAsync((Course?)null);

                // Act
                var result = await _service.GetByIdAsync(courseId);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Equal("Course not found", result.Message);
                Assert.Null(result.Data);

                _mockRepository.Verify(r => r.GetByIdAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenCourseDoesNotExist_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdAsync handles repository exceptions.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ThrowsAsync(new InvalidOperationException("Database error"));

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.GetByIdAsync(courseId));

                _mockRepository.Verify(r => r.GetByIdAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenRepositoryThrowsException_PropagatesException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByStreamAsync Tests

        /// <summary>
        /// Tests that GetByStreamAsync returns success response when courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_WhenCoursesExist_ReturnsSuccessResponse()
        {
            try
            {
                // Arrange
                var streamId = Guid.NewGuid();
                var courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Course 1",
                        StreamId = streamId,
                        Subject = new Subject { Id = Guid.NewGuid(), Name = "Subject 1" },
                    },
                };

                _mockRepository
                    .Setup(r => r.GetByStreamAsync(streamId))
                    .ReturnsAsync(courses);

                // Act
                var result = await _service.GetByStreamAsync(streamId);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Courses retrieved successfully", result.Message);
                Assert.Single(result.Data);

                _mockRepository.Verify(r => r.GetByStreamAsync(streamId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_WhenCoursesExist_ReturnsSuccessResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByStreamAsync returns failure response when no courses found.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_WhenNoCoursesFound_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var streamId = Guid.NewGuid();
                var emptyCourses = new List<Course>();

                _mockRepository
                    .Setup(r => r.GetByStreamAsync(streamId))
                    .ReturnsAsync(emptyCourses);

                // Act
                var result = await _service.GetByStreamAsync(streamId);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Equal("No courses found for the given stream", result.Message);

                _mockRepository.Verify(r => r.GetByStreamAsync(streamId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_WhenNoCoursesFound_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetBySubjectAsync Tests

        /// <summary>
        /// Tests that GetBySubjectAsync returns success response when courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubjectAsync_WhenCoursesExist_ReturnsSuccessResponse()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Course 1",
                        SubjectId = subjectId,
                        Subject = new Subject { Id = subjectId, Name = "Subject 1" },
                    },
                };

                _mockRepository
                    .Setup(r => r.GetBySubjectAsync(subjectId))
                    .ReturnsAsync(courses);

                // Act
                var result = await _service.GetBySubjectAsync(subjectId);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Courses retrieved successfully", result.Message);
                Assert.Single(result.Data);

                _mockRepository.Verify(r => r.GetBySubjectAsync(subjectId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubjectAsync_WhenCoursesExist_ReturnsSuccessResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetBySubjectAsync returns failure response when no courses found.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubjectAsync_WhenNoCoursesFound_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var subjectId = Guid.NewGuid();
                var emptyCourses = new List<Course>();

                _mockRepository
                    .Setup(r => r.GetBySubjectAsync(subjectId))
                    .ReturnsAsync(emptyCourses);

                // Act
                var result = await _service.GetBySubjectAsync(subjectId);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Equal("No courses found for the given subject", result.Message);

                _mockRepository.Verify(r => r.GetBySubjectAsync(subjectId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubjectAsync_WhenNoCoursesFound_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Tests that CreateAsync creates course successfully with valid request.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task CreateAsync_WhenValidRequest_CreatesCourseSucessfully()
        {
            try
            {
                // Arrange
                var request = new CourseRequest(
                    coursename: "Test Course",
                    streamid: Guid.NewGuid(),
                    userId: Guid.NewGuid());

                var createdCourse = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = request.coursename,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Subject = new Subject
                    {
                        Name = "Computer Science",
                    },
                };

                _mockRepository
                    .Setup(r => r.AddAsync(It.IsAny<Course>()))
                    .ReturnsAsync(createdCourse);

                // Act
                var result = await _service.CreateAsync(request);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Course created successfully", result.Message);
                Assert.NotNull(result.Data);
                Assert.Equal(request.coursename, result.Data.Name);

                _mockRepository.Verify(
                    r => r.AddAsync(It.Is<Course>(c =>
                    c.Name == request.coursename
                )), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateAsync_WhenValidRequest_CreatesCourseSucessfully failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that CreateAsync sets CreatedAt timestamp.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task CreateAsync_SetsCreatedAtTimestamp()
        {
            try
            {
                // Arrange
                var request = new CourseRequest(
                    coursename: "Test Course",
                    streamid: Guid.NewGuid(),
                    userId: Guid.NewGuid());

                var beforeCreate = DateTime.UtcNow;

                _mockRepository
                    .Setup(r => r.AddAsync(It.IsAny<Course>()))
                    .ReturnsAsync((Course c) =>
                    {
                        c.Id = Guid.NewGuid();
                        c.Subject = new Subject { Name = "Test Subject" };
                        return c;
                    });

                // Act
                var result = await _service.CreateAsync(request);

                var afterCreate = DateTime.UtcNow;

                // Assert
                _mockRepository.Verify(
                    r => r.AddAsync(It.Is<Course>(c =>
                    c.CreatedAt >= beforeCreate && c.CreatedAt <= afterCreate
                )), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateAsync_SetsCreatedAtTimestamp failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that CreateAsync handles repository exceptions.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task CreateAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            try
            {
                // Arrange
                var request = new CourseRequest(
                    coursename: "Test Course",
                    streamid: Guid.NewGuid(),
                    userId: Guid.NewGuid());

                _mockRepository
                    .Setup(r => r.AddAsync(It.IsAny<Course>()))
                    .ThrowsAsync(new InvalidOperationException("Database error"));

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.CreateAsync(request));

                _mockRepository.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateAsync_WhenRepositoryThrowsException_PropagatesException failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Tests that UpdateAsync updates course successfully when course exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateAsync_WhenCourseExists_UpdatesSuccessfully()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var existingCourse = new Course
                {
                    Id = courseId,
                    Name = "Old Name",
                    SubjectId = Guid.NewGuid(),
                    GradeId = Guid.NewGuid(),
                    IsActive = true,
                    Subject = new Subject { Id = Guid.NewGuid(), Name = "Old Subject" },
                };

                var request = new CourseRequest(
                    coursename: "Test Course",
                    streamid: Guid.NewGuid(),
                    userId: Guid.NewGuid());

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ReturnsAsync(existingCourse);

                _mockRepository
                    .Setup(r => r.UpdateAsync(It.IsAny<Course>()))
                    .ReturnsAsync((Course c) =>
                    {
                        c.Subject = new Subject { Name = "Updated Subject" };
                        return c;
                    });

                // Act
                var result = await _service.UpdateAsync(courseId, request);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Course updated successfully", result.Message);
                Assert.NotNull(result.Data);
                Assert.Equal("Updated Name", result.Data.Name);

                _mockRepository.Verify(r => r.GetByIdAsync(courseId), Times.Once);
                _mockRepository.Verify(
                    r => r.UpdateAsync(It.Is<Course>(c =>
                    c.Name == request.coursename &&
                    c.UpdatedAt != null
                )), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_WhenCourseExists_UpdatesSuccessfully failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that UpdateAsync throws KeyNotFoundException when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateAsync_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var request = new CourseRequest(
                    coursename: "Test Course",
                    streamid: Guid.NewGuid(),
                    userId: Guid.NewGuid());

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ReturnsAsync((Course?)null);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _service.UpdateAsync(courseId, request));

                Assert.Equal("Course not found", exception.Message);

                _mockRepository.Verify(r => r.GetByIdAsync(courseId), Times.Once);
                _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Course>()), Times.Never);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_WhenCourseDoesNotExist_ThrowsKeyNotFoundException failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that UpdateAsync sets UpdatedAt timestamp.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateAsync_SetsUpdatedAtTimestamp()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();
                var existingCourse = new Course
                {
                    Id = courseId,
                    Name = "Test",
                    IsActive = true,
                    Subject = new Subject { Id = Guid.NewGuid(), Name = "Test" },
                };

                var request = new CourseRequest(
                     coursename: "Test Course",
                     streamid: Guid.NewGuid(),
                     userId: Guid.NewGuid());

                var beforeUpdate = DateTime.UtcNow;

                _mockRepository
                    .Setup(r => r.GetByIdAsync(courseId))
                    .ReturnsAsync(existingCourse);

                _mockRepository
                    .Setup(r => r.UpdateAsync(It.IsAny<Course>()))
                    .ReturnsAsync((Course c) =>
                    {
                        c.Subject = new Subject { Name = "Updated" };
                        return c;
                    });

                // Act
                await _service.UpdateAsync(courseId, request);

                var afterUpdate = DateTime.UtcNow;

                // Assert
                _mockRepository.Verify(
                    r => r.UpdateAsync(It.Is<Course>(c =>
                    c.UpdatedAt != null &&
                    c.UpdatedAt >= beforeUpdate &&
                    c.UpdatedAt <= afterUpdate
                )), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_SetsUpdatedAtTimestamp failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Tests that DeleteAsync returns success response when deletion succeeds.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_WhenDeletionSucceeds_ReturnsSuccessResponse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();

                _mockRepository
                    .Setup(r => r.SoftDeleteAsync(courseId))
                    .ReturnsAsync(true);

                // Act
                var result = await _service.DeleteAsync(courseId);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal("Course deleted successfully", result.Message);
                Assert.True(result.Data);

                _mockRepository.Verify(r => r.SoftDeleteAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] DeleteAsync_WhenDeletionSucceeds_ReturnsSuccessResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that DeleteAsync returns failure response when course not found.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_WhenCourseNotFound_ReturnsFailureResponse()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();

                _mockRepository
                    .Setup(r => r.SoftDeleteAsync(courseId))
                    .ReturnsAsync(false);

                // Act
                var result = await _service.DeleteAsync(courseId);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Equal("Course not found or could not be deleted", result.Message);

                _mockRepository.Verify(r => r.SoftDeleteAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] DeleteAsync_WhenCourseNotFound_ReturnsFailureResponse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that DeleteAsync handles repository exceptions.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            try
            {
                // Arrange
                var courseId = Guid.NewGuid();

                _mockRepository
                    .Setup(r => r.SoftDeleteAsync(courseId))
                    .ThrowsAsync(new InvalidOperationException("Database error"));

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.DeleteAsync(courseId));

                _mockRepository.Verify(r => r.SoftDeleteAsync(courseId), Times.Once);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] DeleteAsync_WhenRepositoryThrowsException_PropagatesException failed: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
