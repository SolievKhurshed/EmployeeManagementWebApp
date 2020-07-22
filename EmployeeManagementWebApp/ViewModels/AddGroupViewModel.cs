using EmployeeManagementWebApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWebApp.ViewModels
{
    public class AddGroupViewModel
    {
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
