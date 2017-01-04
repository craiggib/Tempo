using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Employee
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        
        [Required]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        public string Password { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Rate { get; set; }

        [Required]
        public bool Active { get; set; }

        public List<Models.Timesheet.Timesheet> Timesheets { get; set; }
    }
}