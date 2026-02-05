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
        /// Gets or sets the roles in the database.
        /// </summary>
        public DbSet<Role> Roles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the UserRoles in the database.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; } = null!;


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

            base.OnModelCreating(modelBuilder);
        }
    }
}
