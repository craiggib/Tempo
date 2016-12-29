using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMPO.WebApp.Models.Timesheet
{
    public class TimeEntry
    {
        public int EntryId { get; set; }
        [Range(0, 24)]        
        public float Sunday { get; set; }
        [Range(0, 24)]
        public float Monday { get; set; }
        [Range(0, 24)]
        public float Tuesday { get; set; }
        [Range(0, 24)]
        public float Wednesday { get; set; }
        [Range(0, 24)]
        public float Thursday { get; set; }
        [Range(0, 24)]
        public float Friday { get; set; }
        [Range(0, 24)]
        public float Saturday { get; set; }
        public int WorkTypeId { get; set; }
        public int ClientId { get; set; }        
        public int ProjectId { get; set; }

        public SelectList Projects { get; set; }
        public SelectList WorkTypes { get; set; }
    }
}