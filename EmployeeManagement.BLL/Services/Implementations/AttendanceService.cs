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
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository attendanceRepository;
        private readonly IEmployeeRepository employeeRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository,IEmployeeRepository employeeRepository)
        {
            this.attendanceRepository = attendanceRepository;
            this.employeeRepository = employeeRepository;
        }
        public async Task<bool> CheckInAsync(CreateAttendanceDTO dto)
        {
            var existing = await attendanceRepository.GetTodayAttendanceAsync(dto.EmployeeId, DateTime.Now);
            if (existing != null) return false;

            var now = DateTime.Now;
            //var checkStart = new TimeSpan(7, 30, 0);
            //var checkEnd = new TimeSpan(9, 0, 0);
            //if (now.TimeOfDay < checkStart || now.TimeOfDay > checkEnd) 
            //    return false;

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                CheckInTime = now
            };

            await attendanceRepository.AddAsync(attendance);
            return await attendanceRepository.SaveChangesAsync();
        }

        //public async Task<bool> UpdateCheckOutAsync(int id, UpdateAttendanceDTO dto)
        //{
        //    var attendance = await attendanceRepository.GetByIdAsync(id);
        //    if (attendance == null) return false;

        //    attendance.CheckOut = dto.CheckOut ?? DateTime.Now;
        //    return await attendanceRepository.SaveChangesAsync();
        //}
        public async Task<bool> CheckOutAsync(int employeeId)
        {
            var todayAttendance = await attendanceRepository.GetTodayAttendanceAsync(employeeId, DateTime.Now);

            if (todayAttendance == null) return false;

            if (todayAttendance.CheckOut.HasValue) return false;

            var now = DateTime.Now;
            if (now < todayAttendance.CheckInTime) return false;

            todayAttendance.CheckOut = now;

            return await attendanceRepository.SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await attendanceRepository.GetByIdAsync(id);
            if (attendance == null) return false;

            attendanceRepository.Delete(attendance);
            return await attendanceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AttendanceDTO>> GetAllAsync()
        {
            var records = await attendanceRepository.GetAllAsync();
            return records.Select(a => new AttendanceDTO
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeFullName = $"{a.Employee.FirstName} {a.Employee.LastName}",
                CheckInTime = a.CheckInTime,
                CheckOut = a.CheckOut
            });
        }

        public async Task<IEnumerable<AttendanceDTO>> GetByEmployeeIdAsync(int employeeId)
        {
            var records = await attendanceRepository.GetByEmployeeIdAsync(employeeId);
            return records.Select(a => new AttendanceDTO
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeFullName = $"{a.Employee.FirstName} {a.Employee.LastName}",
                CheckInTime = a.CheckInTime,
                CheckOut = a.CheckOut
            });
        }

        public async Task<AttendanceDTO?> GetByIdAsync(int id)
        {
            var record = await attendanceRepository.GetByIdAsync(id);
            if (record == null) return null;

            return new AttendanceDTO
            {
                Id = record.Id,
                EmployeeId = record.EmployeeId,
                EmployeeFullName = $"{record.Employee.FirstName} {record.Employee.LastName}",
                CheckInTime = record.CheckInTime,
                CheckOut = record.CheckOut
            };
        }

        public async Task<AttendanceDTO?> GetTodayAttendanceAsync(int employeeId)
        {
            var record = await attendanceRepository.GetTodayAttendanceAsync(employeeId, DateTime.Now);
            if (record == null) return null;

            return new AttendanceDTO
            {
                Id = record.Id,
                EmployeeId = record.EmployeeId,
                EmployeeFullName = $"{record.Employee.FirstName} {record.Employee.LastName}",
                CheckInTime = record.CheckInTime,
                CheckOut = record.CheckOut
            };
        }
        public async Task<IEnumerable<AttendanceDTO>> GetDailyAttendanceAsync()
        {
            var attendances = await attendanceRepository.GetAllCheckedInTodayAsync(DateTime.Now);
            return attendances.Select(a => new AttendanceDTO
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeFullName = $"{a.Employee.FirstName} {a.Employee.LastName}",
                CheckInTime = a.CheckInTime,
                CheckOut = a.CheckOut
            }).ToList();
        }

        public async Task<double> GetWeeklyWorkingHoursAsync(int employeeId)
        {
            var today = DateTime.Today;
            var diff = (int)today.DayOfWeek;
            var startOfWeek = today.AddDays(-diff); 
            var endOfWeek = startOfWeek.AddDays(6); 

            return await attendanceRepository.GetWeeklyWorkingHoursAsync(employeeId, startOfWeek, endOfWeek);
        }

        public async Task<IEnumerable<AttendanceDTO>> GetCheckInHistoryAsync(int employeeId)
        {
            var attendances = await attendanceRepository.GetByEmployeeIdAsync(employeeId);

            // تحويل البيانات إلى AttendanceDTO لعرضها في API
            var attendanceDTOs = attendances.Select(a => new AttendanceDTO
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeFullName = $"{a.Employee.FirstName} {a.Employee.LastName}",
                CheckInTime = a.CheckInTime,
                CheckOut = a.CheckOut
            }).ToList();

            return attendanceDTOs;
        }



    }
}
