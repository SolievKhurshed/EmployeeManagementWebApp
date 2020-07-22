using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWebApp.ViewModels
{
    public class AddDepartmentViewModel
    {
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Заполните наименование")]
        public string Name { get; set; }

        [DisplayName("Электронный адерс")]
        public string Email { get; set; }

        [DisplayName("Описание отдела")]
        public string Description { get; set; }
    }
}
