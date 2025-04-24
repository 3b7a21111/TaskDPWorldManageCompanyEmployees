using EmployeeManagement.BLL.Services.Implementations;
using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService attendanceService;
        private readonly IEmployeeService employeeService;

        public AttendanceController(IAttendanceService attendanceService,
                                    IEmployeeService employeeService)
        {
            this.attendanceService = attendanceService;
            this.employeeService = employeeService;
        }

        //end point for check-in
        [Authorize(Roles = "Employee")]
        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(CreateAttendanceDTO dto)
        {
            var result = await attendanceService.CheckInAsync(dto);
            if (!result) return BadRequest("Check-in failed (already checked in or invalid time).");
            return Ok("Checked in successfully.");
        }

        //[Authorize(Roles = "Employee")]
        //[HttpPut("CheckOut/{id}")]
        //public async Task<IActionResult> CheckOut(int id, UpdateAttendanceDTO dto)
        //{
        //    var result = await attendanceService.UpdateCheckOutAsync(id, dto);
        //    if (!result) return NotFound("Attendance record not found.");
        //    return Ok("Checked out successfully.");
        //}

        [HttpPost("CheckOut")]
        [Authorize(Roles = "Employee")] 
        public async Task<IActionResult> CheckOut()
        {
            var userIdClaim =  User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Unauthorized access");

            int UserId =  int.Parse(userIdClaim.Value);
            var employee =  await employeeService.GetEmployeeOfUserIdAsync(UserId);

            if (employee == null) return NotFound("Employee not found");
            
            var result = await attendanceService.CheckOutAsync(employee.Id);
            if (!result) return BadRequest("Check-out failed (may be already checked out or not checked in yet).");

            return Ok("Checked out successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DailyAttendance")]
        public async Task<IActionResult> GetDailyAttendance()
        {
            var dailyAttendance = await attendanceService.GetDailyAttendanceAsync();

            if (dailyAttendance == null || !dailyAttendance.Any())
            {
                return NoContent(); 
            }

            return Ok(dailyAttendance);  
        }

        [HttpGet("WeeklyHours/{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetWeeklyWorkingHours(int employeeId)
        {
            var hours = await attendanceService.GetWeeklyWorkingHoursAsync(employeeId);
            return Ok(new { EmployeeId = employeeId, WeeklyHours = Math.Round(hours, 2) });
        }

        [HttpGet("GetCheckInHistory")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetCheckInHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Unauthorized access");
            int userId = int.Parse(userIdClaim.Value);

            var checkInHistory = await attendanceService.GetCheckInHistoryAsync(userId);

            return Ok(checkInHistory);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await attendanceService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await attendanceService.GetByIdAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        [HttpGet("Employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var records = await attendanceService.GetByEmployeeIdAsync(employeeId);
            return Ok(records);
        }

        [HttpGet("Today/{employeeId}")]
        public async Task<IActionResult> GetTodayAttendance(int employeeId)
        {
            var record = await attendanceService.GetTodayAttendanceAsync(employeeId);
            if (record == null) return NotFound("No check-in today.");
            return Ok(record);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await attendanceService.DeleteAsync(id);
            if (!result) return NotFound("Attendance not found.");
            return Ok("Deleted successfully.");
        }
    }
}
