using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Project;
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

        public ActionResult Edit(int id)
        {
            Project project = GetProject(id);
            return View(project);
        }

        private Project GetProject(int id)
        {
            Project project = Mapper.Map<Project>(_projectManager.GetProject(id));
            project.JobYears = _projectManager.GetJobYears()
                .Select(i => Mapper.Map<Models.Project.JobYear>(i))
                .ToList();
            project.ProjectTypes = _projectManager.GetProjectTypes()
                .Select(i => Mapper.Map<Models.Project.ProjectType>(i))
                .ToList();
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


    }
}