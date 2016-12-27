using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.Project
{
    public class ProjectManager
    {
        public List<Data.Project> GetProjects()
        {
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.Projects.ToList();
        }
    }
}
