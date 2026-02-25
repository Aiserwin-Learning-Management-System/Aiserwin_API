namespace Winfocus.LMS.Infrastructure.Tests.Repositories
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;
    using Winfocus.LMS.Infrastructure.Repositories;
    using Winfocus.LMS.Infrastructure.Tests.Common;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="FeeRepository"/> using InMemory database via
    /// <see cref="DbContextTestBase"/>. Each test gets a fresh database.
    /// Focuses on data persistence, querying, filtering, and null handling.
    /// </summary>
    public sealed class FeeRepositoryTests : DbContextTestBase
    {
        private static readonly Guid StudentId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private static readonly Guid GradeId =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

        private static readonly Guid CourseId =
            Guid.Parse("33333333-3333-3333-3333-333333333333");

        private static readonly Guid FeePlanId =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        private static readonly Guid SelectionId =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        private static readonly Guid NonExistentId =
            Guid.Parse("99999999-9999-9999-9999-999999999999");

        /// <summary>
        /// Verifies a new selection entity is persisted to the database.
        /// </summary>
        [Fact]
        public async Task AddStudentFeeSelectionAsync_PersistsEntity()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var selection = BuildSelection();

            // Act
            await repo.AddStudentFeeSelectionAsync(selection);
            await repo.SaveChangesAsync();

            // Assert
            var saved = await context.StudentFeeSelections
                .FirstOrDefaultAsync(s => s.Id == SelectionId);

            saved.Should().NotBeNull();
            saved!.StudentId.Should().Be(StudentId);
            saved.CourseId.Should().Be(CourseId);
            saved.FeePlanId.Should().Be(FeePlanId);
        }

        /// <summary>
        /// Verifies all discount fields are correctly persisted.
        /// </summary>
        [Fact]
        public async Task AddStudentFeeSelectionAsync_PersistsAllDiscountFields()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var selection = BuildSelection();

            // Act
            await repo.AddStudentFeeSelectionAsync(selection);
            await repo.SaveChangesAsync();

            // Assert
            var saved = await context.StudentFeeSelections
                .FirstAsync(s => s.Id == SelectionId);

            saved.ScholarshipPercent.Should().Be(10m);
            saved.IsScholarshipActive.Should().BeTrue();
            saved.SeasonalPercent.Should().Be(5m);
            saved.IsSeasonalActive.Should().BeTrue();
            saved.ManualDiscountPercent.Should().Be(3m);
            saved.IsManualDiscountActive.Should().BeTrue();
            saved.BaseFee.Should().Be(60000m);
            saved.FinalAmount.Should().Be(49761m);
        }

        /// <summary>
        /// Verifies BaseFee and FinalAmount are persisted for audit.
        /// </summary>
        [Fact]
        public async Task AddStudentFeeSelectionAsync_PersistsBaseFeeAndFinalAmount()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var selection = new StudentFeeSelection(
                StudentId, CourseId, FeePlanId,
                0m, false, 0m, false, 0m, false,
                75000m, 75000m);
            selection.Id = Guid.NewGuid();

            // Act
            await repo.AddStudentFeeSelectionAsync(selection);
            await repo.SaveChangesAsync();

            // Assert
            var saved = await context.StudentFeeSelections
                .FirstAsync(s => s.Id == selection.Id);

            saved.BaseFee.Should().Be(75000m);
            saved.FinalAmount.Should().Be(75000m);
        }

        /// <summary>
        /// Verifies the selection is returned for an existing ID.
        /// </summary>
        [Fact]
        public async Task GetStudentFeeSelectionByIdAsync_Exists_ReturnsEntity()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var selection = BuildSelection();
            context.StudentFeeSelections.Add(selection);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetStudentFeeSelectionByIdAsync(SelectionId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(SelectionId);
            result.StudentId.Should().Be(StudentId);
        }

        /// <summary>
        /// Verifies null is returned for a non-existent ID.
        /// </summary>
        [Fact]
        public async Task GetStudentFeeSelectionByIdAsync_NotExists_ReturnsNull()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            // Act
            var result = await repo.GetStudentFeeSelectionByIdAsync(NonExistentId);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Verifies only active selections for the student are returned.
        /// </summary>
        [Fact]
        public async Task GetSelectionsByStudent_ReturnsOnlyActiveSelections()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var active = BuildSelection(Guid.NewGuid());
            active.IsActive = true;

            var inactive = BuildSelection(Guid.NewGuid());
            inactive.IsActive = false;

            context.StudentFeeSelections.AddRange(active, inactive);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetStudentFeeSelectionsByStudentAsync(StudentId);

            // Assert
            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(active.Id);
        }

        /// <summary>
        /// Verifies selections from other students are excluded.
        /// </summary>
        [Fact]
        public async Task GetSelectionsByStudent_ExcludesOtherStudents()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var otherStudentId = Guid.NewGuid();
            var otherSelection = new StudentFeeSelection(
                otherStudentId, CourseId, FeePlanId,
                10m, true, 5m, true, 0m, false,
                60000m, 51300m);
            otherSelection.Id = Guid.NewGuid();
            otherSelection.IsActive = true;

            context.StudentFeeSelections.Add(otherSelection);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetStudentFeeSelectionsByStudentAsync(StudentId);

            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Verifies empty list when student has no selections.
        /// </summary>
        [Fact]
        public async Task GetSelectionsByStudent_NoSelections_ReturnsEmptyList()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            // Act
            var result = await repo.GetStudentFeeSelectionsByStudentAsync(StudentId);

            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Verifies multiple active selections are returned.
        /// </summary>
        [Fact]
        public async Task GetSelectionsByStudent_MultipleActive_ReturnsAll()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var sel1 = BuildSelection(Guid.NewGuid());
            var sel2 = BuildSelection(Guid.NewGuid());
            var sel3 = BuildSelection(Guid.NewGuid());

            context.StudentFeeSelections.AddRange(sel1, sel2, sel3);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetStudentFeeSelectionsByStudentAsync(StudentId);

            // Assert
            result.Should().HaveCount(3);
        }

        /// <summary>
        /// Verifies the fee plan is returned for an existing ID.
        /// </summary>
        [Fact]
        public async Task GetFeePlanByIdAsync_Exists_ReturnsEntity()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var plan = BuildFeePlan();
            context.FeePlans.Add(plan);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetFeePlanByIdAsync(FeePlanId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(FeePlanId);
            result.TuitionFee.Should().Be(60000m);
            result.PlanName.Should().Be("Yearly");
        }

        /// <summary>
        /// Verifies null is returned for a non-existent ID.
        /// </summary>
        [Fact]
        public async Task GetFeePlanByIdAsync_NotExists_ReturnsNull()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            // Act
            var result = await repo.GetFeePlanByIdAsync(NonExistentId);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Verifies all discount properties are returned on the plan.
        /// </summary>
        [Fact]
        public async Task GetFeePlanByIdAsync_ReturnsDiscountProperties()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var plan = BuildFeePlan();
            context.FeePlans.Add(plan);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetFeePlanByIdAsync(FeePlanId);

            // Assert
            result!.ScholarshipPercent.Should().Be(10m);
            result.SeasonalPercent.Should().Be(5m);
            result.IsSeasonalDiscountActive.Should().BeTrue();
        }

        /// <summary>
        /// Verifies that modifying a tracked entity and calling SaveChanges persists.
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_ModifiedEntity_PersistsChanges()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var selection = BuildSelectionNoDiscounts();
            context.StudentFeeSelections.Add(selection);
            await context.SaveChangesAsync();

            // Act – modify and save through the repo
            var tracked = await repo.GetStudentFeeSelectionByIdAsync(selection.Id);
            tracked!.UpdateManualDiscount(7m, true);
            await repo.SaveChangesAsync();

            // Assert – reload from context
            var reloaded = await context.StudentFeeSelections
                .FirstAsync(s => s.Id == selection.Id);

            reloaded.ManualDiscountPercent.Should().Be(7m);
            reloaded.IsManualDiscountActive.Should().BeTrue();
            reloaded.FinalAmount.Should().BeLessThan(60000m);
        }

        /// <summary>
        /// Verifies that modifying a fee plan's seasonal discount persists.
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_FeePlanSeasonalUpdate_PersistsChanges()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new FeeRepository(context);

            var plan = BuildFeePlan();
            context.FeePlans.Add(plan);
            await context.SaveChangesAsync();

            // Act
            var tracked = await repo.GetFeePlanByIdAsync(FeePlanId);
            tracked!.UpdateSeasonalDiscount(15m, false);
            await repo.SaveChangesAsync();

            // Assert
            var reloaded = await context.FeePlans
                .FirstAsync(p => p.Id == FeePlanId);

            reloaded.SeasonalPercent.Should().Be(15m);
            reloaded.IsSeasonalDiscountActive.Should().BeFalse();
        }

        /// <summary>
        /// Builds a StudentFeeSelection with all 3 discounts active.
        /// </summary>
        private static StudentFeeSelection BuildSelection(
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
        /// Builds a StudentFeeSelection with no discounts.
        /// </summary>
        private static StudentFeeSelection BuildSelectionNoDiscounts(
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

        /// <summary>
        /// Builds a FeePlan using reflection for private setters.
        /// </summary>
        private static FeePlan BuildFeePlan(Guid? planId = null)
        {
            var plan = (FeePlan)Activator.CreateInstance(
                typeof(FeePlan), nonPublic: true)!;

            plan.Id = planId ?? FeePlanId;
            plan.IsActive = true;
            plan.CreatedAt = DateTime.UtcNow;

            SetPrivate(plan, "CourseId", CourseId);
            SetPrivate(plan, "PlanName", "Yearly");
            SetPrivate(plan, "TuitionFee", 60000m);
            SetPrivate(plan, "RegistrationFee", 5000m);
            SetPrivate(plan, "ScholarshipPercent", 10m);
            SetPrivate(plan, "SeasonalPercent", 5m);
            SetPrivate(plan, "IsSeasonalDiscountActive", true);
            SetPrivate(plan, "IsInstallmentAllowed", false);

            return plan;
        }

        /// <summary>
        /// Sets a property value via reflection, bypassing private setters.
        /// </summary>
        private static void SetPrivate<T>(
            T entity, string propertyName, object? value)
            where T : class
        {
            typeof(T).GetProperty(propertyName)!.SetValue(entity, value);
        }
    }
}
