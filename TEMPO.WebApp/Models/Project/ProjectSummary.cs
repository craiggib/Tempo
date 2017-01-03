using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class ProjectSummary
    {
        public int ProjectId { get; set; }        
        public int ClientId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ReferenceJobNumber { get; set; }
        public string ProjectType { get; set; }
        public bool Active { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public float InternalAmount { get; set; }
        public float TotalHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LastHoursLogged { get; set; }
    }
}