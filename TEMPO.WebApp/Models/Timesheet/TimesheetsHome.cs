using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class TimesheetsHome
    {
        public IEnumerable<Timesheet> SavedTimeSheets { get; set; }
        public IEnumerable<Timesheet> RejectedTimeSheets { get; set; }
        public SelectList NewTimesheets { get; set; }

        public IEnumerable<Timesheet> ApprovedCurrentMonth { get; set; }
        public IEnumerable<Timesheet> ApprovedBackMonth1 { get; set; }
        public IEnumerable<Timesheet> ApprovedBackMonth2 { get; set; }

        public int NewPeriodEndingId { get; set; }
        
        
    }
}