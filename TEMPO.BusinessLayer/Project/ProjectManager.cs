using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.Project
{
    public class ProjectManager : BaseManager
    {
        public List<Data.Project> GetProjects()
        {
            return DataContext.Projects.ToList();
        }

        public List<Data.Project> GetProjects(int clientId)
        {
            return DataContext.Projects.Where(i => i.clientid == clientId).ToList();
        }
        public Data.Project GetProject(int projectId)
        {
            return DataContext.Projects.Where(i => i.projectid == projectId).FirstOrDefault();
        }
    }
}
