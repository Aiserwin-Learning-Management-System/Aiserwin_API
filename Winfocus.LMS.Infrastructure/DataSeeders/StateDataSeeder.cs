namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Microsoft.EntityFrameworkCore;
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
            Guid onlineOnlyId = Guid.Parse("99999999-9999-9999-9999-999999999999");

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

                            if (!db.ModeOfStudies.Any())
                            {
                                var onlineMode = new ModeOfStudy
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Online",
                                    CountryId = country.Id,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = Guid.Empty,
                                };
                                onlineOnlyId = onlineMode.Id;
                                db.ModeOfStudies.Add(onlineMode);
                                db.SaveChanges();
                            }

                            var state = new State
                            {
                                Id = Guid.NewGuid(),
                                Name = stateDto.Name,
                                CountryId = country.Id,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = Guid.Empty,
                                IsActive = true,
                                ModeOfStudyId = onlineOnlyId,
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
