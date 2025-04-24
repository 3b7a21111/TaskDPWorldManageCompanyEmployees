using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
       IQueryable<Employee> GetAll();
        Task<Employee?> GetByIdAsync (int id);
        Task<Employee?> GetEmployeeOfUserIdAsync(int id);

        Task<bool> CreateAsync (Employee employee);
        Task<bool> UpdateAsync(Employee updatedEmployee); 
        Task<bool> DeleteAsync (int id);
        Task<EmployeeProfileDTO?> GetEmployeeProfileAsync(int employeeId);
        //method of update signture
        Task<bool> UpdateSignatureAsync(int employeeId, string signature);
    }
}
