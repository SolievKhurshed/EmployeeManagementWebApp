using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.ViewModels
{
    public class PositionViewModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Заполните наименование")]
        public string Name { get; set; }
    }
}
