using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Quote
{
    public class Quote
    {
        public int QuoteId { get; set; }

        [Display(Name = "Estimated Hours")]
        [Required]
        public float EstimatedHours { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Estimated Price")]
        [Required]        
        [DataType(DataType.Currency)]
        public decimal EstimatedPrice { get; set; }

        [Display(Name = "Created")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Updated")]
        public DateTime LastUpdatedDate { get; set; }
        public int CreateByEmployeeId { get; set; }

        [Display(Name = "Awarded Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}")]
        public DateTime AwardedDate { get; set; }

        [Display(Name = "Created By")]        
        public string CreatedBy { get; set; }
        public bool Awarded { get; set; }
        public int? ClientId { get; set; }

        [Display(Name = "Client Name")]
        [Required]
        public string ClientName { get; set; }
        public string Tags { get; set; }

        public List<Models.Client.Client> Clients { get; set; }

        public List<Models.Project.Project > PossibleProjects { get; set; }

        public Models.Project.Project AssociatedProject { get; set; }
        public int? AssociateToProject { get; set; }
    }
}