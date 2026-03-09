using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    /// <summary>
    /// Provides methods to seed staff type data into the database.
    /// </summary>
    public static class StaffTypeDataSeeder
    {
        /// <summary>
        /// Seeds the <see cref="StaffType"/> entities into the database if none exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (!db.StaffCategories.Any())
            {
                // Build path to JSON file
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeederFile", "stafftypes.json");

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);

                    var staffTypesFromJson = JsonSerializer.Deserialize<List<StaffCategory>>(json);

                    if (staffTypesFromJson != null)
                    {
                        foreach (var staffType in staffTypesFromJson)
                        {
                            staffType.Id = Guid.NewGuid();
                            staffType.CreatedAt = DateTime.UtcNow;
                            staffType.IsActive = true;
                            staffType.CreatedBy = Guid.Empty;
                        }

                        db.StaffCategories.AddRange(staffTypesFromJson);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
