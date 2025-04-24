using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs
{
    public class AttendanceSummaryDTO
    {
        public string EmployeeFullName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public int DaysPresent { get; set; }
        public int DaysAbsent { get; set; }
        public double TotalWorkingHours { get; set; }
    }

}
