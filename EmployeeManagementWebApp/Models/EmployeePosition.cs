using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class EmployeePosition
    {
        //public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
