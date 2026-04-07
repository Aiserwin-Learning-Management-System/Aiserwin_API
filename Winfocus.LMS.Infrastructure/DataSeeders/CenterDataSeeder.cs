namespace Winfocus.LMS.Infrastructure.DataSeeders
{
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Seeds a default Online centre linked to India so that admin users
    /// can be linked to a centre at startup.
    /// Must run <b>after</b> <see cref="CountryDataSeeder"/>
    /// and <see cref="StateDataSeeder"/> (which creates ModeOfStudy).
    /// </summary>
    public static class CenterDataSeeder
    {
        /// <summary>
        /// Seeds one default Online centre for India.
        /// Skips entirely when centres already exist.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public static void Seed(AppDbContext db)
        {
            if (db.Centres.Any())
            {
                return;
            }

            var uae = db.Countries
                .FirstOrDefault(c => c.Name.ToLower() == "united arab emirates");

            if (uae == null)
            {
                return;
            }

            var onlineMode = db.ModeOfStudies
                .FirstOrDefault(m =>
                    (m.Name.ToLower() == "online" || m.Name.ToLower() == uae.Name.ToLower() + " online") &&
                    m.CountryId == uae.Id);

            if (onlineMode == null)
            {
                onlineMode = db.ModeOfStudies
                    .FirstOrDefault(m => m.Name.ToLower() == "online");

                if (onlineMode == null)
                {
                    return;
                }
            }

            var rakState = db.States
                .FirstOrDefault(s => s.Name.ToLower() == "ras al khaimah" && s.CountryId == uae.Id);

            var center = new Center
            {
                Id = Guid.NewGuid(),
                Name = "Ras Al Khaimah Online Center",
                CenterType = CentreType.Online,
                CenterCode = "UAE-RAK-002",
                ModeOfStudyId = onlineMode.Id,
                CountryId = uae.Id,
                StateId = rakState?.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsActive = true,
            };

            db.Centres.Add(center);
            db.SaveChanges();
        }
    }
}
