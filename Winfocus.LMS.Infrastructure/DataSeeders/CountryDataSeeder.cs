namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using System.Text.Json;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides methods to seed country data into the database.
    /// </summary>
    public static class CountryDataSeeder
    {
        /// <summary>
        /// Seeds the <see cref="Country"/> entities into the database if none exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.Countries.Any())
            {
                // Build path to JSON file (make sure it's set to "Copy if newer" in file properties)
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeederFile", "countries.json");

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);

                    var countriesFromJson = JsonSerializer.Deserialize<List<Country>>(json);

                    if (countriesFromJson != null)
                    {
                        foreach (var country in countriesFromJson)
                        {
                            country.Id = Guid.NewGuid();
                            country.CreatedAt = DateTime.UtcNow;
                            country.CreatedBy = Guid.Empty;
                            country.IsActive = true;
                        }

                        db.Countries.AddRange(countriesFromJson);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
