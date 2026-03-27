namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository to query teachers grouped by staff category.
    /// </summary>
    public class TeachersRepository : ITeachersRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachersRepository"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public TeachersRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="categoryName">The categoryName.</param>
        /// <returns>teachers list.</returns>
        public async Task<List<TeachersByCategoryDto>> GetTeachersByCategoryAsync(string? categoryName = null)
        {
            var query = _db.StaffRegistrations
                .AsNoTracking()
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.Values).ThenInclude(v => v.FormField)
                .Where(sr => sr.Status == RegistrationStatus.Approved);

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                var filter = categoryName.Trim();
                query = query.Where(sr => sr.StaffCategory.Name!
                    .Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            var list = await query.ToListAsync();

            var fullNameFields = new[] { "full_name", "fullname", "name" };
            var firstNameFields = new[] { "first_name", "firstname" };
            var lastNameFields = new[] { "last_name", "lastname" };
            var emailFields = new[] { "email", "email_address" };
            var empIdFields = new[] { "employee_id", "employeeid", "emp_id", "employee" };
            var roleFields = new[] { "role", "designation", "job_title" };
            var dobFields = new[] { "date_of_birth", "dob", "birth_date" };

            var grouped = list
                .GroupBy(sr => sr.StaffCategory?.Name ?? "Uncategorized")
                .Select(g => new TeachersByCategoryDto
                {
                    CategoryName = g.Key,
                    Teachers = g.Select(sr =>
                    {
                        var values = sr.Values?.ToDictionary(
                            v => v.FieldName?.Trim().ToLower() ?? string.Empty,
                            v => v.Value,
                            StringComparer.OrdinalIgnoreCase) ?? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

                        string? Find(params string[] keys)
                        {
                            foreach (var k in keys)
                            {
                                if (values.TryGetValue(k, out var val) && !string.IsNullOrWhiteSpace(val))
                                    return val;
                            }
                            return null;
                        }

                        var fullName = Find(fullNameFields) ??
                                       (Find(firstNameFields) is string f && Find(lastNameFields) is string l ? f + " " + l : null) ??
                                       "Unknown";

                        var email = Find(emailFields);

                        var role = Find(roleFields) ?? sr.StaffCategory?.Name ?? "Staff";
                        var dob = Find(dobFields);

                        return new TeacherListDto
                        {
                            RegistrationId = sr.Id,
                            FullName = fullName,
                            Role = role ?? string.Empty,
                            Email = email,
                            DateOfBirth = dob,
                            JoinedAt = sr.CreatedAt
                        };
                    }).ToList()
                }).ToList();

            return grouped;
        }
    }
}
