namespace Winfocus.LMS.Application.Tests.Common
{
    using System.Reflection;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Reflection-based factory for creating domain entities with private setters.
    /// Shared across Application and Infrastructure test projects.
    /// </summary>
    public static class FeeTestEntityFactory
    {
        /// <summary>The default student identifier.</summary>
        public static readonly Guid StudentId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        /// <summary>The default grade identifier.</summary>
        public static readonly Guid GradeId =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

        /// <summary>The default course identifier.</summary>
        public static readonly Guid CourseId =
            Guid.Parse("33333333-3333-3333-3333-333333333333");

        /// <summary>The default fee plan identifier.</summary>
        public static readonly Guid FeePlanId =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        /// <summary>The default stream identifier.</summary>
        public static readonly Guid StreamId =
            Guid.Parse("55555555-5555-5555-5555-555555555555");

        /// <summary>The default selection identifier.</summary>
        public static readonly Guid SelectionId =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        /// <summary>A non-existent identifier for negative tests.</summary>
        public static readonly Guid NonExistentId =
            Guid.Parse("99999999-9999-9999-9999-999999999999");

        /// <summary>
        /// Creates an entity instance using its private parameterless constructor.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>A new uninitialized entity instance.</returns>
        public static T Create<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;
        }

        /// <summary>
        /// Sets a property value via reflection, bypassing private setters.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The same entity instance for fluent chaining.</returns>
        public static T Set<T>(this T entity, string propertyName, object? value)
            where T : class
        {
            var prop = typeof(T).GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop == null)
            {
                throw new ArgumentException(
                    $"Property '{propertyName}' not found on '{typeof(T).Name}'.");
            }

