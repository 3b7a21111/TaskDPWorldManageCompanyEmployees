using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs
{
    public class EmployeeProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public int Age { get; set; }
        public string? SignaturePath { get; set; }

        public List<AttendanceRecordDTO> WeeklyAttendance { get; set; }
    }

}
