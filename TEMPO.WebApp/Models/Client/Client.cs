using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Client
{
    public class Client
    {
        public int ClientId { get; set; }
                
        [Display(Name = "Name")]
        [Required]        
        public string ClientName { get; set; }

        public List<Models.Project.ProjectSummary> ProjectList { get; set; }
        public List<Models.Quote.Quote> QuoteList { get; set; }

        public List<Models.Project.JobYear> JobYears { get; set; }
        public int? CurrentJobYearId { get; set; }

        public List<Models.Project.ProjectType> ProjectTypes { get; set; }
    }
}