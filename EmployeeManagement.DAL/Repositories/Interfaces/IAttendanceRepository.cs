using EmployeeManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        // this method for Displaying a specific employee's attendance .
        Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId);
        // this method make it to Block duplicate check-in
        Task<Attendance?> GetTodayAttendanceAsync(int employeeId, DateTime date);
        Task AddAsync(Attendance attendance);
        // i do not make update method because check-in once per day only غالبا مش هنحتاج نعمل عحيشفث  
        void Delete (Attendance attendance);
        // return list attendance اللي عامليين check-in
        Task<IEnumerable<Attendance>> GetAllCheckedInTodayAsync(DateTime date);
        //Total working hours per week per employee. 
        Task<double> GetWeeklyWorkingHoursAsync(int employeeId, DateTime startOfWeek, DateTime endOfWeek);
        //عرض سجل الحضور السابق للموظف
        Task<IEnumerable<Attendance>> GetEmployeeAttendanceHistoryAsync(int employeeId);

        Task<bool> SaveChangesAsync();
    }
}
