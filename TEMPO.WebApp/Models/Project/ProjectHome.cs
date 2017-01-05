using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class ProjectHome
    {
        public List<ProjectSummary> ProjectSummaries { get; set; }

        public List<Models.Project.JobYear> JobYears { get; set; }
        public int? CurrentJobYearId { get; set; }

        public List<Models.Project.ProjectType> ProjectTypes { get; set; }

        public List<Models.Client.Client> Clients { get; set; }
    }
}