using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDTO>> GetAllAsync();
        Task<AttendanceDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceDTO>> GetByEmployeeIdAsync(int employeeId);
        Task<AttendanceDTO?> GetTodayAttendanceAsync(int employeeId);
        // to check employee register in time window [ 7:30  :  9:00 ] 
        Task<bool> CheckInAsync(CreateAttendanceDTO dto);
        //Task<bool> UpdateCheckOutAsync(int id, UpdateAttendanceDTO dto);
        Task<bool> CheckOutAsync(int employeeId);
        //View a daily attendance list of all employees who have checked in. 
        Task<IEnumerable<AttendanceDTO>> GetDailyAttendanceAsync();
        Task<double> GetWeeklyWorkingHoursAsync(int employeeId);
        Task<IEnumerable<AttendanceDTO>> GetCheckInHistoryAsync(int employeeId);

        Task<bool> DeleteAsync(int id);
    }
}
