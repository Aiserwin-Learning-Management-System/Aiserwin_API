namespace Winfocus.LMS.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Configurations;
    using Winfocus.LMS.Infrastructure.Data.Configurations;
    using Winfocus.LMS.Infrastructure.Persistence.Configurations;

    /// <summary>
    /// Represents the application's database context for Entity Framework Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="opts">The options to be used by a <see cref="DbContext"/>.</param>
        public AppDbContext(DbContextOptions<AppDbContext> opts)
            : base(opts)
        {
        }

        /// <summary>
        /// Gets or sets the countries in the database.
        /// </summary>
        public DbSet<Country> Countries { get; set; } = null!;

        /// <summary>
        /// Gets or sets the centres in the database.
        /// </summary>
        public DbSet<Center> Centres { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Doubt Clearing in the database.
        /// </summary>
        public DbSet<DoubtClearing> DoubtClearing { get; set; } = null!;

        /// <summary>
        /// Gets or sets the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Gets or sets the roles in the database.
        /// </summary>
        public DbSet<Role> Roles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the UserRoles in the database.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the batches in the database.
        /// </summary>
        public DbSet<Batch> Batches { get; set; } = null!;

        /// <summary>
        /// Gets or sets the batches timing monday to friday in the database.
        /// </summary>
        public DbSet<BatchTimingMTF> BatchTimingMTFs { get; set; } = null!;

        /// <summary>
        /// Gets or sets the batches timing saturday in the database.
        /// </summary>
        public DbSet<BatchTimingSaturday> BatchTimingSaturdays { get; set; } = null!;

        /// <summary>
        /// Gets or sets the batches timing sunday in the database.
        /// </summary>
        public DbSet<BatchTimingSunday> BatchTimingSundays { get; set; } = null!;

        /// <summary>
        /// Gets or sets the course in the database.
        /// </summary>
        public DbSet<Course> Courses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the grades  in the database.
        /// </summary>
        public DbSet<Grade> Grades { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Mode of study in the database.
        /// </summary>
        public DbSet<ModeOfStudy> ModeOfStudies { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Preferred batches  in the database.
        /// </summary>
        public DbSet<PreferredBatch> PreferredBatches { get; set; } = null!;

        /// <summary>
        /// Gets or sets the states  in the database.
        /// </summary>
        public DbSet<State> States { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student  in the database.
        /// </summary>
        public DbSet<Student> Students { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student academic details  in the database.
        /// </summary>
        public DbSet<StudentAcademicDetails> StudentAcademicDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student documents  in the database.
        /// </summary>
        public DbSet<StudentDocuments> StudentDocuments { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student personal details  in the database.
        /// </summary>
        public DbSet<StudentPersonalDetails> StudentPersonalDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sunject  in the database.
        /// </summary>
        public DbSet<Subject> Subjects { get; set; } = null!;

        /// <summary>
        /// Gets or sets the syllabus  in the database.
        /// </summary>
        public DbSet<Syllabus> Syllabuses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the streams  in the database.
        /// </summary>
        public DbSet<Streams> Streams { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SubjectBatchTimingMTFs  in the database.
        /// </summary>
        public DbSet<SubjectBatchTimingMTF> SubjectBatchTimingMTFs { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SubjectBatchTimingSaturdays  in the database.
        /// </summary>
        public DbSet<SubjectBatchTimingSaturday> SubjectBatchTimingSaturdays { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SubjectBatchTimingSundays  in the database.
        /// </summary>
        public DbSet<SubjectBatchTimingSunday> SubjectBatchTimingSundays { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fee plans.
        /// </summary>
        /// <value>
        /// The fee plans.
        /// </value>
        public DbSet<FeePlan> FeePlans { get; set; }

        /// <summary>
        /// Gets or sets the fee installments.
        /// </summary>
        /// <value>
        /// The fee installments.
        /// </value>
        public DbSet<FeeInstallment> FeeInstallments { get; set; }

        /// <summary>
        /// Gets or sets the student fee selections.
        /// </summary>
        /// <value>
        /// The student fee selections.
        /// </value>
        public DbSet<StudentFeeSelection> StudentFeeSelections { get; set; }

        /// <summary>
        /// Gets or sets the student installments.
        /// </summary>
        /// <value>
        /// The student installments.
        /// </value>
        public DbSet<StudentInstallment> StudentInstallments { get; set; }

        /// <summary>
        /// Gets or sets the student academic couses.
        /// </summary>
        /// <value>
        /// The student academic couses.
        /// </value>
        public DbSet<StudentAcademicCouses> StudentAcademicCouses { get; set; }

        /// <summary>
        /// Gets or sets the student academic batchtiming mtfs.
        /// </summary>
        /// <value>
        /// The student academic batchtiming mtf.
        /// </value>
        public DbSet<StudentBatchTimingMTF> StudentBatchTimingMTFs { get; set; }

        /// <summary>
        /// Gets or sets the student academic batchtiming saturdays.
        /// </summary>
        /// <value>
        /// The student academic batchtiming saturdays.
        /// </value>
        public DbSet<StudentBatchTimingSaturday> StudentBatchTimingSaturdays { get; set; }

        /// <summary>
        /// Gets or sets the student academic batchtiming sundays.
        /// </summary>
        /// <value>
        /// The student academic batchtiming sundays.
        /// </value>
        public DbSet<StudentBatchTimingSunday> StudentBatchTimingSundays { get; set; }

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        /// <value>
        /// The academic year.
        /// </value>
        public DbSet<AcademicYear> AcademicYears { get; set; }

        /// <summary>
        /// Gets or sets the user activation token.
        /// </summary>
        /// <value>
        /// The user activation token.
        /// </value>
        public DbSet<UserActivationToken> UserActivationTokens { get; set; }

        /// <summary>
        /// Gets or sets the FeePlanDiscount.
        /// </summary>
        /// <value>
        /// The FeePlanDiscount.
        /// </value>
        public DbSet<FeePlanDiscount> FeePlanDiscount { get; set; }

        /// <summary>
        /// Gets or sets stores all available system permissions.
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets stores the mapping between roles and permissions.
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Gets the user login logs.
        /// </summary>
        /// <value>
        /// The user login logs.
        /// </value>
        public DbSet<UserLoginLog> UserLoginLogs => Set<UserLoginLog>();

        /// <summary>
        /// Gets or sets the user active sessions.
        /// </summary>
        public DbSet<UserActiveSession> UserActiveSessions { get; set; } = null!;

        /// <summary>
        /// Gets or sets the staff categories.
        /// </summary>
        /// <value>
        /// The staff categories.
        /// </value>
        public DbSet<StaffCategory> StaffCategories { get; set; } = null!;

        /// <summary>
        /// Gets or sets the field groups in the database.
        /// </summary>
        public DbSet<FieldGroup> FieldGroups { get; set; } = null!;

        /// <summary>
        /// Gets or sets the form fields in the database.
        /// </summary>
        public DbSet<FormField> FormFields { get; set; } = null!;

        /// <summary>
        /// Gets or sets the field options in the database.
        /// </summary>
        public DbSet<FieldOption> FieldOptions { get; set; } = null!;

        /// <summary>
        /// Gets or sets the registration forms in the database.
        /// </summary>
        public DbSet<RegistrationForm> RegistrationForms { get; set; } = null!;

        /// <summary>
        /// Gets or sets the registration form groups in the database.
        /// </summary>
        public DbSet<RegistrationFormGroup> RegistrationFormGroups { get; set; } = null!;

        /// <summary>
        /// Gets or sets the registration form fields in the database.
        /// </summary>
        public DbSet<RegistrationFormField> RegistrationFormFields { get; set; } = null!;

        /// <summary>
        /// Gets or sets the staff registrations in the database.
        /// </summary>
        public DbSet<StaffRegistration> StaffRegistrations { get; set; } = null!;

        /// <summary>
        /// Gets or sets the staff registration values in the database.
        /// </summary>
        public DbSet<StaffRegistrationValue> StaffRegistrationValues { get; set; } = null!;

        /// <summary>
        /// Gets or sets the page headings.
        /// </summary>
        /// <value>
        /// The page headings.
        /// </value>
        public DbSet<PageHeading> PageHeadings { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam syllabuses.
        /// </summary>
        /// <value>
        /// The exam syllabuses.
        /// </value>
        public DbSet<ExamSyllabus> ExamSyllabuses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam grades.
        /// </summary>
        /// <value>
        /// The exam grades.
        /// </value>
        public DbSet<ExamGrade> ExamGrades { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam subjects.
        /// </summary>
        /// <value>
        /// The exam subjects.
        /// </value>
        public DbSet<ExamSubject> ExamSubjects { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam units.
        /// </summary>
        /// <value>
        /// The exam units.
        /// </value>
        public DbSet<ExamUnit> ExamUnits { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam chapters.
        /// </summary>
        /// <value>
        /// The exam chapters.
        /// </value>
        public DbSet<ExamChapter> ExamChapters { get; set; } = null!;

        /// <summary>
        /// Gets or sets the content resource types.
        /// </summary>
        /// <value>
        /// The content resource types.
        /// </value>
        public DbSet<ContentResourceType> ContentResourceTypes { get; set; } = null!;

        /// <summary>Gets or sets the task assignments.</summary>
        public DbSet<TaskAssignment> TaskAssignments { get; set; } = null!;

        /// <summary>Gets or sets the questions.</summary>
        public DbSet<Question> Questions { get; set; } = null!;

        /// <summary>Gets or sets the question options.</summary>
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;

        /// <summary>Gets or sets the question reviews.</summary>
        public DbSet<QuestionReview> QuestionReviews { get; set; } = null!;

        /// <summary>Gets or sets the daily activity reports.</summary>
        public DbSet<DailyActivityReport> DailyActivityReports { get; set; } = null!;

        /// <summary>Gets or sets the guidelines.</summary>
        public DbSet<Guideline> Guidelines { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student course discounts.
        /// </summary>
        /// <value>
        /// The student course discounts.
        /// </value>
        public DbSet<StudentCourseDiscount> StudentCourseDiscounts { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student fee discounts.
        /// </summary>
        /// <value>
        /// The student fee discounts.
        /// </value>
        public DbSet<StudentFeeDiscount> StudentFeeDiscounts { get; set; } = null!;

        /// <summary>
        /// Gets or sets the question configurations.
        /// </summary>
        /// <value>
        /// The question configurations.
        /// </value>
        public DbSet<QuestionConfiguration> QuestionConfigurations { get; set; } = null!;

        /// <summary>
        /// Gets or sets the question type configuration.
        /// </summary>
        /// <value>
        /// The question type configuration.
        /// </value>
        public DbSet<QuestionTypeConfig> QuestionTypeConfigs { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam.
        /// </summary>
        /// <value>
        /// The exam details.
        /// </value>
        public DbSet<Exam> Exams { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam question.
        /// </summary>
        /// <value>
        /// The exam and question identifiers.
        /// </value>
        public DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;

        /// <summary>
        /// Configures the model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StreamConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new UserActivationTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginLogConfiguration());
            modelBuilder.ApplyConfiguration(new UserActiveSessionConfiguration());
            modelBuilder.ApplyConfiguration(new StaffCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FieldGroupConfiguration());
            modelBuilder.ApplyConfiguration(new FormFieldConfiguration());
            modelBuilder.ApplyConfiguration(new FieldOptionConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationFormConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationFormGroupConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationFormFieldConfiguration());
            modelBuilder.ApplyConfiguration(new StaffRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new StaffRegistrationValueConfiguration());
            modelBuilder.ApplyConfiguration(new PageHeadingConfiguration());

            modelBuilder.ApplyConfiguration(new ExamSyllabusConfiguration());
            modelBuilder.ApplyConfiguration(new ExamGradeConfiguration());
            modelBuilder.ApplyConfiguration(new ExamSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new ExamUnitConfiguration());
            modelBuilder.ApplyConfiguration(new ExamChapterConfiguration());
            modelBuilder.ApplyConfiguration(new ContentResourceTypeConfiguration());

            modelBuilder.ApplyConfiguration(new TaskAssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfig());
            modelBuilder.ApplyConfiguration(new QuestionOptionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionReviewConfiguration());
            modelBuilder.ApplyConfiguration(new DailyActivityReportConfiguration());
            modelBuilder.ApplyConfiguration(new GuidelineConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfigurationConfiguration());

            modelBuilder.ApplyConfiguration(new QuestionTypeConfigConfiguration());

            modelBuilder.ApplyConfiguration(new ExamConfiguration());
            modelBuilder.ApplyConfiguration(new ExamQuestionConfiguration());

            modelBuilder.ApplyConfiguration(new StudentCourseDiscountConfiguration());
            modelBuilder.ApplyConfiguration(new StudentFeeDiscountConfiguration());
            modelBuilder.ApplyConfiguration(new StudentFeeSelectionConfiguration());
            modelBuilder.ApplyConfiguration(new StudentInstallmentConfiguration());

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            // Role configuration
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            // UserRole configuration (many-to-many)
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Country>(e =>
            {
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Country>(e =>
            {
                e.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SubjectBatchTimingMTF>()
                .HasKey(x => new { x.SubjectId, x.BatchTimingId });
            modelBuilder.Entity<SubjectBatchTimingSaturday>()
               .HasKey(x => new { x.SubjectId, x.BatchTimingId });
            modelBuilder.Entity<SubjectBatchTimingSunday>()
               .HasKey(x => new { x.SubjectId, x.BatchTimingId });

            modelBuilder.Entity<StudentFeeSelection>()
            .HasIndex(x => new { x.StudentId, x.CourseId })
            .IsUnique();

            modelBuilder.Entity<StudentAcademicCouses>()
               .HasKey(x => new { x.StudentId, x.CourseId });
            modelBuilder.Entity<StudentBatchTimingMTF>()
               .HasKey(x => new { x.StudentId, x.BatchTimingMTFId });
            modelBuilder.Entity<StudentBatchTimingSaturday>()
               .HasKey(x => new { x.StudentId, x.BatchTimingSaturdayId });
            modelBuilder.Entity<StudentBatchTimingSunday>()
               .HasKey(x => new { x.StudentId, x.BatchTimingSundayId });

            modelBuilder.Entity<FeePlan>()
                .HasOne(fp => fp.Course)
                .WithMany(c => c.FeePlans)
                .HasForeignKey(fp => fp.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FeePlan>()
                .HasOne(fp => fp.Subject)
                .WithMany()
                .HasForeignKey(fp => fp.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Ensure email is globally unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ensure username is globally unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Center>(e =>
            {
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.CenterType).HasConversion<int>();

                e.HasOne(x => x.Country)
                 .WithMany(x => x.Centers)
                 .HasForeignKey(x => x.CountryId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.modeOfStudy)
                 .WithMany()
                 .HasForeignKey(x => x.ModeOfStudyId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(c => c.State)
                 .WithMany(s => s.Centers)
                 .HasForeignKey(c => c.StateId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<State>()
                .HasOne(s => s.ModeOfStudy)
                .WithMany()
                .HasForeignKey(s => s.ModeOfStudyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentAcademicDetails>(e =>
            {
                e.HasOne(s => s.Country)
                 .WithMany()
                 .HasForeignKey(s => s.CountryId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.ModeOfStudy)
                 .WithMany()
                 .HasForeignKey(s => s.ModeOfStudyId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.State)
                 .WithMany()
                 .HasForeignKey(s => s.StateId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.Center)
                 .WithMany()
                 .HasForeignKey(s => s.CenterId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.Syllabus)
                 .WithMany()
                 .HasForeignKey(s => s.SyllabusId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.Grade)
                 .WithMany()
                 .HasForeignKey(s => s.GradeId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.Stream)
                 .WithMany()
                 .HasForeignKey(s => s.StreamId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(s => s.Subject)
                 .WithMany()
                 .HasForeignKey(s => s.SubjectId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne<AcademicYear>()
                 .WithMany()
                 .HasForeignKey(s => s.AcademicYearId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StudentFeeSelection>(e =>
            {
                e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();

                e.HasOne(sfs => sfs.Student)
                    .WithMany()
                   .HasForeignKey(sfs => sfs.StudentId)
                   .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(sfs => sfs.FeePlan)
                    .WithMany()
                    .HasForeignKey(sfs => sfs.FeePlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(sfs => sfs.Course)
                    .WithMany()
                    .HasForeignKey(sfs => sfs.CourseId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Batch>(e =>
            {
                e.HasOne(b => b.Subject)
                 .WithMany()
                 .HasForeignKey(b => b.SubjectId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BatchTimingMTF>(e =>
            {
                e.HasOne(btmf => btmf.Subject)
                 .WithMany()
                 .HasForeignKey(btmf => btmf.SubjectId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BatchTimingSaturday>(e =>
            {
                e.HasOne(bts => bts.Subject)
                 .WithMany()
                 .HasForeignKey(bts => bts.SubjectId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BatchTimingSunday>(e =>
            {
                e.HasOne(bts => bts.Subject)
                 .WithMany()
                 .HasForeignKey(bts => bts.SubjectId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.Property(s => s.UserId)
                    .IsRequired(false)
                    .HasColumnType("uniqueidentifier");

                e.HasIndex(s => s.UserId)
                    .IsUnique()
                    .HasFilter("[UserId] IS NOT NULL")
                    .HasDatabaseName("IX_Students_UserId_Unique");

                e.HasOne(s => s.User)
                    .WithOne()
                    .HasForeignKey<Student>(s => s.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
        /// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        /// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        /// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        /// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information
        /// and examples.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
        /// </para>
        /// </remarks>
        public override int SaveChanges()
        {
            HandleSoftDelete();
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
        /// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        /// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        /// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        /// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more
        /// information and examples.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
        /// </para>
        /// </remarks>
        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            HandleSoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDelete()
        {
            var deletedEntries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in deletedEntries)
            {
                // Convert hard delete → soft delete
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.IsActive = false;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
