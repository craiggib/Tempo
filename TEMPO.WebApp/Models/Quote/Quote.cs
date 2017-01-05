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

        public string Description { get; set; }

        [Display(Name = "Estimated Hours")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal EstimatedPrice { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int CreateByEmployeeId { get; set; }

        [Display(Name = "Created By")]        
        public string CreatedBy { get; set; }
        public bool Awarded { get; set; }
        public int? ClientId { get; set; }

        [Display(Name = "Client Name")]
        [Required]
        public string ClientName { get; set; }
    }
}