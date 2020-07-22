using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWebApp.ViewModels
{
    public class AddPositionViewModel
    {
        public int EmployeeId { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Заполните наименование")]
        public string Name { get; set; }
    }
}
