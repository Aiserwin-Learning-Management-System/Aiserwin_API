namespace Winfocus.LMS.Api.Tests.Controllers
{
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;
    using Moq;
    using Winfocus.LMS.Api.Tests.Common;
    using Winfocus.LMS.Application.Common.Exceptions;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Enums;
    using Xunit;

    /// <summary>
    /// Integration tests for <see cref="API.Controllers.FeeController"/>.
    /// Uses <see cref="TestWebApplicationFactory"/> with a mocked IFeeService
    /// to verify HTTP status codes, routing, serialization, and delegation.
    /// </summary>
    public sealed class FeeControllerTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<IFeeService> _serviceMock;

        private static readonly Guid TestStudentId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private static readonly Guid TestCourseId =
            Guid.Parse("33333333-3333-3333-3333-333333333333");

        private static readonly Guid TestFeePlanId =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        private static readonly Guid TestSelectionId =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        private static readonly Guid TestInstallmentId =
            Guid.Parse("77777777-7777-7777-7777-777777777777");

        private static readonly Guid TestDiscountId =
            Guid.Parse("88888888-8888-8888-8888-888888888888");

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
        /// Verifies GET student fee page returns 200 OK with fee listings.
        /// </summary>
        [Fact]
        public async Task GetStudentFeePage_ValidStudent_Returns200WithFeeListings()
        {
            // Arrange
            var expected = new StudentFeePageDto
            {
                StudentId = TestStudentId,
                StudentName = "John Doe",
                RegistrationNumber = "REG-001",
                GradeId = Guid.NewGuid(),
                GradeName = "Grade 10",
                SyllabusId = Guid.NewGuid(),
                SyllabusName = "CBSE",
                ScholarshipPercent = 10m,
                SelectedFeePlanId = TestFeePlanId,
                FeeListings = new List<FeeListingRowDto>
                {
                    new ()
                    {
                        FeePlanId = TestFeePlanId,
                        CourseId = TestCourseId,
                        CourseName = "Physics",
                        YearlyFee = 60000m,
                        PaymentType = PaymentType.Yearly,
                        DurationInYears = 1,
                        TotalDiscountPercent = 10m,
                        TotalBeforeDiscount = 60000m,
                        FeeAfterDiscount = 54000m,
                        InstallmentCount = 1,
                        PerInstallment = 54000m,
                        IsSelected = true,
                    },
                },
            };

            _serviceMock
                .Setup(s => s.GetStudentFeePageAsync(TestStudentId))
                .ReturnsAsync(CommonResponse<StudentFeePageDto>.SuccessResponse("Success", expected));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/student-page/{TestStudentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<StudentFeePageDto>>();

            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.FeeListings.Should().HaveCount(1);
            result.Data.FeeListings[0].YearlyFee.Should().Be(60000m);
            result.Data.FeeListings[0].FeeAfterDiscount.Should().Be(54000m);
        }

        /// <summary>
        /// Verifies GET student fee page delegates to service with correct studentId.
        /// </summary>
        [Fact]
        public async Task GetStudentFeePage_DelegatesToService_WithCorrectStudentId()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetStudentFeePageAsync(TestStudentId))
                .ReturnsAsync(CommonResponse<StudentFeePageDto>.SuccessResponse(
                    "Success", new StudentFeePageDto()));

            // Act
            await _client.GetAsync($"/api/v1/Fee/student-page/{TestStudentId}");

            // Assert
            _serviceMock.Verify(
                s => s.GetStudentFeePageAsync(TestStudentId), Times.Once);
        }

        /// <summary>
        /// Verifies GET student fee page returns error when student not found.
        /// </summary>
        [Fact]
        public async Task GetStudentFeePage_StudentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetStudentFeePageAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Student not found.", 404, "STUDENT_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync($"/api/v1/Fee/student-page/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies POST confirm fee selection returns 200 OK with response.
        /// </summary>
        [Fact]
        public async Task ConfirmFeeSelection_ValidRequest_Returns200WithResponse()
        {
            // Arrange
            var request = new ConfirmFeeRequestDto
            {
                StudentId = TestStudentId,
                FeePlanId = TestFeePlanId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                DeclarationAccepted = true,
            };

            var expected = new ConfirmFeeResponseDto
            {
                SelectionId = TestSelectionId,
                CourseName = "Physics",
                PlanName = "Yearly",
                YearlyFee = 60000m,
                DurationYears = 1,
                TotalBeforeDiscount = 60000m,
                TotalDiscountPercent = 10m,
                TotalDiscountAmount = 6000m,
                FinalAmount = 54000m,
                PaymentType = PaymentType.Yearly,
                TotalInstallments = 1,
                Status = FeeSelectionStatus.Confirmed,
            };

            _serviceMock
                .Setup(s => s.ConfirmFeeSelectionAsync(It.Is<ConfirmFeeRequestDto>(r =>
                    r.StudentId == TestStudentId &&
                    r.FeePlanId == TestFeePlanId)))
                .ReturnsAsync(CommonResponse<ConfirmFeeResponseDto>.SuccessResponse("Success", expected));

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/confirm", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<ConfirmFeeResponseDto>>();

            result.Should().NotBeNull();
            result!.Data.FinalAmount.Should().Be(54000m);
            result.Data.TotalDiscountAmount.Should().Be(6000m);
        }

        /// <summary>
        /// Verifies POST confirm fee returns error when plan not found.
        /// </summary>
        [Fact]
        public async Task ConfirmFeeSelection_FeePlanNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new ConfirmFeeRequestDto
            {
                StudentId = TestStudentId,
                FeePlanId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                DeclarationAccepted = true,
            };

            _serviceMock
                .Setup(s => s.ConfirmFeeSelectionAsync(It.IsAny<ConfirmFeeRequestDto>()))
                .ThrowsAsync(new AppException(
                    "Fee plan not found.", 404, "FEE_PLAN_NOT_FOUND"));

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/confirm", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies GET admin student fee page returns 200 OK.
        /// </summary>
        [Fact]
        public async Task GetAdminStudentFeePage_ValidStudent_Returns200()
        {
            // Arrange
            var expected = new AdminStudentFeePageDto
            {
                StudentId = TestStudentId,
                StudentName = "John Doe",
                RegistrationNumber = "REG-001",
                GradeName = "Grade 10",
                SyllabusName = "CBSE",
                TotalPayable = 54000m,
                CourseDiscounts = new List<CourseDiscountBlockDto>
                {
                    new ()
                    {
                        CourseId = TestCourseId,
                        CourseName = "Physics",
                        BaseYearlyFee = 60000m,
                        CalculatedFeeAfterDiscount = 54000m,
                        AvailableDiscounts = new List<AvailableDiscountDto>(),
                    },
                },
            };

            _serviceMock
                .Setup(s => s.GetAdminStudentFeePageAsync(TestStudentId))
                .ReturnsAsync(CommonResponse<AdminStudentFeePageDto>.SuccessResponse("Success", expected));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/student-admin/{TestStudentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<AdminStudentFeePageDto>>();

            result.Should().NotBeNull();
            result!.Data.StudentName.Should().Be("John Doe");
            result.Data.CourseDiscounts.Should().HaveCount(1);
        }

        /// <summary>
        /// Verifies POST assign discounts returns 200 OK.
        /// </summary>
        [Fact]
        public async Task AssignDiscounts_ValidRequest_Returns200()
        {
            // Arrange
            var request = new AssignDiscountsRequestDto
            {
                StudentId = TestStudentId,
                CourseId = TestCourseId,
                SelectedDiscountId = TestDiscountId,
                UserId = Guid.NewGuid(),
            };

            _serviceMock
                .Setup(s => s.AssignDiscountsAsync(It.IsAny<AssignDiscountsRequestDto>()))
                .ReturnsAsync(CommonResponse<bool>.SuccessResponse("Discounts assigned.", true));

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/v1/Fee/assign-discounts", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Verifies PUT assign discounts (update) returns 200 OK.
        /// </summary>
        [Fact]
        public async Task UpdateDiscountAssignments_ValidRequest_Returns200()
        {
            // Arrange
            var request = new AssignDiscountsRequestDto
            {
                StudentId = TestStudentId,
                CourseId = TestCourseId,
                ManualDiscount = new ManualDiscountRequestDto
                {
                    DiscountName = "Merit",
                    DiscountPercent = 5m,
                },
                UserId = Guid.NewGuid(),
            };

            _serviceMock
                .Setup(s => s.AssignDiscountsAsync(It.IsAny<AssignDiscountsRequestDto>()))
                .ReturnsAsync(CommonResponse<bool>.SuccessResponse("Discounts updated.", true));

            // Act
            var response = await _client.PutAsJsonAsync(
                "/api/v1/Fee/assign-discounts", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Verifies DELETE assign discounts returns 200 OK.
        /// </summary>
        [Fact]
        public async Task RemoveDiscounts_ValidIds_Returns200()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.RemoveDiscountsAsync(TestStudentId, TestCourseId))
                .ReturnsAsync(CommonResponse<bool>.SuccessResponse("Discounts removed.", true));

            // Act
            var response = await _client.DeleteAsync(
                $"/api/v1/Fee/assign-discounts/{TestStudentId}/{TestCourseId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Verifies GET installments returns 200 with list.
        /// </summary>
        [Fact]
        public async Task GetInstallments_ValidSelection_Returns200WithList()
        {
            // Arrange
            var installments = new List<InstallmentScheduleDto>
            {
                new ()
                {
                    InstallmentId = TestInstallmentId,
                    No = 1,
                    DueDate = DateTime.UtcNow.AddMonths(1),
                    DueAmount = 27000m,
                    PaidAmount = 0m,
                    BalanceAmount = 27000m,
                    Status = InstallmentStatus.Pending,
                },
                new ()
                {
                    InstallmentId = Guid.NewGuid(),
                    No = 2,
                    DueDate = DateTime.UtcNow.AddMonths(6),
                    DueAmount = 27000m,
                    PaidAmount = 0m,
                    BalanceAmount = 27000m,
                    Status = InstallmentStatus.Pending,
                },
            };

            _serviceMock
                .Setup(s => s.GetInstallmentsAsync(TestSelectionId))
                .ReturnsAsync(CommonResponse<List<InstallmentScheduleDto>>.SuccessResponse("Success", installments));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/installments/{TestSelectionId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<List<InstallmentScheduleDto>>>();

            result.Should().NotBeNull();
            result!.Data.Should().HaveCount(2);
            result.Data[0].DueAmount.Should().Be(27000m);
        }

        /// <summary>
        /// Verifies PUT record payment returns 200 OK with updated installment.
        /// </summary>
        [Fact]
        public async Task RecordPayment_ValidRequest_Returns200WithUpdatedInstallment()
        {
            // Arrange
            var request = new RecordPaymentRequestDto
            {
                PaidAmount = 27000m,
                PaidDate = DateTime.UtcNow,
                Remarks = "First installment",
            };

            var expected = new InstallmentScheduleDto
            {
                InstallmentId = TestInstallmentId,
                No = 1,
                DueDate = DateTime.UtcNow.AddMonths(1),
                DueAmount = 27000m,
                PaidAmount = 27000m,
                BalanceAmount = 0m,
                Status = InstallmentStatus.Paid,
                PaidDate = DateTime.UtcNow,
            };

            _serviceMock
                .Setup(s => s.RecordPaymentAsync(TestInstallmentId, It.IsAny<RecordPaymentRequestDto>()))
                .ReturnsAsync(CommonResponse<InstallmentScheduleDto>.SuccessResponse("Payment recorded.", expected));

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/v1/Fee/installments/{TestInstallmentId}/pay", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<InstallmentScheduleDto>>();

            result.Should().NotBeNull();
            result!.Data.Status.Should().Be(InstallmentStatus.Paid);
            result.Data.PaidAmount.Should().Be(27000m);
        }

        /// <summary>
        /// Verifies GET payment summary returns 200 with summary.
        /// </summary>
        [Fact]
        public async Task GetPaymentSummary_ValidStudent_Returns200WithSummary()
        {
            // Arrange
            var expected = new PaymentSummaryDto
            {
                StudentId = TestStudentId,
                StudentName = "John Doe",
                GrandTotal = 54000m,
                GrandPaid = 27000m,
                GrandRemaining = 27000m,
                Selections = new List<SelectionPaymentDto>
                {
                    new ()
                    {
                        SelectionId = TestSelectionId,
                        CourseName = "Physics",
                        PlanName = "Yearly",
                        PaymentType = PaymentType.Yearly,
                        TotalFee = 54000m,
                        TotalPaid = 27000m,
                        TotalRemaining = 27000m,
                        Status = FeeSelectionStatus.PartiallyPaid,
                    },
                },
            };

            _serviceMock
                .Setup(s => s.GetPaymentSummaryAsync(TestStudentId))
                .ReturnsAsync(CommonResponse<PaymentSummaryDto>.SuccessResponse("Success", expected));

            // Act
            var response = await _client.GetAsync(
                $"/api/v1/Fee/payment-summary/{TestStudentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<CommonResponse<PaymentSummaryDto>>();

            result.Should().NotBeNull();
            result!.Data.GrandTotal.Should().Be(54000m);
            result.Data.GrandPaid.Should().Be(27000m);
            result.Data.Selections.Should().HaveCount(1);
        }

        /// <summary>
        /// Verifies GET student fee page returns error when service throws.
        /// </summary>
        [Fact]
        public async Task GetStudentFeePage_ServiceThrows_ReturnsErrorResponse()
        {
            // Arrange
            var unknownId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetStudentFeePageAsync(unknownId))
                .ThrowsAsync(new AppException(
                    "Student not found.", 404, "STUDENT_NOT_FOUND"));

            // Act
            var response = await _client.GetAsync($"/api/v1/Fee/student-page/{unknownId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
