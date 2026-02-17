using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="Student"/> entities.
    /// </summary>
    public sealed class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger<StateService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StudentService(IStudentRepository repository, ILogger<StateService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StateDto.</returns>
        public async Task<IReadOnlyList<StudentDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentDto.</returns>
        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            return student == null ? null : Map(student);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StateDto.</returns>
        /// <exception cref="InvalidOperationException">State code already exists. </exception>
        public async Task<StudentDto> CreateAsync(StudentDto request)
        {
            var student = new Student
            {
                StudentAcademicId = request.StudentAcademicId,
                StudentDocumentsId = request.StudentDocumentsId,
                StudentPersonalId = request.StudentPersonalId,
                CreatedBy = request.Userid,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(student);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Student not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, StudentDto request)
        {
            var student = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Student not found");

            student.StudentAcademicId = request.StudentAcademicId;
            student.StudentDocumentsId = request.StudentDocumentsId;
            student.StudentPersonalId = request.StudentPersonalId;
            student.UpdatedBy = request.Userid;
            student.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(student);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private static StudentDto Map(Student c) =>
     new StudentDto
     {
         Id = c.Id,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         UpdatedAt = c.UpdatedAt,
         UpdatedBy = c.UpdatedBy,
     };
    }
}
