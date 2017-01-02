using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceJobNumber { get; set; }
                
        public string CustomerName { get; set; }

        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Job Number")]
        public string JobNumber { get; set; }
        
    }
}