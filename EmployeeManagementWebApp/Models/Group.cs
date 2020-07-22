using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public Department Department { get; set; }

        [DisplayName("Наименование группы")]
        public string Name { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Описание группы")]
        public string Description { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
