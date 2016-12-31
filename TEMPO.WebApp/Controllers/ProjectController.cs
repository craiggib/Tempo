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
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var projectManager = new ProjectManager();
            Project project = Mapper.Map<Project>(projectManager.GetProject(id));
            return View(project);
        }
    }
}