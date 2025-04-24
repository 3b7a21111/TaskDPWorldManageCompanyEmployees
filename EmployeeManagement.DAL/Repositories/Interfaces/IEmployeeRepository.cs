using EmployeeManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> GetAll();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetEmployeeOfUserIdAsync(int id);
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<bool> SaveChangesAsync();
    }
}
