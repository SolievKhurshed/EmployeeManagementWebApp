using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class AppLog
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public AppLogType AppLogType { get; set; }
        public string Message { get; set; }
    }
}
