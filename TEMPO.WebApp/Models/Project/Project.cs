using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class Project
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }

        [Display(Name = "Ref. Number")]
        public string ReferenceJobNumber { get; set; }

        [Display(Name = "Project Number")]
        [StringLength(10)]
        [Required]
        public string ProjectNumber { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Job Number")]
        public string JobNumber { get; set; }
                
        [Display(Name = "Year")]
        [Required]
        public int JobYearId { get; set; }
               
        [Required]
        [Display(Name = "Type")]
        public int ProjectTypeId { get; set; }
                
        [Display(Name = "Contracted Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal? ContractedAmount { get; set; }

        [Display(Name = "Drawings")]
        public int? DrawingCount { get; set; }

        [Display(Name = "Weight (Tonnage)")]
        public int? Weight { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        public List<Models.Project.ProjectType> ProjectTypes { get; set; }

        public List<Models.Project.JobYear> JobYears { get; set; }
        public List<Models.Timesheet.TimeEntrySummary> TimeEntrySummaries { get; set; }

        public float InternalHours { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal InternalAmount { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal InternalDifference { get; set; }
                
        [DisplayFormat(DataFormatString = "{0:p}")]
        public float InternalDifferenceRatio { get; set; }
    }
}