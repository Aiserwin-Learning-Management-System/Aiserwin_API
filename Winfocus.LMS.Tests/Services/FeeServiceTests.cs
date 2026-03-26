namespace Winfocus.LMS.Application.Tests
{
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Services;
    using Winfocus.LMS.Application.Tests.Common;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="FeeService"/>.
    /// All repository calls are mocked via Moq.
    /// Tests verify business logic, fee calculations, exception handling,
    /// and proper delegation to the repository.
    /// </summary>
    public sealed class FeeServiceTests
    {
        private readonly Mock<IFeeRepository> _repoMock;
        private readonly Mock<ILogger<FeeService>> _loggerMock;
        private readonly FeeService _sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeServiceTests"/> class.
        /// Initializes shared mocks and the system under test for each test.
        /// </summary>
        public FeeServiceTests()
        {
            _repoMock = new Mock<IFeeRepository>();
            _loggerMock = new Mock<ILogger<FeeService>>();
            _sut = new FeeService(_repoMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Verifies AppException(404) when student does not exist.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_StudentNotFound_ThrowsAppException404()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((Student?)null);

            // Act
            var act = () => _sut.GetFeePageAsync(FeeTestEntityFactory.NonExistentId);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.StatusCode.Should().Be(404);
            ex.Which.ErrorCode.Should().Be("STUDENT_NOT_FOUND");
        }

        /// <summary>
        /// Verifies a single-row pricing table is returned for one course/plan.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_OneCourse_ReturnsSingleRow()
        {
            // Arrange
            SetupStudentAndCourses();

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            result.Should().NotBeNull();
            result.PricingTable.Should().HaveCount(1);
        }

        /// <summary>
        /// Verifies BaseFee equals TuitionFee only (RegistrationFee excluded).
        /// FeePlan: TuitionFee=60000, RegistrationFee=5000
        /// Expected BaseFee: 60000 (NOT 65000).
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_BaseFee_IsTuitionOnly_NoRegistrationFee()
        {
            // Arrange
            SetupStudentAndCourses();

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            var row = result.PricingTable.Single();
            row.BaseFee.Should().Be(60000m);
            row.BaseFee.Should().NotBe(65000m);
        }

        /// <summary>
        /// Verifies only active discounts are applied to FeeAfterDiscount.
        /// Scholarship=10% active, Seasonal=5% active, Manual=0% inactive.
        /// 60000 → 54000 → 51300.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_ActiveDiscountsOnly_AppliedToFeeAfterDiscount()
        {
            // Arrange
            SetupStudentAndCourses();

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            var row = result.PricingTable.Single();
            row.ScholarshipPercent.Should().Be(10m);
            row.IsScholarshipActive.Should().BeTrue();
            row.SeasonalPercent.Should().Be(5m);
            row.IsSeasonalActive.Should().BeTrue();
            row.ManualDiscountPercent.Should().Be(0m);
            row.IsManualDiscountActive.Should().BeFalse();
            row.FeeAfterDiscount.Should().Be(51300m);
        }

        /// <summary>
        /// Verifies persisted manual discounts from existing selections appear in the row.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_ExistingSelection_ShowsPersistedManualDiscount()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var course = FeeTestEntityFactory.BuildCourse();
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection> { selection });

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            var row = result.PricingTable.Single();
            row.ManualDiscountPercent.Should().Be(3m);
            row.IsManualDiscountActive.Should().BeTrue();
        }

        /// <summary>
        /// Verifies IsSelected is true when student has the course in their selections.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_StudentHasCourse_MarkedAsSelected()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudentWithCourse(
                FeeTestEntityFactory.CourseId);
            var course = FeeTestEntityFactory.BuildCourse();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection>());

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            result.PricingTable.Single().IsSelected.Should().BeTrue();
        }

        /// <summary>
        /// Verifies no discounts scenario returns BaseFee as FeeAfterDiscount.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task GetFeePageAsync_NoDiscounts_FeeAfterDiscountEqualsBaseFee()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var plan = FeeTestEntityFactory.BuildFeePlanNoDiscounts();
            var course = FeeTestEntityFactory.BuildCourse(feePlan: plan);

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection>());

            // Act
            var result = await _sut.GetFeePageAsync(FeeTestEntityFactory.StudentId);

            // Assert
            var row = result.PricingTable.Single();
            row.FeeAfterDiscount.Should().Be(60000m);
        }

        /// <summary>
        /// Verifies AppException(404) when student does not exist.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task SelectFeeAsync_StudentNotFound_ThrowsAppException404()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((Student?)null);

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.NonExistentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
            };

            // Act
            var act = () => _sut.SelectFeeAsync(request);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.ErrorCode.Should().Be("STUDENT_NOT_FOUND");
        }

        /// <summary>
        /// Verifies AppException(404) when fee plan does not exist.
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task SelectFeeAsync_FeePlanNotFound_ThrowsAppException404()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var course = FeeTestEntityFactory.BuildCourse();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.NonExistentId,
            };

            // Act
            var act = () => _sut.SelectFeeAsync(request);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.ErrorCode.Should().Be("FEE_PLAN_NOT_FOUND");
        }

        /// <summary>
        /// Verifies correct sequential calculation: Scholarship(10%) → Seasonal(5%) → Manual(3%).
        /// 60000 → 54000 → 51300 → 49761.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFeeAsync_AllDiscountsActive_CorrectSequentialCalculation()
        {
            // Arrange
            SetupStudentAndCourses();

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                IsScholarshipActive = true,
                IsSeasonalActive = true,
                ManualDiscountPercent = 3m,
                IsManualDiscountActive = true,
            };

            // Act
            var result = await _sut.SelectFeeAsync(request);

            // Assert
            result.BaseFee.Should().Be(60000m);
            result.ScholarshipDiscount.Should().Be(6000m);
            result.SeasonalDiscount.Should().Be(2700m);
            result.ManualDiscount.Should().Be(1539m);
            result.TotalPayable.Should().Be(49761m);
        }

        /// <summary>
        /// Verifies no discounts when all flags are false.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFeeAsync_AllDiscountsInactive_TotalEqualsBaseFee()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var plan = FeeTestEntityFactory.BuildFeePlanNoDiscounts();
            var course = FeeTestEntityFactory.BuildCourse(feePlan: plan);

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                IsScholarshipActive = false,
                IsSeasonalActive = false,
                ManualDiscountPercent = 0m,
                IsManualDiscountActive = false,
            };

            // Act
            var result = await _sut.SelectFeeAsync(request);

            // Assert
            result.TotalPayable.Should().Be(60000m);
            result.ScholarshipDiscount.Should().Be(0m);
            result.SeasonalDiscount.Should().Be(0m);
            result.ManualDiscount.Should().Be(0m);
        }

        /// <summary>
        /// Verifies scholarship override from request takes priority over plan default.
        /// Plan has 10%, request overrides to 25%.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFeeAsync_ScholarshipOverride_UsesRequestValue()
        {
            // Arrange
            SetupStudentAndCourses();

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                ScholarshipPercent = 25m,
                IsScholarshipActive = true,
                IsSeasonalActive = false,
                ManualDiscountPercent = 0m,
                IsManualDiscountActive = false,
            };

            // Act
            var result = await _sut.SelectFeeAsync(request);

            // Assert
            result.ScholarshipDiscount.Should().Be(15000m);
            result.TotalPayable.Should().Be(45000m);
        }

        /// <summary>
        /// Verifies entity is persisted with all discount fields.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFeeAsync_PersistsSelectionWithAllDiscountFields()
        {
            // Arrange
            SetupStudentAndCourses();
            StudentFeeSelection? captured = null;

            _repoMock
                .Setup(r => r.AddStudentFeeSelectionAsync(
                    It.IsAny<StudentFeeSelection>()))
                .Callback<StudentFeeSelection>(s => captured = s);

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                ScholarshipPercent = 10m,
                IsScholarshipActive = true,
                IsSeasonalActive = true,
                ManualDiscountPercent = 5m,
                IsManualDiscountActive = true,
            };

            // Act
            await _sut.SelectFeeAsync(request);

            // Assert
            _repoMock.Verify(
                r => r.AddStudentFeeSelectionAsync(
                    It.IsAny<StudentFeeSelection>()),
                Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            captured.Should().NotBeNull();
            captured!.StudentId.Should().Be(FeeTestEntityFactory.StudentId);
            captured.ScholarshipPercent.Should().Be(10m);
            captured.IsScholarshipActive.Should().BeTrue();
            captured.SeasonalPercent.Should().Be(5m);
            captured.IsSeasonalActive.Should().BeTrue();
            captured.ManualDiscountPercent.Should().Be(5m);
            captured.IsManualDiscountActive.Should().BeTrue();
            captured.BaseFee.Should().Be(60000m);
        }

        /// <summary>
        /// Verifies seasonal is only active when both request AND plan say active.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFeeAsync_SeasonalInactiveOnPlan_NotAppliedEvenIfRequestSaysActive()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var plan = FeeTestEntityFactory.BuildFeePlan();
            plan.Set("IsSeasonalDiscountActive", false);
            var course = FeeTestEntityFactory.BuildCourse(feePlan: plan);

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });

            var request = new SelectFeeRequestDto
            {
                StudentId = FeeTestEntityFactory.StudentId,
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                IsScholarshipActive = false,
                IsSeasonalActive = true,
                ManualDiscountPercent = 0m,
                IsManualDiscountActive = false,
            };

            // Act
            var result = await _sut.SelectFeeAsync(request);

            // Assert
            result.SeasonalDiscount.Should().Be(0m);
            result.TotalPayable.Should().Be(60000m);
        }

        /// <summary>
        /// Verifies AppException when student not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudentAsync_StudentNotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((Student?)null);

            // Act
            var act = () => _sut.GetDiscountsByStudentAsync(
                FeeTestEntityFactory.NonExistentId);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Verifies 3 entries (Scholarship, Seasonal, Manual) per selection.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudentAsync_OneSelection_Returns3Entries()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection> { selection });

            // Act
            var result = await _sut.GetDiscountsByStudentAsync(
                FeeTestEntityFactory.StudentId);

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(d => d.DiscountType == DiscountType.Scholarship);
            result.Should().Contain(d => d.DiscountType == DiscountType.Seasonal);
            result.Should().Contain(d => d.DiscountType == DiscountType.Manual);
        }

        /// <summary>
        /// Verifies 6 entries for 2 selections.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudentAsync_TwoSelections_Returns6Entries()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();
            var sel1 = FeeTestEntityFactory.BuildSelection(Guid.NewGuid());
            var sel2 = FeeTestEntityFactory.BuildSelection(Guid.NewGuid());

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection> { sel1, sel2 });

            // Act
            var result = await _sut.GetDiscountsByStudentAsync(
                FeeTestEntityFactory.StudentId);

            // Assert
            result.Should().HaveCount(6);
        }

        /// <summary>
        /// Verifies empty list when student has no selections.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudentAsync_NoSelections_ReturnsEmpty()
        {
            // Arrange
            var student = FeeTestEntityFactory.BuildStudent();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection>());

            // Act
            var result = await _sut.GetDiscountsByStudentAsync(
                FeeTestEntityFactory.StudentId);

            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Verifies AppException when selection not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsBySelectionAsync_NotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((StudentFeeSelection?)null);

            // Act
            var act = () => _sut.GetDiscountsBySelectionAsync(
                FeeTestEntityFactory.NonExistentId);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.ErrorCode.Should().Be("SELECTION_NOT_FOUND");
        }

        /// <summary>
        /// Verifies correct values in each discount detail entry.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsBySelectionAsync_ValidId_Returns3CorrectEntries()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            // Act
            var result = await _sut.GetDiscountsBySelectionAsync(
                FeeTestEntityFactory.SelectionId);

            // Assert
            result.Should().HaveCount(3);

            var scholarship = result.Single(
                d => d.DiscountType == DiscountType.Scholarship);
            scholarship.Percent.Should().Be(10m);
            scholarship.IsActive.Should().BeTrue();

            var seasonal = result.Single(
                d => d.DiscountType == DiscountType.Seasonal);
            seasonal.Percent.Should().Be(5m);
            seasonal.IsActive.Should().BeTrue();

            var manual = result.Single(
                d => d.DiscountType == DiscountType.Manual);
            manual.Percent.Should().Be(3m);
            manual.IsActive.Should().BeTrue();
        }

        /// <summary>
        /// Verifies AppException for DiscountType.None.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateDiscountAsync_DiscountTypeNone_ThrowsAppException()
        {
            // Arrange
            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = FeeTestEntityFactory.SelectionId,
                DiscountType = DiscountType.None,
                Percent = 10m,
                IsActive = true,
            };

            // Act
            var act = () => _sut.UpdateDiscountAsync(request);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.ErrorCode.Should().Be("INVALID_DISCOUNT_TYPE");
            ex.Which.StatusCode.Should().Be(400);
        }

        /// <summary>
        /// Verifies AppException when selection not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateDiscountAsync_SelectionNotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((StudentFeeSelection?)null);

            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = FeeTestEntityFactory.NonExistentId,
                DiscountType = DiscountType.Scholarship,
                Percent = 10m,
                IsActive = true,
            };

            // Act
            var act = () => _sut.UpdateDiscountAsync(request);

            // Assert
            var ex = await act.Should().ThrowAsync<AppException>();
            ex.Which.ErrorCode.Should().Be("SELECTION_NOT_FOUND");
        }

        /// <summary>
        /// Verifies scholarship update recalculates and saves.
        /// Starting from no discounts, add 20% scholarship.
        /// 60000 → 48000.
        /// </summary>
        [Fact]
        public async Task UpdateDiscountAsync_Scholarship_RecalculatesAndSaves()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelectionNoDiscounts();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = FeeTestEntityFactory.SelectionId,
                DiscountType = DiscountType.Scholarship,
                Percent = 20m,
                IsActive = true,
            };

            // Act
            var result = await _sut.UpdateDiscountAsync(request);

            // Assert
            result.BaseFee.Should().Be(60000m);
            result.ScholarshipDiscount.Should().Be(12000m);
            result.TotalPayable.Should().Be(48000m);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies seasonal update recalculates and saves.
        /// </summary>
        [Fact]
        public async Task UpdateDiscountAsync_Seasonal_RecalculatesAndSaves()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelectionNoDiscounts();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = FeeTestEntityFactory.SelectionId,
                DiscountType = DiscountType.Seasonal,
                Percent = 10m,
                IsActive = true,
            };

            // Act
            var result = await _sut.UpdateDiscountAsync(request);

            // Assert
            result.SeasonalDiscount.Should().Be(6000m);
            result.TotalPayable.Should().Be(54000m);
        }

        /// <summary>
        /// Verifies manual update recalculates and saves.
        /// </summary>
        [Fact]
        public async Task UpdateDiscountAsync_Manual_RecalculatesAndSaves()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelectionNoDiscounts();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = FeeTestEntityFactory.SelectionId,
                DiscountType = DiscountType.Manual,
                Percent = 5m,
                IsActive = true,
            };

            // Act
            var result = await _sut.UpdateDiscountAsync(request);

            // Assert
            result.ManualDiscount.Should().Be(3000m);
            result.TotalPayable.Should().Be(57000m);
        }

        /// <summary>
        /// Verifies AppException for DiscountType.None.
        /// </summary>
        [Fact]
        public async Task RemoveDiscountAsync_DiscountTypeNone_ThrowsAppException()
        {
            // Act
            var act = () => _sut.RemoveDiscountAsync(
                FeeTestEntityFactory.SelectionId, DiscountType.None);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Verifies AppException when selection not found.
        /// </summary>
        [Fact]
        public async Task RemoveDiscountAsync_SelectionNotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((StudentFeeSelection?)null);

            // Act
            var act = () => _sut.RemoveDiscountAsync(
                FeeTestEntityFactory.NonExistentId, DiscountType.Scholarship);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Verifies removing scholarship sets to 0% inactive and recalculates.
        /// Selection starts with Scholarship=10%, Seasonal=5%, Manual=3%.
        /// After removing scholarship: Seasonal(5%) and Manual(3%) remain.
        /// 60000 → 57000 → 55290.
        /// </summary>
        [Fact]
        public async Task RemoveDiscountAsync_Scholarship_SetsZeroAndRecalculates()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            // Act
            var result = await _sut.RemoveDiscountAsync(
                FeeTestEntityFactory.SelectionId, DiscountType.Scholarship);

            // Assert
            selection.ScholarshipPercent.Should().Be(0m);
            selection.IsScholarshipActive.Should().BeFalse();
            result.ScholarshipDiscount.Should().Be(0m);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies removing manual sets to 0% inactive.
        /// </summary>
        [Fact]
        public async Task RemoveDiscountAsync_Manual_SetsZeroAndRecalculates()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            // Act
            var result = await _sut.RemoveDiscountAsync(
                FeeTestEntityFactory.SelectionId, DiscountType.Manual);

            // Assert
            selection.ManualDiscountPercent.Should().Be(0m);
            selection.IsManualDiscountActive.Should().BeFalse();
            result.ManualDiscount.Should().Be(0m);
        }

        /// <summary>
        /// Verifies removing seasonal sets to 0% inactive.
        /// </summary>
        [Fact]
        public async Task RemoveDiscountAsync_Seasonal_SetsZeroAndRecalculates()
        {
            // Arrange
            var selection = FeeTestEntityFactory.BuildSelection();

            _repoMock
                .Setup(r => r.GetStudentFeeSelectionByIdAsync(
                    FeeTestEntityFactory.SelectionId))
                .ReturnsAsync(selection);

            // Act
            var result = await _sut.RemoveDiscountAsync(
                FeeTestEntityFactory.SelectionId, DiscountType.Seasonal);

            // Assert
            selection.SeasonalPercent.Should().Be(0m);
            selection.IsSeasonalActive.Should().BeFalse();
            result.SeasonalDiscount.Should().Be(0m);
        }

        /// <summary>
        /// Verifies AppException when plan not found for seasonal update.
        /// </summary>
        [Fact]
        public async Task UpdateSeasonalOnPlan_PlanNotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((FeePlan?)null);

            var request = new UpdateSeasonalDiscountRequestDto
            {
                FeePlanId = FeeTestEntityFactory.NonExistentId,
                Percent = 5m,
                IsActive = true,
            };

            // Act
            var act = () => _sut.UpdateSeasonalDiscountOnPlanAsync(request);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Verifies seasonal is updated and saved on the plan.
        /// </summary>
        [Fact]
        public async Task UpdateSeasonalOnPlan_ValidPlan_UpdatesAndSaves()
        {
            // Arrange
            var plan = FeeTestEntityFactory.BuildFeePlan();

            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.FeePlanId))
                .ReturnsAsync(plan);

            var request = new UpdateSeasonalDiscountRequestDto
            {
                FeePlanId = FeeTestEntityFactory.FeePlanId,
                Percent = 12m,
                IsActive = true,
            };

            // Act
            await _sut.UpdateSeasonalDiscountOnPlanAsync(request);

            // Assert
            plan.SeasonalPercent.Should().Be(12m);
            plan.IsSeasonalDiscountActive.Should().BeTrue();
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies get seasonal returns correct detail.
        /// </summary>
        [Fact]
        public async Task GetSeasonalOnPlan_ValidPlan_ReturnsDetail()
        {
            // Arrange
            var plan = FeeTestEntityFactory.BuildFeePlan();

            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.FeePlanId))
                .ReturnsAsync(plan);

            // Act
            var result = await _sut.GetSeasonalDiscountOnPlanAsync(
                FeeTestEntityFactory.FeePlanId);

            // Assert
            result.DiscountType.Should().Be(DiscountType.Seasonal);
            result.Percent.Should().Be(5m);
            result.IsActive.Should().BeTrue();
        }

        /// <summary>
        /// Verifies get seasonal throws when plan not found.
        /// </summary>
        [Fact]
        public async Task GetSeasonalOnPlan_NotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((FeePlan?)null);

            // Act
            var act = () => _sut.GetSeasonalDiscountOnPlanAsync(
                FeeTestEntityFactory.NonExistentId);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Verifies remove seasonal zeroes and deactivates on the plan.
        /// </summary>
        [Fact]
        public async Task RemoveSeasonalOnPlan_ValidPlan_ZeroesAndDeactivates()
        {
            // Arrange
            var plan = FeeTestEntityFactory.BuildFeePlan();

            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.FeePlanId))
                .ReturnsAsync(plan);

            // Act
            await _sut.RemoveSeasonalDiscountOnPlanAsync(FeeTestEntityFactory.FeePlanId);

            // Assert
            plan.SeasonalPercent.Should().Be(0m);
            plan.IsSeasonalDiscountActive.Should().BeFalse();
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies remove seasonal throws when plan not found.
        /// </summary>
        [Fact]
        public async Task RemoveSeasonalOnPlan_NotFound_ThrowsAppException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetFeePlanByIdAsync(FeeTestEntityFactory.NonExistentId))
                .ReturnsAsync((FeePlan?)null);

            // Act
            var act = () => _sut.RemoveSeasonalDiscountOnPlanAsync(
                FeeTestEntityFactory.NonExistentId);

            // Assert
            await act.Should().ThrowAsync<AppException>();
        }

        /// <summary>
        /// Sets up default student, course, and empty selections mocks.
        /// </summary>
        private void SetupStudentAndCourses()
        {
            var student = FeeTestEntityFactory.BuildStudent();
            var course = FeeTestEntityFactory.BuildCourse();

            _repoMock
                .Setup(r => r.GetStudentWithCoursesAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(student);
            _repoMock
                .Setup(r => r.GetCoursesByGradeAsync(FeeTestEntityFactory.GradeId))
                .ReturnsAsync(new List<Course> { course });
            _repoMock
                .Setup(r => r.GetStudentFeeSelectionsByStudentAsync(
                    FeeTestEntityFactory.StudentId))
                .ReturnsAsync(new List<StudentFeeSelection>());
        }
    }
}
