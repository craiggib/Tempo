using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Admin
{
    public class AdminHome
    {
        public IEnumerable<Models.Timesheet.Timesheet> SubmittedTimesheets { get; set; }
    }
}