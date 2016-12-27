using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ReferenceJobNumber { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
    }
}