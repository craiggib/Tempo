using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class ProjectTimeBreakdown
    {
        public List<string> WorkTypes { get; set; }
        public List<decimal> Hours { get; set; }
    }
}