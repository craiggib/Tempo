using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.WebApp.Models.Client;

namespace TEMPO.WebApp.Controllers
{
    public class ClientController : BaseController
    {
        private ClientManager _clientManager;

        public ClientController()
        {
            _clientManager = new ClientManager();
        }
        public ActionResult Index()
        {
            var clientList = _clientManager.GetClients()
                .Select(i => Mapper.Map<Models.Client.Client>(i))
                .OrderBy(i=> i.ClientName)
                .ToList();
            return View(clientList);
        }

        public ActionResult Details(int id)
        {
            var projectManager = new ProjectManager();
            var timesheetManager = new TimesheetManager();

            ClientDetails cDetails = new ClientDetails();
            cDetails.Client = Mapper.Map<Models.Client.Client>(_clientManager.GetClient(id));
            cDetails.ProjectList = projectManager.GetProjects(id)
                .Select(i => Mapper.Map<Models.Project.Project>(i));

            cDetails.TotalHours = 0;                        
            foreach (var project in cDetails.ProjectList)
            {
                var timeEntries = timesheetManager.GetTimeEntries(project.ProjectId);
                cDetails.TotalHours += (float)timeEntries.Sum(i => i.sunday + i.monday + i.tuesday + i.wednesday + i.thursday + i.friday + i.saturday);
            }
            
            return View(cDetails);
        }
    }
}