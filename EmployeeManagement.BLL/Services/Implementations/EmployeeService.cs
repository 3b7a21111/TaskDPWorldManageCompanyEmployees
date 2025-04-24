using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Interfaces.Services;
using EmployeeManagement.Core.Models;
using EmployeeManagement.DAL.Repositories.Implementations;
using EmployeeManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.BLL.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IAttendanceRepository attendanceRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IAttendanceRepository attendanceRepository)
        {
            this.employeeRepository = employeeRepository;
            this.attendanceRepository = attendanceRepository;
        }
        public async Task<bool> CreateAsync(Employee employee)
        {
            await employeeRepository.AddAsync(employee);
            return await employeeRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await employeeRepository.GetByIdAsync(id);
            if (existing == null)  return false;
            employeeRepository.Delete(existing);
            return await employeeRepository.SaveChangesAsync();
        }

        public IQueryable<Employee> GetAll()
        {
            return  employeeRepository.GetAll();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee?> GetEmployeeOfUserIdAsync(int id)
        {
            return await employeeRepository.GetEmployeeOfUserIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Employee updatedEmployee)
        {
            employeeRepository.Update(updatedEmployee);
            return await employeeRepository.SaveChangesAsync();
        }

        public async Task<EmployeeProfileDTO?> GetEmployeeProfileAsync(int employeeId)
        {
            var employee = await employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return null;

            var attendances = await attendanceRepository.GetByEmployeeIdAsync(employeeId);

            var lastWeek = attendances
                .Where(a => a.CheckInTime >= DateTime.Today.AddDays(-7))
                .Select(a => new AttendanceRecordDTO
                {
                    CheckInTime = a.CheckInTime,
                    CheckOut = a.CheckOut,
                    WorkingHours = a.CheckOut.HasValue
                        ? (a.CheckOut.Value - a.CheckInTime).TotalHours
                        : null
                })
                .ToList();

            return new EmployeeProfileDTO
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                NationalId = employee.NationalId,
                Age = employee.Age,
                SignaturePath = employee.SignaturePath,
                WeeklyAttendance = lastWeek
            };
        }

        public async Task<bool> UpdateSignatureAsync(int employeeId, string signature)
        {
            var employee = await employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(employee.SignaturePath))
            {
                return false;
            }

            employee.SignaturePath = signature;
            await employeeRepository.SaveChangesAsync();

            return true;
        }


    }
}
