namespace Winfocus.LMS.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Domain.Entities;

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
        public DbSet<Student> students { get; set; } = null!;

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
        /// Configures the model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Country configuration
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Country>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Centre configuration
            modelBuilder.Entity<Centre>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Centre>()
                .HasOne(c => c.Country)
                .WithMany(c => c.Centres)
                .HasForeignKey(c => c.CountryId);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            base.OnModelCreating(modelBuilder);
        }
    }
}
