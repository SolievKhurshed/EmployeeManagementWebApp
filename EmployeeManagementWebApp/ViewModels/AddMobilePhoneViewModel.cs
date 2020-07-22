using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWebApp.ViewModels
{
    public class AddMobilePhoneViewModel
    {
        public int EmployeeId { get; set; }

        [DisplayName("Номер мобильного телефона")]
        [Required(ErrorMessage = "Заполните номер мобильного телефона")]
        [RegularExpression(@"^(\+[0-9]{11})$", ErrorMessage = "Номер телефона указан не корректно")]
        public string PhoneNumber { get; set; }
    }
}
