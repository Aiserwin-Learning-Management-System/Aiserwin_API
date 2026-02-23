namespace Winfocus.LMS.Application.Services
{
    using Winfocus.LMS.Application.DTOs.Fees;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// FeeService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFeeService" />
    public sealed class FeeService : IFeeService
    {
        private readonly IFeeRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public FeeService(IFeeRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets the fee page asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>
        /// Task&lt;FeePageResponseDto&gt;.
        /// </returns>
        /// <exception cref="System.Exception">Student not found.</exception>
        public async Task<FeePageResponseDto> GetFeePageAsync(Guid studentId)
        {
            var student = await _repo.GetStudentWithCoursesAsync(studentId);

            if (student == null)
            {
                throw new Exception("Student not found");
            }

            var selectedCourseIds = student.StudentAcademicCouses
                .Select(x => x.CourseId)
                .ToList();

            var courses = await _repo.GetCoursesByGradeAsync(student.AcademicDetails.GradeId);

            var pricingTable = courses
                .SelectMany(course => course.FeePlans, (course, feePlan) =>
                {
                    var baseAmount = feePlan.TuitionFee + feePlan.RegistrationFee;

                    var afterScholarship =
                        baseAmount - (baseAmount * feePlan.ScholarshipPercent / 100);

                    var afterSeasonal =
                        afterScholarship - (afterScholarship * feePlan.SeasonalPercent / 100);

                    return new FeeRowDto
                    {
                        CourseId = course.Id,
                        FeePlanId = feePlan.Id,
                        CourseName = course.Name,
                        YearlyFee = baseAmount,
                        PaymentType = feePlan.PlanName,
                        ScholarshipPercent = feePlan.ScholarshipPercent,
                        SeasonalPercent = feePlan.SeasonalPercent,
                        FeeAfterDiscount = afterSeasonal,
                        IsSelected = selectedCourseIds.Contains(course.Id),
                    };
                })
                .ToList();

            return new FeePageResponseDto
            {
                PricingTable = pricingTable,
            };
        }

        /// <summary>
        /// Selects the fee asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Task&lt;FeeSummaryDto&gt;.
        /// </returns>
        /// <exception cref="System.Exception">Student not found.</exception>
        public async Task<FeeSummaryDto> SelectFeeAsync(SelectFeeRequestDto request)
        {
            var student = await _repo.GetStudentWithCoursesAsync(request.StudentId);

            if (student == null)
            {
                throw new Exception("Student not found");
            }

            var courses = await _repo.GetCoursesByGradeAsync(student.AcademicDetails.GradeId);

            var feePlan = courses
                .SelectMany(c => c.FeePlans)
                .First(x => x.Id == request.FeePlanId);

            var baseAmount = feePlan.TuitionFee + feePlan.RegistrationFee;

            var afterScholarship =
                baseAmount - (baseAmount * feePlan.ScholarshipPercent / 100);

            var afterSeasonal =
                afterScholarship - (afterScholarship * feePlan.SeasonalPercent / 100);

            var final =
                afterSeasonal - (afterSeasonal * request.ManualDiscountPercent / 100);

            var selection = new StudentFeeSelection(
                request.StudentId,
                feePlan.CourseId,
                feePlan.Id,
                request.ManualDiscountPercent,
                final);

            await _repo.AddStudentFeeSelectionAsync(selection);
            await _repo.SaveChangesAsync();

            return new FeeSummaryDto
            {
                BaseFee = baseAmount,
                TotalPayable = final,
            };
        }
    }
}
