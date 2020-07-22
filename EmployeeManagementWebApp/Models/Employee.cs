using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Учетка")]
        public string Login { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Внутренний номер")]
        public int? ExtensionPhone { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Дата устройства")]
        public DateTime? EmploymentDate { get; set; }

        [DisplayName("Статус")]
        public bool IsActive { get; set; }

        public Group Group { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }
        public List<MobilePhone> MobilePhones { get; set; }
    }
}
