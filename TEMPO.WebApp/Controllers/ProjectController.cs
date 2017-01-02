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
            Project project = Mapper.Map<Project>(_projectManager.GetProject(id));
            return View(project);
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
                projectVm.Description);

            return RedirectToAction("Edit", new { id = newProject.projectid });
        }


    }
}