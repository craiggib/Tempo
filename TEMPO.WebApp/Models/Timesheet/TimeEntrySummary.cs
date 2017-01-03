using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class TimeEntrySummary
    {
        public int EntryId { get; set; }

        public int ProjectId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]        
        public float EntryHours { get; set; }

        public string WorkTypeName { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal InternalAmount { get; set; }
        public string EmployeeName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Week Ending")]
        public DateTime EndingDate { get; set; }
        public int TimesheetId { get; set; }

    }
}