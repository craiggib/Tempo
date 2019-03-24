using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Model;

namespace TEMPO.BusinessLayer.Project
{
    public class ProjectManager : BaseManager
    {
        public List<Model.Project> GetProjects(bool? active = null, bool? hasQuote = null)
        {            
            IQueryable<Model.Project> allProjects = DataContext.Projects;
            if (active.HasValue)
            {
                allProjects = allProjects.Where(i=>i.Active == active.Value);
            }         
            if(hasQuote.HasValue)                
            {
                if (hasQuote.Value)
                {
                    allProjects = allProjects.Where(i => i.quoteid.HasValue);
                }
                else
                {
                    allProjects = allProjects.Where(i => !i.quoteid.HasValue);
                }
            }

            return allProjects.ToList();
            
        }

        public List<Model.Project> GetProjects(int clientId)
        {
            return DataContext.Projects.Where(i => i.clientid == clientId).ToList();
        }

        public List<Model.ProjectSummary> GetProjectSummaries(int clientId)
        {
            return DataContext.ProjectSummaries.Where(i => i.clientid == clientId).ToList();
        }

        public List<Model.ProjectSummary> GetProjectSummaries(bool active, DateTime lastHoursSince)
        {
            return DataContext.ProjectSummaries
                .Where(i => i.active == active && i.lastHoursLogged.HasValue && i.lastHoursLogged > lastHoursSince)
                .ToList();
        }

        public Model.Project GetProject(int projectId)
        {
            return DataContext.Projects.Where(i => i.projectid == projectId).FirstOrDefault();
        }

        public List<Model.JobYear> GetJobYears()
        {
            return DataContext.JobYears.ToList();
        }

        public List<Model.ProjectType> GetProjectTypes()
        {
            return DataContext.ProjectTypes.ToList();
        }

        public Model.Project Create(int clientId, int jobYearId, string projectNumber, string refNumber, int typeId, string description, decimal? amount)
        {
            var newProject = new Model.Project
            {
                clientid = clientId,
                Active = true,
                description = description,
                jobnum = projectNumber,
                jobnumyear = jobYearId,
                refjobnum = refNumber,
                projecttypeid = typeId,
                contractamount = amount
            };
            DataContext.Projects.Add(newProject);
            DataContext.SaveChanges();
            return newProject;
        }

        public void Update(int projectId, int jobYearId, string projectNumber, string refNumber, int typeId, string description, decimal? amount, bool active, int? weight, int? drawingCount)
        {
            Model.Project project = GetProject(projectId);
            if(project != null)
            {
                project.jobnumyear = jobYearId;
                project.jobnum = projectNumber;
                project.Active = active;
                project.description = description;
                project.refjobnum = refNumber;
                project.projecttypeid = typeId;
                project.contractamount = amount;
                project.Weight = weight;
                project.DrawingCount = drawingCount;

                DataContext.SaveChanges();
            }
        }

        public Model.Project FindAwardedProject(int quoteId)
        {
            return DataContext.Projects.Where(i => i.quoteid == quoteId).FirstOrDefault();
        }

        public void AssociateQuote(int projectId, int quoteId)
        {
            Model.Project project = GetProject(projectId);
            if (project != null)
            {
                project.quoteid = quoteId;
                DataContext.SaveChanges();
            }
        }

        public void RemoveQuoteAssociation(int projectId)
        {
            Model.Project project = GetProject(projectId);
            if (project != null)
            {
                project.quoteid = null;
                DataContext.SaveChanges();
            }
        }

        public void Delete(int projectId)
        {
            Model.Project project = GetProject(projectId);
            if (project != null)
            {
                DataContext.Projects.Remove(project);
                DataContext.SaveChanges();
            }
        }

    }
}
