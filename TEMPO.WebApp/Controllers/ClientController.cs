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
        public ActionResult Index(string sort)
        {
            var clientList = _clientManager.GetClientSummary()
                .Select(i => Mapper.Map<Models.Client.ClientSummary>(i));                         

            if (!string.IsNullOrEmpty(sort))
            {
                if(sort == "hours")
                {
                    clientList = clientList.OrderByDescending(i => i.TotalHoursLogged);
                    
                }
                else if(sort == "alpha")
                {
                    clientList = clientList.OrderBy(i => i.ClientName);
                }
                else
                {
                    clientList = clientList.OrderByDescending(i => i.LastHoursLogged); 
                }
            }
            else
            {
                clientList = clientList.OrderByDescending(i => i.LastHoursLogged);
            }
                
                
            return View(clientList.ToList());
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

        public ActionResult Edit(int id)
        {
            var client = Mapper.Map<Models.Client.Client>(_clientManager.GetClient(id));
            client.ProjectList = new ProjectManager().GetProjectSummaries(id)
                .Select(i=> Mapper.Map<Models.Project.ProjectSummary>(i))
                .OrderByDescending(i=>i.ProjectName)
                .ToList();

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(Models.Client.Client clientVm)
        {
            _clientManager.UpdateClient(clientVm.ClientId, clientVm.ClientName);
            ViewBag.SuccessMessage = "Updated Successfully";
            return RedirectToAction("Edit", new { id = clientVm.ClientId });
        }

        [HttpPost]
        public ActionResult Create(Models.Client.Client clientVm)
        {
            var newClient = _clientManager.CreateClient(clientVm.ClientName);
            return RedirectToAction("Edit", new { id = newClient.clientid });
        }

        public JsonResult GetAllCustomers()
        {
            var searchResults = _clientManager.GetClients()
                .Select(i => Mapper.Map<Client>(i));
            return Json(searchResults, JsonRequestBehavior.AllowGet);
        }
        
    }
}