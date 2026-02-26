namespace Winfocus.LMS.Infrastructure.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;
    using Winfocus.LMS.Infrastructure.Repositories;
    using Xunit;

    /// <summary>
    /// Unit tests for CourseRepository to validate data persistence logic,
    /// query optimization, and database interactions using in-memory database.
    /// </summary>
    public sealed class CourseRepositoryTests : IDisposable
    {
        /// <summary>
        /// In-memory database context for testing.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Instance of CourseRepository under test.
        /// </summary>
        private readonly CourseRepository _repository;

        /// <summary>
        /// Mock logger instance for testing logging behavior.
        /// </summary>
        private readonly Mock<ILogger<CourseRepository>> _mockLogger;

        /// <summary>
        /// Flag to track whether the instance has been disposed.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepositoryTests"/> class.
        /// Sets up in-memory database and repository instance for testing.
        /// </summary>
        public CourseRepositoryTests()
        {
            try
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging()
                    .Options;

                _context = new AppDbContext(options);
                _repository = new CourseRepository(_context);
                _mockLogger = new Mock<ILogger<CourseRepository>>();

                // Seed initial test data
                SeedTestData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize CourseRepositoryTests: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Seeds the in-memory database with initial test data.
        /// </summary>
        private void SeedTestData()
        {
            try
            {
                var streamId = Guid.NewGuid();
                var subjectId1 = Guid.NewGuid();
                var subjectId2 = Guid.NewGuid();
                var gradeId = Guid.NewGuid();

                var stream = new Streams
                {
                    Id = streamId,
                    Name = "Test Stream",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var subject1 = new Subject
                {
                    Id = subjectId1,
                    Name = "Mathematics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var subject2 = new Subject
                {
                    Id = subjectId2,
                    Name = "Physics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var grade = new Grade
                {
                    Id = gradeId,
                    Name = "Grade 10",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Streams.Add(stream);
                _context.Subjects.Add(subject1);
                _context.Subjects.Add(subject2);
                _context.Grades.Add(grade);
                _context.SaveChanges();

                var courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mathematics 101",
                        StreamId = streamId,
                        SubjectId = subjectId1,
                        GradeId = gradeId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid()
                    },
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Physics 101",
                        StreamId = streamId,
                        SubjectId = subjectId2,
                        GradeId = gradeId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid()
                    },
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Name = "Inactive Course",
                        StreamId = streamId,
                        SubjectId = subjectId1,
                        GradeId = gradeId,
                        IsActive = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid()
                    }
                };

                _context.Courses.AddRange(courses);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to seed test data: {ex.Message}");
                throw;
            }
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Tests that GetAllAsync returns only active courses.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsOnlyActiveCourses()
        {
            try
            {
                // Act
                var result = await _repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.All(result, course => Assert.True(course.IsActive));
                Assert.DoesNotContain(result, c => c.Name == "Inactive Course");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_ReturnsOnlyActiveCourses failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync includes related entities (Subject, Stream).
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_IncludesRelatedEntities()
        {
            try
            {
                // Act
                var result = await _repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.All(result, course =>
                {
                    Assert.NotNull(course.Subject);
                    Assert.NotNull(course.Stream);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_IncludesRelatedEntities failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync uses AsNoTracking for read-only queries.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_UsesNoTracking()
        {
            try
            {
                // Act
                var result = await _repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                foreach (var course in result)
                {
                    var entry = _context.Entry(course);
                    Assert.Equal(EntityState.Detached, entry.State);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_UsesNoTracking failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetAllAsync returns empty list when no active courses exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_WhenNoActiveCourses_ReturnsEmptyList()
        {
            try
            {
                // Arrange - Mark all courses as inactive
                var allCourses = await _context.Courses.ToListAsync();
                foreach (var course in allCourses)
                {
                    course.IsActive = false;
                }
                await _context.SaveChangesAsync();

                // Act
                var result = await _repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync_WhenNoActiveCourses_ReturnsEmptyList failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Tests that GetByIdAsync returns course when it exists and is active.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenCourseExistsAndActive_ReturnsCourse()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive && c.Name == "Mathematics 101");

                // Act
                var result = await _repository.GetByIdAsync(activeCourse.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(activeCourse.Id, result.Id);
                Assert.Equal("Mathematics 101", result.Name);
                Assert.NotNull(result.Subject);
                Assert.NotNull(result.Stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenCourseExistsAndActive_ReturnsCourse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdAsync returns null when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenCourseDoesNotExist_ReturnsNull()
        {
            try
            {
                // Arrange
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await _repository.GetByIdAsync(nonExistentId);

                // Assert
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenCourseDoesNotExist_ReturnsNull failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdAsync returns null when course is inactive.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_WhenCourseIsInactive_ReturnsNull()
        {
            try
            {
                // Arrange
                var inactiveCourse = await _context.Courses
                    .FirstAsync(c => !c.IsActive);

                // Act
                var result = await _repository.GetByIdAsync(inactiveCourse.Id);

                // Assert
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_WhenCourseIsInactive_ReturnsNull failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdAsync includes related entities.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_IncludesRelatedEntities()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                // Act
                var result = await _repository.GetByIdAsync(activeCourse.Id);

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Subject);
                Assert.NotNull(result.Stream);
                Assert.NotEmpty(result.Subject.Name);
                Assert.NotEmpty(result.Stream.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdAsync_IncludesRelatedEntities failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByStreamAsync Tests

        /// <summary>
        /// Tests that GetByStreamAsync returns courses for specified stream.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_ReturnsCoursesByStream()
        {
            try
            {
                // Arrange
                var streamId = await _context.Streams
                    .Where(s => s.IsActive)
                    .Select(s => s.Id)
                    .FirstAsync();

                // Act
                var result = await _repository.GetByStreamAsync(streamId);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.All(result, course =>
                {
                    Assert.Equal(streamId, course.StreamId);
                    Assert.True(course.IsActive);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_ReturnsCoursesByStream failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByStreamAsync returns empty list for non-existent stream.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_WhenNoCoursesForStream_ReturnsEmptyList()
        {
            try
            {
                // Arrange
                var nonExistentStreamId = Guid.NewGuid();

                // Act
                var result = await _repository.GetByStreamAsync(nonExistentStreamId);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_WhenNoCoursesForStream_ReturnsEmptyList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByStreamAsync includes Subject but not Stream.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_IncludesSubjectEntity()
        {
            try
            {
                // Arrange
                var streamId = await _context.Streams
                    .Select(s => s.Id)
                    .FirstAsync();

                // Act
                var result = await _repository.GetByStreamAsync(streamId);

                // Assert
                Assert.NotNull(result);
                Assert.All(result, course =>
                {
                    Assert.NotNull(course.Subject);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_IncludesSubjectEntity failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByStreamAsync uses AsNoTracking.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByStreamAsync_UsesNoTracking()
        {
            try
            {
                // Arrange
                var streamId = await _context.Streams
                    .Select(s => s.Id)
                    .FirstAsync();

                // Act
                var result = await _repository.GetByStreamAsync(streamId);

                // Assert
                Assert.NotNull(result);
                foreach (var course in result)
                {
                    var entry = _context.Entry(course);
                    Assert.Equal(EntityState.Detached, entry.State);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByStreamAsync_UsesNoTracking failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetBySubjectAsync Tests

        /// <summary>
        /// Tests that GetBySubjectAsync returns courses for specified subject.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubjectAsync_ReturnsCoursesBySubject()
        {
            try
            {
                // Arrange
                var subjectId = await _context.Subjects
                    .Where(s => s.Name == "Mathematics")
                    .Select(s => s.Id)
                    .FirstAsync();

                // Act
                var result = await _repository.GetBySubjectAsync(subjectId);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.All(result, course =>
                {
                    Assert.Equal(subjectId, course.SubjectId);
                    Assert.True(course.IsActive);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubjectAsync_ReturnsCoursesBySubject failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetBySubjectAsync returns empty list for non-existent subject.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubjectAsync_WhenNoCoursesForSubject_ReturnsEmptyList()
        {
            try
            {
                // Arrange
                var nonExistentSubjectId = Guid.NewGuid();

                // Act
                var result = await _repository.GetBySubjectAsync(nonExistentSubjectId);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubjectAsync_WhenNoCoursesForSubject_ReturnsEmptyList failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetBySubjectAsync includes Stream entity.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetBySubjectAsync_IncludesStreamEntity()
        {
            try
            {
                // Arrange
                var subjectId = await _context.Subjects
                    .Select(s => s.Id)
                    .FirstAsync();

                // Act
                var result = await _repository.GetBySubjectAsync(subjectId);

                // Assert
                Assert.NotNull(result);
                Assert.All(result, course =>
                {
                    Assert.NotNull(course.Stream);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetBySubjectAsync_IncludesStreamEntity failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region AddAsync Tests

        /// <summary>
        /// Tests that AddAsync successfully adds a new course.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_SuccessfullyAddsNewCourse()
        {
            try
            {
                // Arrange
                var streamId = await _context.Streams.Select(s => s.Id).FirstAsync();
                var subjectId = await _context.Subjects.Select(s => s.Id).FirstAsync();
                var gradeId = await _context.Grades.Select(g => g.Id).FirstAsync();

                var newCourse = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = "Chemistry 101",
                    StreamId = streamId,
                    SubjectId = subjectId,
                    GradeId = gradeId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
                };

                // Act
                var result = await _repository.AddAsync(newCourse);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newCourse.Id, result.Id);
                Assert.Equal("Chemistry 101", result.Name);

                // Verify it's in the database
                var dbCourse = await _context.Courses.FindAsync(newCourse.Id);
                Assert.NotNull(dbCourse);
                Assert.Equal("Chemistry 101", dbCourse.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AddAsync_SuccessfullyAddsNewCourse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that AddAsync generates a new ID if not provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_GeneratesIdIfNotProvided()
        {
            try
            {
                // Arrange
                var streamId = await _context.Streams.Select(s => s.Id).FirstAsync();
                var subjectId = await _context.Subjects.Select(s => s.Id).FirstAsync();
                var gradeId = await _context.Grades.Select(g => g.Id).FirstAsync();

                var newCourse = new Course
                {
                    Name = "Biology 101",
                    StreamId = streamId,
                    SubjectId = subjectId,
                    GradeId = gradeId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid(),
                };

                // Act
                var result = await _repository.AddAsync(newCourse);

                // Assert
                Assert.NotNull(result);
                Assert.NotEqual(Guid.Empty, result.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AddAsync_GeneratesIdIfNotProvided failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that AddAsync saves changes to database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_SavesChangesToDatabase()
        {
            // Arrange
            var streamId = await _context.Streams.Select(s => s.Id).FirstAsync();
            var subjectId = await _context.Subjects.Select(s => s.Id).FirstAsync();
            var gradeId = await _context.Grades.Select(g => g.Id).FirstAsync();

            var courseId = Guid.NewGuid();

            var newCourse = new Course
            {
                Id = courseId,
                Name = "Computer Science 101",
                StreamId = streamId,
                SubjectId = subjectId,
                GradeId = gradeId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(),
            };

            // Act
            await _repository.AddAsync(newCourse);

            // Assert
            var savedCourse = await _context.Courses.FindAsync(courseId);

            Assert.NotNull(savedCourse);
            Assert.Equal("Computer Science 101", savedCourse!.Name);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Tests that UpdateAsync successfully updates an existing course.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateAsync_SuccessfullyUpdatesExistingCourse()
        {
            try
            {
                // Arrange
                var existingCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                existingCourse.Name = "Updated Course Name";
                existingCourse.UpdatedAt = DateTime.UtcNow;
                existingCourse.UpdatedBy = Guid.NewGuid();

                // Act
                var result = await _repository.UpdateAsync(existingCourse);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Updated Course Name", result.Name);

                // Verify in database
                var dbCourse = await _context.Courses.FindAsync(existingCourse.Id);
                Assert.NotNull(dbCourse);
                Assert.Equal("Updated Course Name", dbCourse.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_SuccessfullyUpdatesExistingCourse failed: {ex.Message}");
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
                var existingCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                var beforeUpdate = DateTime.UtcNow;
                existingCourse.Name = "Time Test Course";
                existingCourse.UpdatedAt = DateTime.UtcNow;

                // Act
                await _repository.UpdateAsync(existingCourse);
                var afterUpdate = DateTime.UtcNow;

                // Assert
                var dbCourse = await _context.Courses.FindAsync(existingCourse.Id);
                Assert.NotNull(dbCourse);
                Assert.NotNull(dbCourse.UpdatedAt);
                Assert.True(dbCourse.UpdatedAt >= beforeUpdate);
                Assert.True(dbCourse.UpdatedAt <= afterUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_SetsUpdatedAtTimestamp failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that UpdateAsync saves changes to database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateAsync_SavesChangesToDatabase()
        {
            try
            {
                // Arrange
                var existingCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                var originalName = existingCourse.Name;
                existingCourse.Name = "Persistence Test Course";

                // Act
                await _repository.UpdateAsync(existingCourse);

                // Assert - Reload from database
                _context.Entry(existingCourse).State = EntityState.Detached;
                var reloadedCourse = await _context.Courses.FindAsync(existingCourse.Id);

                Assert.NotNull(reloadedCourse);
                Assert.NotEqual(originalName, reloadedCourse.Name);
                Assert.Equal("Persistence Test Course", reloadedCourse.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] UpdateAsync_SavesChangesToDatabase failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region SoftDeleteAsync Tests

        /// <summary>
        /// Tests that SoftDeleteAsync successfully marks course as inactive.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task SoftDeleteAsync_SuccessfullyMarksAsInactive()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                // Act
                var result = await _repository.SoftDeleteAsync(activeCourse.Id);

                // Assert
                Assert.True(result);

                var dbCourse = await _context.Courses.FindAsync(activeCourse.Id);
                Assert.NotNull(dbCourse);
                Assert.False(dbCourse.IsActive);
                Assert.NotNull(dbCourse.UpdatedAt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SoftDeleteAsync_SuccessfullyMarksAsInactive failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that SoftDeleteAsync returns false when course does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task SoftDeleteAsync_WhenCourseDoesNotExist_ReturnsFalse()
        {
            try
            {
                // Arrange
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await _repository.SoftDeleteAsync(nonExistentId);

                // Assert
                Assert.False(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SoftDeleteAsync_WhenCourseDoesNotExist_ReturnsFalse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that SoftDeleteAsync sets UpdatedAt timestamp.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task SoftDeleteAsync_SetsUpdatedAtTimestamp()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive && c.Name == "Physics 101");

                var beforeDelete = DateTime.UtcNow;

                // Act
                await _repository.SoftDeleteAsync(activeCourse.Id);
                var afterDelete = DateTime.UtcNow;

                // Assert
                var dbCourse = await _context.Courses.FindAsync(activeCourse.Id);
                Assert.NotNull(dbCourse);
                Assert.NotNull(dbCourse.UpdatedAt);
                Assert.True(dbCourse.UpdatedAt >= beforeDelete);
                Assert.True(dbCourse.UpdatedAt <= afterDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SoftDeleteAsync_SetsUpdatedAtTimestamp failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that SoftDeleteAsync does not physically remove course.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task SoftDeleteAsync_DoesNotPhysicallyRemoveCourse()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                var courseId = activeCourse.Id;

                // Act
                await _repository.SoftDeleteAsync(courseId);

                // Assert
                var dbCourse = await _context.Courses
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                Assert.NotNull(dbCourse);
                Assert.False(dbCourse.IsActive);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SoftDeleteAsync_DoesNotPhysicallyRemoveCourse failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that SoftDeleteAsync can be called multiple times (idempotent).
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task SoftDeleteAsync_IsIdempotent()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                // Act
                var result1 = await _repository.SoftDeleteAsync(activeCourse.Id);
                var result2 = await _repository.SoftDeleteAsync(activeCourse.Id);

                // Assert
                Assert.True(result1);
                Assert.False(result2); // Second call returns false as entity already inactive
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SoftDeleteAsync_IsIdempotent failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region GetByIdWithSubjectsAsync Tests

        /// <summary>
        /// Tests that GetByIdWithSubjectsAsync returns course with subjects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdWithSubjectsAsync_ReturnsCourseWithSubjects()
        {
            try
            {
                // Arrange
                var activeCourse = await _context.Courses
                    .FirstAsync(c => c.IsActive);

                // Act
                var result = await _repository.GetByIdWithSubjectsAsync(activeCourse.Id);

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Subject);
                Assert.Equal(activeCourse.Id, result.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdWithSubjectsAsync_ReturnsCourseWithSubjects failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests that GetByIdWithSubjectsAsync returns null for non-existent course.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdWithSubjectsAsync_WhenCourseDoesNotExist_ReturnsNull()
        {
            try
            {
                // Arrange
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await _repository.GetByIdWithSubjectsAsync(nonExistentId);

                // Assert
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetByIdWithSubjectsAsync_WhenCourseDoesNotExist_ReturnsNull failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _context?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to dispose context: {ex.Message}");
                    }
                }

                _disposed = true;
            }
        }
    }
}
