using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class Department
    {
        public int Id { get; set; }

        [DisplayName("Наименование отдела")]
        public string Name { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Описание отдела")]
        public string Description { get; set; }

        public List<Group> Groups { get; set; }
    }
}
