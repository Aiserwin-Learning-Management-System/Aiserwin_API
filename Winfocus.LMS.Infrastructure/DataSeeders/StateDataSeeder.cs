namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using System.Text.Json;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides methods to seed state/emirate data into the database.
    /// </summary>
    public static class StateDataSeeder
    {
        /// <summary>
        /// Seeds the specified database.
        /// </summary>
        /// <param name="db">The database.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.States.Any())
            {
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeederFile", "states.json");

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);

                    var statesFromJson = JsonSerializer.Deserialize<List<StateSeedDto>>(json);

                    if (statesFromJson != null)
                    {
                        foreach (var stateDto in statesFromJson)
                        {
                            var country = db.Countries.FirstOrDefault(c => c.Name == stateDto.CountryName);
                            if (country == null)
                            {
                                continue;
                            }

                            var state = new State
                            {
                                Id = Guid.NewGuid(),
                                Name = stateDto.Name,
                                CountryId = country.Id,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = Guid.Empty,
                                IsActive = true,
                            };

                            db.States.Add(state);
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        private class StateSeedDto
        {
            public string Name { get; set; } = null!;
            public string CountryName { get; set; } = null!;
        }
    }
}
