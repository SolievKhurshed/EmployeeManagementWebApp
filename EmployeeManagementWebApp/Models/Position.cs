using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class Position
    {
        public int Id { get; set; }

        [DisplayName("Должность")]
        public string Name { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }
    }
}
