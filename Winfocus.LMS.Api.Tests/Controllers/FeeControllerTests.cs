namespace Winfocus.LMS.Api.Tests.Controllers
{
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;
    using Moq;
    using Winfocus.LMS.Api.Tests.Common;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;
    using Xunit;

    /// <summary>
    /// Integration tests for <see cref="API.Controllers.FeeController"/>.
    /// Uses <see cref="FeeTestWebApplicationFactory"/> with a mocked IFeeService
    /// to verify HTTP status codes, routing, serialization, and delegation.
    /// </summary>
    public sealed class FeeControllerTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<IFeeService> _serviceMock;

        private static readonly Guid TestStudentId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private static readonly Guid TestSelectionId =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        private static readonly Guid TestFeePlanId =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeControllerTests"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public FeeControllerTests(TestWebApplicationFactory factory)
        {
            _serviceMock = factory.FeeServiceMock;
            _serviceMock.Reset();
            _client = factory.CreateClient();
        }

        /// <summary>
        /// Gets the fee page valid student returns200 with pricing table.
        /// </summary>
        /// Verifies GET fee page returns 200 OK with correct pricing table data.
        /// The service mock is set up to return a predefined FeePageResponseDto,
        /// and the test asserts that the response contains the expected fee amounts
        /// Registration fee is excluded from calculations, so only course fees are tested here.
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns></summary>
        [Fact]
        public async Task GetFeePage_ValidStudent_Returns200WithPricingTable()
        {
            // Arrange
            var expected = new FeePageResponseDto
            {
                PricingTable = new List<FeeRowDto>
                {
                    new ()
                    {
                        FeePlanId = TestFeePlanId,
                        CourseId = Guid.NewGuid(),
                        CourseName = "Physics",
                        BaseFee = 60000m,
                        PaymentType = "Yearly",
                        ScholarshipPercent = 10m,
                        IsScholarshipActive = true,
                        SeasonalPercent = 5m,
                        IsSeasonalActive = true,
                        ManualDiscountPercent = 0m,
                        IsManualDiscountActive = false,
                        FeeAfterDiscount = 51300m,
                        IsSelected = false,
                    },
                },
            };

            _serviceMock
                .Setup(s => s.GetFeePageAsync(TestStudentId))
                .ReturnsAsync(expected);

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/{TestStudentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<FeePageResponseDto>();

            body.Should().NotBeNull();
            body!.PricingTable.Should().HaveCount(1);
            body.PricingTable[0].BaseFee.Should().Be(60000m);
            body.PricingTable[0].FeeAfterDiscount.Should().Be(51300m);
        }

        /// <summary>
        /// Verifies GET fee page delegates to the service with correct studentId.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetFeePage_DelegatesToService_WithCorrectStudentId()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetFeePageAsync(TestStudentId))
                .ReturnsAsync(new FeePageResponseDto { PricingTable = new () });

            // Act
            await _client.GetAsync($"/api/v1/Fee/{TestStudentId}");

            // Assert
            _serviceMock.Verify(
                s => s.GetFeePageAsync(TestStudentId), Times.Once);
        }

        /// <summary>
        /// Verifies GET fee page returns error when student not found.
        /// The GlobalExceptionMiddleware catches AppException and returns the
        /// appropriate status code.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetFeePage_StudentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetFeePageAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Student not found.", 404, "STUDENT_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync($"/api/v1/Fee/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies POST select fee returns 200 OK with fee summary.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFee_ValidRequest_Returns200WithSummary()
        {
            // Arrange
            var request = new SelectFeeRequestDto
            {
                StudentId = TestStudentId,
                FeePlanId = TestFeePlanId,
                ScholarshipPercent = 10m,
                IsScholarshipActive = true,
                IsSeasonalActive = true,
                ManualDiscountPercent = 3m,
                IsManualDiscountActive = true,
            };

            var expected = new FeeSummaryDto
            {
                BaseFee = 60000m,
                ScholarshipDiscount = 6000m,
                SeasonalDiscount = 2700m,
                ManualDiscount = 1539m,
                TotalPayable = 49761m,
            };

            _serviceMock
                .Setup(s => s.SelectFeeAsync(It.Is<SelectFeeRequestDto>(r =>
                    r.StudentId == TestStudentId &&
                    r.FeePlanId == TestFeePlanId)))
                .ReturnsAsync(expected);

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/select", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<FeeSummaryDto>();

            body.Should().NotBeNull();
            body!.BaseFee.Should().Be(60000m);
            body.TotalPayable.Should().Be(49761m);
            body.ScholarshipDiscount.Should().Be(6000m);
            body.SeasonalDiscount.Should().Be(2700m);
            body.ManualDiscount.Should().Be(1539m);
        }

        /// <summary>
        /// Verifies POST select fee returns error when student not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFee_StudentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new SelectFeeRequestDto
            {
                StudentId = Guid.NewGuid(),
                FeePlanId = TestFeePlanId,
            };

            _serviceMock
                .Setup(s => s.SelectFeeAsync(It.IsAny<SelectFeeRequestDto>()))
                .ThrowsAsync(new AppException(
                    "Student not found.", 404,  "STUDENT_NOT_FOUND"));

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/select", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies POST select fee returns error when plan not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SelectFee_FeePlanNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new SelectFeeRequestDto
            {
                StudentId = TestStudentId,
                FeePlanId = Guid.NewGuid(),
            };

            _serviceMock
                .Setup(s => s.SelectFeeAsync(It.IsAny<SelectFeeRequestDto>()))
                .ThrowsAsync(new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND"));

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/select", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies GET discounts by student returns 200 with list.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudent_ValidId_Returns200WithList()
        {
            // Arrange
            var discounts = new List<DiscountDetailDto>
            {
                new ()
                {
                    StudentFeeSelectionId = TestSelectionId,
                    StudentId = TestStudentId,
                    DiscountType = DiscountType.Scholarship,
                    Percent = 10m,
                    IsActive = true,
                    BaseFee = 60000m,
                    FinalAmount = 51300m,
                },
                new ()
                {
                    StudentFeeSelectionId = TestSelectionId,
                    StudentId = TestStudentId,
                    DiscountType = DiscountType.Seasonal,
                    Percent = 5m,
                    IsActive = true,
                    BaseFee = 60000m,
                    FinalAmount = 51300m,
                },
                new ()
                {
                    StudentFeeSelectionId = TestSelectionId,
                    StudentId = TestStudentId,
                    DiscountType = DiscountType.Manual,
                    Percent = 0m,
                    IsActive = false,
                    BaseFee = 60000m,
                    FinalAmount = 51300m,
                },
            };

            _serviceMock
                .Setup(s => s.GetDiscountsByStudentAsync(TestStudentId))
                .ReturnsAsync(discounts.AsReadOnly());

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/student/{TestStudentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<List<DiscountDetailDto>>();

            body.Should().HaveCount(3);
        }

        /// <summary>
        /// Verifies GET discounts by student returns error when not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsByStudent_NotFound_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetDiscountsByStudentAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Student not found.", 404, "STUDENT_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/student/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies GET discounts by selection returns 200 with 3 entries.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsBySelection_ValidId_Returns200With3Entries()
        {
            // Arrange
            var discounts = new List<DiscountDetailDto>
            {
                new () { DiscountType = DiscountType.Scholarship },
                new () { DiscountType = DiscountType.Seasonal },
                new () { DiscountType = DiscountType.Manual },
            };

            _serviceMock
                .Setup(s => s.GetDiscountsBySelectionAsync(TestSelectionId))
                .ReturnsAsync(discounts.AsReadOnly());

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/selection/{TestSelectionId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<List<DiscountDetailDto>>();

            body.Should().HaveCount(3);
        }

        /// <summary>
        /// Verifies GET discounts by selection returns error when not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetDiscountsBySelection_NotFound_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetDiscountsBySelectionAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Selection not found.", 404, "SELECTION_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/selection/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies PUT update discount returns 200 with recalculated summary.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateDiscount_ValidRequest_Returns200WithSummary()
        {
            // Arrange
            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = TestSelectionId,
                DiscountType = DiscountType.Manual,
                Percent = 5m,
                IsActive = true,
            };

            var expected = new FeeSummaryDto
            {
                BaseFee = 60000m,
                ManualDiscount = 3000m,
                TotalPayable = 57000m,
            };

            _serviceMock
                .Setup(s => s.UpdateDiscountAsync(It.Is<UpdateDiscountRequestDto>(r =>
                    r.StudentFeeSelectionId == TestSelectionId &&
                    r.DiscountType == DiscountType.Manual)))
                .ReturnsAsync(expected);

            // Act
            var response = await _client.PutAsJsonAsync(
                "/api/v1/Fee/discounts", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<FeeSummaryDto>();

            body!.ManualDiscount.Should().Be(3000m);
            body.TotalPayable.Should().Be(57000m);
        }

        /// <summary>
        /// Verifies PUT update discount returns error for invalid discount type.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateDiscount_InvalidDiscountType_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = TestSelectionId,
                DiscountType = DiscountType.None,
                Percent = 10m,
                IsActive = true,
            };

            _serviceMock
                .Setup(s => s.UpdateDiscountAsync(It.IsAny<UpdateDiscountRequestDto>()))
                .ThrowsAsync(new AppException(
                    "Invalid discount type.", 400, "INVALID_DISCOUNT_TYPE"));

            // Act
            var response = await _client.PutAsJsonAsync(
                "/api/v1/Fee/discounts", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Verifies PUT update scholarship delegates correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateDiscount_Scholarship_DelegatesToService()
        {
            // Arrange
            var request = new UpdateDiscountRequestDto
            {
                StudentFeeSelectionId = TestSelectionId,
                DiscountType = DiscountType.Scholarship,
                Percent = 15m,
                IsActive = true,
            };

            _serviceMock
                .Setup(s => s.UpdateDiscountAsync(It.IsAny<UpdateDiscountRequestDto>()))
                .ReturnsAsync(new FeeSummaryDto());

            // Act
            await _client.PutAsJsonAsync("/api/v1/Fee/discounts", request);

            // Assert
            _serviceMock.Verify(
                s => s.UpdateDiscountAsync(It.Is<UpdateDiscountRequestDto>(r =>
                    r.DiscountType == DiscountType.Scholarship &&
                    r.Percent == 15m)),
                Times.Once);
        }

        /// <summary>
        /// Verifies DELETE scholarship discount returns 200 with summary.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RemoveDiscount_Scholarship_Returns200WithSummary()
        {
            // Arrange
            var expected = new FeeSummaryDto
            {
                BaseFee = 60000m,
                ScholarshipDiscount = 0m,
                SeasonalDiscount = 3000m,
                TotalPayable = 57000m,
            };

            _serviceMock
                .Setup(s => s.RemoveDiscountAsync(
                    TestSelectionId, DiscountType.Scholarship))
                .ReturnsAsync(expected);

            // Act
            var response = await _client.DeleteAsync(
                $"/api/v1/Fee/discounts/{TestSelectionId}/{DiscountType.Scholarship}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<FeeSummaryDto>();

            body!.ScholarshipDiscount.Should().Be(0m);
        }

        /// <summary>
        /// Verifies DELETE manual discount delegates correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RemoveDiscount_Manual_DelegatesToService()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.RemoveDiscountAsync(
                    TestSelectionId, DiscountType.Manual))
                .ReturnsAsync(new FeeSummaryDto());

            // Act
            await _client.DeleteAsync(
                $"/api/v1/Fee/discounts/{TestSelectionId}/{DiscountType.Manual}");

            // Assert
            _serviceMock.Verify(
                s => s.RemoveDiscountAsync(
                    TestSelectionId, DiscountType.Manual),
                Times.Once);
        }

        /// <summary>
        /// Verifies DELETE seasonal discount delegates correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RemoveDiscount_Seasonal_DelegatesToService()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.RemoveDiscountAsync(
                    TestSelectionId, DiscountType.Seasonal))
                .ReturnsAsync(new FeeSummaryDto());

            // Act
            await _client.DeleteAsync(
                $"/api/v1/Fee/discounts/{TestSelectionId}/{DiscountType.Seasonal}");

            // Assert
            _serviceMock.Verify(
                s => s.RemoveDiscountAsync(
                    TestSelectionId, DiscountType.Seasonal),
                Times.Once);
        }

        /// <summary>
        /// Verifies GET seasonal on plan returns 200 with detail.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetSeasonalDiscountOnPlan_ValidId_Returns200()
        {
            // Arrange
            var expected = new DiscountDetailDto
            {
                FeePlanId = TestFeePlanId,
                DiscountType = DiscountType.Seasonal,
                Percent = 5m,
                IsActive = true,
                BaseFee = 60000m,
            };

            _serviceMock
                .Setup(s => s.GetSeasonalDiscountOnPlanAsync(TestFeePlanId))
                .ReturnsAsync(expected);

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/plan/seasonal/{TestFeePlanId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content
                .ReadFromJsonAsync<DiscountDetailDto>();

            body!.Percent.Should().Be(5m);
            body.IsActive.Should().BeTrue();
        }

        /// <summary>
        /// Verifies GET seasonal on plan returns error when not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetSeasonalDiscountOnPlan_NotFound_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetSeasonalDiscountOnPlanAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/discounts/plan/seasonal/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies PUT seasonal on plan returns 204 No Content.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateSeasonalDiscountOnPlan_ValidRequest_Returns204()
        {
            // Arrange
            var request = new UpdateSeasonalDiscountRequestDto
            {
                FeePlanId = TestFeePlanId,
                Percent = 8m,
                IsActive = true,
            };

            _serviceMock
                .Setup(s => s.UpdateSeasonalDiscountOnPlanAsync(
                    It.IsAny<UpdateSeasonalDiscountRequestDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _client.PutAsJsonAsync(
                "/api/v1/Fee/discounts/plan/seasonal", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Verifies PUT seasonal on plan delegates to service.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateSeasonalDiscountOnPlan_DelegatesToService()
        {
            // Arrange
            var request = new UpdateSeasonalDiscountRequestDto
            {
                FeePlanId = TestFeePlanId,
                Percent = 12m,
                IsActive = true,
            };

            _serviceMock
                .Setup(s => s.UpdateSeasonalDiscountOnPlanAsync(
                    It.IsAny<UpdateSeasonalDiscountRequestDto>()))
                .Returns(Task.CompletedTask);

            // Act
            await _client.PutAsJsonAsync(
                "/api/v1/Fee/discounts/plan/seasonal", request);

            // Assert
            _serviceMock.Verify(
                s => s.UpdateSeasonalDiscountOnPlanAsync(
                    It.Is<UpdateSeasonalDiscountRequestDto>(r =>
                        r.FeePlanId == TestFeePlanId &&
                        r.Percent == 12m)),
                Times.Once);
        }

        /// <summary>
        /// Verifies DELETE seasonal on plan returns 204 No Content.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RemoveSeasonalDiscountOnPlan_ValidId_Returns204()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.RemoveSeasonalDiscountOnPlanAsync(TestFeePlanId))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync(
                $"/api/v1/Fee/discounts/plan/seasonal/{TestFeePlanId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Verifies DELETE seasonal on plan delegates to service.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RemoveSeasonalDiscountOnPlan_DelegatesToService()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.RemoveSeasonalDiscountOnPlanAsync(TestFeePlanId))
                .Returns(Task.CompletedTask);

            // Act
            await _client.DeleteAsync(
                $"/api/v1/Fee/discounts/plan/seasonal/{TestFeePlanId}");

            // Assert
            _serviceMock.Verify(
                s => s.RemoveSeasonalDiscountOnPlanAsync(TestFeePlanId),
                Times.Once);
        }
    }
}