            prop.SetValue(entity, value);
            return entity;
        }

        /// <summary>
        /// Creates a FeePlan with standard test values.
        /// TuitionFee=60000, RegistrationFee=5000, Scholarship=10%, Seasonal=5% active.
        /// </summary>
        /// <param name="feePlanId">Optional ID override.</param>
        /// <param name="courseId">Optional CourseId override.</param>
        /// <returns>A configured FeePlan.</returns>
        public static FeePlan BuildFeePlan(
            Guid? feePlanId = null,
            Guid? courseId = null)
        {
            var plan = Create<FeePlan>();
            plan.Id = feePlanId ?? FeePlanId;
            plan.IsActive = true;
            plan.CreatedAt = DateTime.UtcNow;

            plan
                .Set("CourseId", courseId ?? CourseId)
                .Set("PlanName", "Yearly")
                .Set("TuitionFee", 60000m)
                .Set("RegistrationFee", 5000m)
                .Set("ScholarshipPercent", 10m)
                .Set("SeasonalPercent", 5m)
                .Set("IsSeasonalDiscountActive", true)
                .Set("IsInstallmentAllowed", false);

            return plan;
        }

        /// <summary>
        /// Creates a FeePlan with zero discounts and seasonal inactive.
        /// </summary>
        /// <param name="feePlanId">Optional ID override.</param>
        /// <param name="courseId">Optional CourseId override.</param>
        /// <returns>A configured FeePlan with no discounts.</returns>
        public static FeePlan BuildFeePlanNoDiscounts(
            Guid? feePlanId = null,
            Guid? courseId = null)
        {
            var plan = BuildFeePlan(feePlanId, courseId);
            plan
                .Set("ScholarshipPercent", 0m)
                .Set("SeasonalPercent", 0m)
                .Set("IsSeasonalDiscountActive", false);
            return plan;
        }

        /// <summary>
        /// Creates a Course entity with a Stream and a FeePlan attached.
        /// </summary>
        /// <param name="courseId">Optional CourseId override.</param>
        /// <param name="feePlan">Optional FeePlan to attach.</param>
        /// <returns>A configured Course with navigation properties.</returns>
        public static Course BuildCourse(
            Guid? courseId = null,
            FeePlan? feePlan = null)
        {
            var id = courseId ?? CourseId;

            var plan = feePlan ?? BuildFeePlan(courseId: id);
            plan.Set("CourseId", id);

            var stream = Create<Streams>();
            stream.Id = StreamId;
            stream.IsActive = true;
            stream.Set("GradeId", GradeId);

            var subject = Create<Subject>();
            subject.Id = Guid.NewGuid();
            subject.IsActive = true;
            subject.Set("Name", "Physics Subject");

            var course = Create<Course>();
            course.Id = id;
            course.IsActive = true;
            course.CreatedAt = DateTime.UtcNow;
            course
                .Set("Name", "Physics")
                .Set("Stream", stream)
                .Set("Subject", subject)
                .Set("FeePlans", new List<FeePlan> { plan });

            return course;
        }

        /// <summary>
        /// Creates a Student with AcademicDetails and empty course selections.
        /// </summary>
        /// <param name="studentId">Optional StudentId override.</param>
        /// <param name="gradeId">Optional GradeId override.</param>
        /// <returns>A configured Student entity.</returns>
        public static Student BuildStudent(
            Guid? studentId = null,
            Guid? gradeId = null)
        {
            var academicDetails = Create<StudentAcademicDetails>();
            academicDetails.Id = Guid.NewGuid();
            academicDetails.Set("GradeId", gradeId ?? GradeId);

            var student = Create<Student>();
            student.Id = studentId ?? StudentId;
            student.IsActive = true;
            student.CreatedAt = DateTime.UtcNow;
            student
                .Set("AcademicDetails", academicDetails)
                .Set("StudentAcademicCouses", new List<StudentAcademicCouses>());

            return student;
        }

        /// <summary>
        /// Creates a Student with one pre-selected course.
        /// </summary>
        /// <param name="selectedCourseId">The already-selected course identifier.</param>
        /// <returns>A configured Student with one course selected.</returns>
        public static Student BuildStudentWithCourse(Guid selectedCourseId)
        {
            var student = BuildStudent();

            var sac = Create<StudentAcademicCouses>();
            sac.Set("CourseId", selectedCourseId);

            student.Set(
                "StudentAcademicCouses",
                new List<StudentAcademicCouses> { sac });

            return student;
        }


        /// <summary>
        /// Creates a StudentFeeSelection with all three discounts active.
        /// Scholarship=10%, Seasonal=5%, Manual=3%.
        /// </summary>
        /// <param name="selectionId">Optional SelectionId override.</param>
        /// <returns>A configured StudentFeeSelection with recalculated final amount.</returns>
        public static StudentFeeSelection BuildSelection(
            Guid? selectionId = null)
        {
            var selection = new StudentFeeSelection(
                studentId: StudentId,
                courseId: CourseId,
                feePlanId: FeePlanId,
                scholarshipPercent: 10m,
                isScholarshipActive: true,
                seasonalPercent: 5m,
                isSeasonalActive: true,
                manualDiscountPercent: 3m,
                isManualDiscountActive: true,
                baseFee: 60000m,
                finalAmount: 0m);

            selection.Id = selectionId ?? SelectionId;
            selection.IsActive = true;
            selection.CreatedAt = DateTime.UtcNow;
            selection.RecalculateFinalAmount();

            return selection;
        }

        /// <summary>
        /// Creates a StudentFeeSelection with all discounts off.
        /// </summary>
        /// <param name="selectionId">Optional SelectionId override.</param>
        /// <returns>A selection with BaseFee=FinalAmount=60000.</returns>
        public static StudentFeeSelection BuildSelectionNoDiscounts(
            Guid? selectionId = null)
        {
            var selection = new StudentFeeSelection(
                studentId: StudentId,
                courseId: CourseId,
                feePlanId: FeePlanId,
                scholarshipPercent: 0m,
                isScholarshipActive: false,
                seasonalPercent: 0m,
                isSeasonalActive: false,
                manualDiscountPercent: 0m,
                isManualDiscountActive: false,
                baseFee: 60000m,
                finalAmount: 60000m);

            selection.Id = selectionId ?? SelectionId;
            selection.IsActive = true;
            selection.CreatedAt = DateTime.UtcNow;

            return selection;
        }
    }
}
