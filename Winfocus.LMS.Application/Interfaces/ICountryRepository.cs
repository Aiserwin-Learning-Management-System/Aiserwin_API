using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    public interface ICountryRepository 
    { 
        Task<IEnumerable<Country>> GetAllAsync(); 
        Task<Country?> GetByIdAsync(Guid id); 
        Task<Country> AddAsync(Country country); 
        Task UpdateAsync(Country country); 
        Task DeleteAsync(Guid id); }
}
