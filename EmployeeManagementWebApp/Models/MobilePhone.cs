using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class MobilePhone
    {
        public int Id { get; set; }

        public Employee Employee { get; set; }

        [DisplayName("Номер мобильного телефона")]
        public string Number { get; set; }
    }
}
