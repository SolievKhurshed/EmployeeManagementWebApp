using EmployeeManagementWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.ViewModels
{
    public class AddPositionToEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [DisplayName("Должность")]
        [Required(ErrorMessage = "Выберите должность")]
        public string PositionValue { get; set; }
        public Position Position { get; set; }
    }
}
