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
        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        public float Sunday { get; set; }

        [Range(0, 24)]
        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        public float Monday { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        [Range(0, 24)]
        public float Tuesday { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        [Range(0, 24)]
        public float Wednesday { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        [Range(0, 24)]
        public float Thursday { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        [Range(0, 24)]
        public float Friday { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.#}", ApplyFormatInEditMode = false)]
        [Range(0, 24)]
        public float Saturday { get; set; }

        public int WorkTypeId { get; set; }

        public int ClientId { get; set; }

        public int ProjectId { get; set; }

        public SelectList Projects { get; set; }
        public SelectList WorkTypes { get; set; }
        public string ProjectName { get; set; }
        public string WorkTypeName { get; set; }
    }
}