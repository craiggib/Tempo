using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPO.Data
{
    public class ClientProjectSummary
    {
        public ClientSummary ClientSummary { get; set; }
        public List<Project> ProjectList { get; set; }

        public List<TimeEntrySummary> TimeEntrySummaries { get; set; }
    }
}
