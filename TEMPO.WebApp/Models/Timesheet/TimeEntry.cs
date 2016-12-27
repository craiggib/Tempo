using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class TimeEntry
    {
        public int EntryId { get; set; }
        public float Sunday { get; set; }
        public float Monday { get; set; }
        public float Tuesday { get; set; }
        public float Wednesday { get; set; }
        public float Thursday { get; set; }
        public float Friday { get; set; }
        public float Saturday { get; set; }
        public int WorkTypeId { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }

        public SelectList Projects { get; set; }
        public float WeeklyTotal { get; set; }
    }
}