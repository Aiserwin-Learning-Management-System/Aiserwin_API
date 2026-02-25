namespace Winfocus.LMS.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

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
        public DbSet<Centre> Centres { get; set; } = null!;

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
        /// Configures the model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StreamConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new UserActivationTokenConfiguration());

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

            modelBuilder.Entity<Centre>(e =>
            {
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.CenterType).HasConversion<int>();

                e.HasOne(x => x.Country)
                 .WithMany(x => x.Centres)
                 .HasForeignKey(x => x.CountryId);
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
            .HasForeignKey(fp => fp.CourseId);

            // Ensure email is globally unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ensure username is globally unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
