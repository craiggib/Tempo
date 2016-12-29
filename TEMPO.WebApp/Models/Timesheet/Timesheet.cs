
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class Timesheet
    {
        public int TimesheetId { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Week Ending")]
        public DateTime PeriodEnding { get; set; }

        public List<TimeEntry> TimeEntries { get; set; }

        public float WeeklyTotal { get; set; }

        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
     
    }
}