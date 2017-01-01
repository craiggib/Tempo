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

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        public List<Models.Project.ProjectSummary> ProjectList { get; set; }
    }
}