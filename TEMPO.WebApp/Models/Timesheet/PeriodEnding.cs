using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class PeriodEnding
    {
        public int PeriodEndingId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Week Ending")]
        public DateTime EndingDate { get; set; }
    }
}