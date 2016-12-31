using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Client
{
    public class ClientDetails
    {
        public Client Client { get; set; }
        public IEnumerable<Project.Project> ProjectList { get; set; }
        public float TotalHours { get; set; }
        public float TotalBilling { get; set; }
    }
}