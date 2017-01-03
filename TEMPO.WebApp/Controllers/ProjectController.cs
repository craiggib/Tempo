using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.WebApp.Models.Project;

namespace TEMPO.WebApp.Controllers
{
    public class ProjectController : BaseController
    {
        private ProjectManager _projectManager;

        public ProjectController()
        {
            _projectManager = new ProjectManager();
        }


        public ActionResult Index()
        {
            return View();
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
                .GroupBy(i => i.worktypename);
            
            timeBreakdown.WorkTypes = groupedByWorkType.Select(i => i.Key).ToList();
            timeBreakdown.Hours = groupedByWorkType.Select(i => i.Sum(j => j.entryHours.Value)).ToList();

            return Json(timeBreakdown, JsonRequestBehavior.AllowGet);
        }

    }
}