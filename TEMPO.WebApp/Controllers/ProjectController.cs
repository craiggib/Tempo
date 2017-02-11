using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.WebApp.Models.Project;

namespace TEMPO.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectController : BaseController
    {
        private ProjectManager _projectManager;

        public ProjectController()
        {
            _projectManager = new ProjectManager();
        }

        public ActionResult Index(string sort)
        {
            ProjectHome projectHome = new ProjectHome();

            var projectList = _projectManager.GetProjectSummaries(true)
                .Select(i => Mapper.Map<ProjectSummary>(i));

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "hours")
                {
                    projectList = projectList.OrderByDescending(i => i.TotalHours);

                }
                else if (sort == "jobnum")
                {
                    projectList = projectList.OrderByDescending(i => i.JobYear).ThenByDescending(i=>i.JobNumber);
                }
                else
                {
                    projectList = projectList.OrderByDescending(i => i.LastHoursLogged);
                }
            }
            else
            {
                projectList = projectList.OrderByDescending(i => i.LastHoursLogged);
            }

            projectHome.ProjectSummaries = projectList.ToList();

            projectHome.JobYears = _projectManager.GetJobYears()
                .Select(i => Mapper.Map<Models.Project.JobYear>(i))
                .ToList();
            projectHome.CurrentJobYearId = projectHome.JobYears.FirstOrDefault(i => i.Year == DateTime.Today.Year)?.JobYearId;

            projectHome.ProjectTypes = _projectManager.GetProjectTypes()
                .Select(i => Mapper.Map<Models.Project.ProjectType>(i))
                .ToList();

            projectHome.Clients = new ClientManager().GetClients()
                .Select(i => Mapper.Map<Models.Client.Client>(i))
                .OrderBy(i=>i.ClientName)
                .ToList();

            return View(projectHome);
        }

        public ActionResult Edit(int id, DateTime? start, DateTime? end)
        {
            Project project = GetProject(id, start, end);
            return View(project);
        }
        
        [HttpPost]
        public ActionResult Edit(Project projectVm)
        {
            _projectManager.Update(
                projectVm.ProjectId,
                projectVm.JobYearId,
                projectVm.ProjectNumber,
                projectVm.ReferenceJobNumber,
                projectVm.ProjectTypeId,
                projectVm.Description,
                projectVm.ContractedAmount,
                projectVm.Active,
                projectVm.Weight,
                projectVm.DrawingCount);

            Project project = GetProject(projectVm.ProjectId);
            return View(project);
        }

        private Project GetProject(int id, DateTime? start = null, DateTime? end = null)
        {
            Project project = Mapper.Map<Project>(_projectManager.GetProject(id));
            project.JobYears = _projectManager.GetJobYears()
                .Select(i => Mapper.Map<Models.Project.JobYear>(i))
                .ToList();
            project.ProjectTypes = _projectManager.GetProjectTypes()
                .Select(i => Mapper.Map<Models.Project.ProjectType>(i))
                .ToList();

            var tsManager = new TimesheetManager();

            project.TimeEntrySummaries = tsManager.GetTimeEntrySummaries(id, start, end)
                .Select(i => Mapper.Map<Models.Timesheet.TimeEntrySummary>(i))
                .OrderByDescending(i => i.EndingDate)
                .ToList();

            if (project.TimeEntrySummaries != null)
            {
                project.InternalHours = project.TimeEntrySummaries.Sum(j => j.EntryHours);
                project.InternalAmount = project.TimeEntrySummaries.Sum(j => j.InternalAmount);
                if (project.ContractedAmount.HasValue)
                {
                    project.InternalDifference = project.ContractedAmount.Value - project.InternalAmount;
                    project.InternalDifferenceRatio = (float)(project.InternalAmount / project.ContractedAmount.Value);
                }

            }
            else
            {
                project.InternalHours = 0;
                project.InternalAmount = 0;
            }

            return project;
        }

        [HttpPost]
        public ActionResult Create(Project projectVm)
        {
            var newProject = _projectManager.Create(
                projectVm.ClientId,
                projectVm.JobYearId,
                projectVm.ProjectNumber,
                projectVm.ReferenceJobNumber,
                projectVm.ProjectTypeId,
                projectVm.Description,
                projectVm.ContractedAmount);

            return RedirectToAction("Edit", new { id = newProject.projectid });
        }

        public JsonResult GetProjectTimebreakdown(int projectId)
        {
            var tsManager = new TimesheetManager();

            ProjectTimeBreakdown timeBreakdown = new ProjectTimeBreakdown();

            var groupedByWorkType = tsManager.GetTimeEntrySummaries(projectId)
                .GroupBy(i => i.worktypeid)
                .Select(i => new
                {
                    Id = i.Key,
                    WorkType = i.First().worktypename,
                    WorkTypeTotal = i.Sum(s => s.entryHours.Value)
                })
                .OrderBy(i => i.Id);

            timeBreakdown.WorkTypes = groupedByWorkType.Select(i => i.WorkType).ToList();
            timeBreakdown.Hours = groupedByWorkType.Select(i => i.WorkTypeTotal).ToList();

            return Json(timeBreakdown, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllProjects()
        {
            var searchResults = _projectManager.GetProjects()
                .Select(i => Mapper.Map<Project>(i));
            return Json(searchResults, JsonRequestBehavior.AllowGet);
        }
        
    }
}