using EmployeeManagementWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.ViewModels
{
    public class EditGroupViewModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Заполните наименование")]
        public string Name { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Описание группы")]
        public string Description { get; set; }

        [DisplayName("Отдел")]
        [Required(ErrorMessage = "Выберите отдел")]
        public string DepartmentValue { get; set; }
        public Department Department { get; set; }
    }
}
