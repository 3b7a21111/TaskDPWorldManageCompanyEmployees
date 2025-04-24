using EmployeeManagement.Core.Models;
using EmployeeManagement.DAL.Context;
using EmployeeManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext context;

        public AttendanceRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Attendance attendance)
        {
            await context.Attendances.AddAsync(attendance);
        }

        public void Delete(Attendance attendance)
        {
            context.Attendances.Remove(attendance);
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await context.Attendances
                                .Include(x=>x.Employee)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId)
        {
            return await context.Attendances
                                .Where(x => x.EmployeeId == employeeId)
                                .ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await context.Attendances
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Attendance?> GetTodayAttendanceAsync(int employeeId, DateTime date)
        {
            return await context.Attendances
                                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId 
                                && x.CheckInTime.Date==date.Date);
        }   
        public async Task<IEnumerable<Attendance>> GetAllCheckedInTodayAsync(DateTime date)
        {
            return await context.Attendances
                                .Include(a => a.Employee)
                                .Where(a => a.CheckInTime.Date == date.Date && !a.CheckOut.HasValue)  // فقط الحضور اليومي والموظفين الذين لم يسجلوا الخروج بعد
                                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<double> GetWeeklyWorkingHoursAsync(int employeeId,
                                                        DateTime startOfWeek,
                                                       DateTime endOfWeek)
        {
            var attendances = await context.Attendances
                        .Where(a => a.EmployeeId == employeeId
                                    && a.CheckInTime.Date >= startOfWeek.Date
                                    && a.CheckInTime.Date <= endOfWeek.Date
                                    && a.CheckOut != null)
                        .ToListAsync();

            double totalHours = 0;

                foreach (var att in attendances)
                {
                    totalHours += (att.CheckOut.Value - att.CheckInTime).TotalHours;
                }

            return totalHours;
        }
        public async Task<IEnumerable<Attendance>> GetEmployeeAttendanceHistoryAsync(int employeeId)
        {
            return await context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync();
        }

    }
}
