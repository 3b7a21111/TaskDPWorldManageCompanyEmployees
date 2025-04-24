using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs
{
    public class AttendanceRecordDTO
    {
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOut { get; set; }
        public double? WorkingHours { get; set; }
    }
}

