using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public int Age { get; set; }
        public string? SignaturePath { get; set; }

        public string Username { get; set; }
        public string Role { get; set; }
    }
}
