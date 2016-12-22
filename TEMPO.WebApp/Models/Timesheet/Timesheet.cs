using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class Timesheet
    {
        public string StatusName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime PeriodEnding { get; set; }
    }
}