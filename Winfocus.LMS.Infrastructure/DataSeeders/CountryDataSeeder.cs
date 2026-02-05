namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides methods to seed country data into the database.
    /// </summary>
    public class CountryDataSeeder
    {
        /// <summary>
        /// Seeds the <see cref="Country"/> entities into the database if none exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.Countries.Any())
            {
                db.Countries.AddRange(
                    new Country { Name = "India", Code = "IN" },
                    new Country { Name = "UAE", Code = "AE" });
                db.SaveChanges();
            }
        }
    }
}
