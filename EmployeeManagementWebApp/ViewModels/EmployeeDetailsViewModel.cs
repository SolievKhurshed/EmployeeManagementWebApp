using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Отдел")]
        public string Department { get; set; }

        [DisplayName("Группа")]
        public string Group { get; set; }

        [DisplayName("Должность")]
        public string Position { get; set; }

        [DisplayName("Учетка")]
        public string Login { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Внутренний номер")]
        public int? ExtensionPhone { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? BithDate { get; set; }

        [DisplayName("Дата устройства")]
        public DateTime? EmploymentDate { get; set; }

        [DisplayName("Номер мобильного телефона")]
        public string MobilePhone { get; set; }

        [DisplayName("Статус")]
        public bool IsActive { get; set; }
    }
}
