using EmployeeManagement.BLL.Services.Implementations;
using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Interfaces.Services;
using EmployeeManagement.Core.Models;
using EmployeeManagement.DAL.Repositories.Implementations;
using EmployeeManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeService employeeService,
                                  IUserRepository userRepository,
                                  IEmployeeRepository employeeRepository)
        {
            _employeeService = employeeService;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetEmployees(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sortBy = "Name",
    [FromQuery] string filter = "")
        {
            var query =  _employeeRepository.GetAll();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(e => e.FirstName.Contains(filter)
                    || e.LastName.Contains(filter)
                    || e.NationalId.Contains(filter));
            }

            // Apply sorting
            switch (sortBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(e => e.FirstName);
                    break;
                case "age":
                    query = query.OrderBy(e => e.Age);
                    break;
                case "nationalid":
                    query = query.OrderBy(e => e.NationalId);
                    break;
                default:
                    query = query.OrderBy(e => e.FirstName);
                    break;
            }

            var totalEmployees = await query.CountAsync();

            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    PhoneNumber = e.PhoneNumber,
                    NationalId = e.NationalId,
                    Age = e.Age,
                    SignaturePath = e.SignaturePath,
                    Username = e.User.Username,
                    Role = e.User.Role
                })
                .ToListAsync();

            return Ok(new { employees, totalEmployees });
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);

                if (employee == null)
                    return NotFound($"Employee with ID {id} not found.");

                var employeeDto = new EmployeeDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    NationalId = employee.NationalId,
                    Age = employee.Age,
                    SignaturePath = employee.SignaturePath,
                    Username = employee.User.Username,
                    Role = employee.User.Role
                };

                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateDTO employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //  Check for existing username [dont repeate username]
            var existingUser = await _userRepository.GetByUsernameAsync(employeeDto.Username);
            if (existingUser != null)
                return BadRequest("Username already exists.");

            var user = new User
            {
                Username = employeeDto.Username,
                Role = employeeDto.Role
            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, employeeDto.Password);

            var userCreated = await _userRepository.AddAsync(user);
            if (!userCreated)
                return StatusCode(500, "Failed to create user.");

            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                PhoneNumber = employeeDto.PhoneNumber,
                NationalId = employeeDto.NationalId,
                Age = employeeDto.Age,
                SignaturePath = employeeDto.Signature,
                UserId = user.Id
            };

            var employeeCreated = await _employeeService.CreateAsync(employee);
            if (!employeeCreated)
                return StatusCode(500, "Failed to create employee.");

            return Ok("Employee created successfully.");

        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDTO dto)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound("Employee not found");

            // Update Employee fields
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.NationalId = dto.NationalId;
            employee.Age = dto.Age;
            employee.SignaturePath = dto.Signature;

            // Update related User fields
            if (employee.User != null)
            {
                employee.User.Username = dto.Username;
                employee.User.Role = dto.Role;
            }

            var result = await _employeeService.UpdateAsync(employee);

            if (!result)
                return StatusCode(500, "An error occurred while updating the employee");

            return Ok("Employee updated successfully");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var result = await _employeeService.DeleteAsync(id);
            if (result)
            {
                return Ok($"Employee with ID {id} deleted successfully");
            }

            return BadRequest($"Unable to delete employee with ID {id}. Please try again later.");
        }

        [HttpGet("Profile")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Unauthorized access");
            int employeeId = int.Parse(userIdClaim.Value);

            var profile = await _employeeService.GetEmployeeProfileAsync(employeeId);
            if (profile == null) return NotFound("Employee not found");

            return Ok(profile);
        }

        [HttpPost("UploadSignature")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UploadSignature([FromBody] SignatureDTO signatureDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Unauthorized access");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _employeeService.UpdateSignatureAsync(userId, signatureDTO.Signature);

            if (!result)
            {
                return BadRequest("Signature already added or employee not found.");
            }

            return Ok("Signature uploaded successfully.");
        }

    }
}
