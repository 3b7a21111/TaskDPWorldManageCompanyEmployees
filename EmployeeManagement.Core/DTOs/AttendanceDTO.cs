using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
