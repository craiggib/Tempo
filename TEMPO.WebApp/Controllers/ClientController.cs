using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.Quotes;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.WebApp.Models.Client;

namespace TEMPO.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientController : BaseController
    {
        private ClientManager _clientManager;

        public ClientController()
        {
            _clientManager = new ClientManager();
        }
        public ActionResult Index(string sort)
        {
            var clientList = _clientManager.GetClientSummary(DateTime.Now.AddMonths(-6))
                .Select(i => 
                {
                    ClientSummary clientSummary = Mapper.Map<Models.Client.ClientSummary>(i);                    
                    return clientSummary;
                });                         

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

        public ActionResult Edit(int id, string sort)
        {
            Client client = GetClient(id, ToSortType(sort));
            return View(client);
        }

        private ProjectSortType ToSortType(string sort)
        {
            if(sort == "hours")
            {
                return ProjectSortType.BilledHours;
            }
            else if (sort == "recent")
            {
                return ProjectSortType.Recent;
            }
            else
            {
                return ProjectSortType.Default;
            }
        }

        private Client GetClient(int id, ProjectSortType sortype)
        {
            var client = Mapper.Map<Models.Client.Client>(_clientManager.GetClient(id));
            //client.QuoteList = _clientManager.GetQuotes(id)
            //    .Select(i => Mapper.Map<Quote>(i))
            //    .ToList();

            var projectManager = new ProjectManager();
            client.ProjectList = projectManager.GetProjectSummaries(id)
                .Select(i => Mapper.Map<Models.Project.ProjectSummary>(i))
                .ToList();

            client.JobYears = projectManager.GetJobYears()
                .Select(i => Mapper.Map<Models.Project.JobYear>(i))
                .ToList();
            client.CurrentJobYearId = client.JobYears.FirstOrDefault(i => i.Year == DateTime.Today.Year)?.JobYearId;

            client.ProjectTypes = projectManager.GetProjectTypes()
                .Select(i => Mapper.Map<Models.Project.ProjectType>(i))
                .ToList();

            client.QuoteList = new QuoteManager().GetQuotes(id)
                .Select(i => Mapper.Map<Models.Quote.Quote>(i))
                .OrderByDescending(i=>i.LastUpdatedDate)
                .ToList();

            switch (sortype)
            {
                default:
                case ProjectSortType.Default:
                case ProjectSortType.Recent:
                    client.ProjectList = client.ProjectList.OrderByDescending(i => i.LastHoursLogged).ToList();
                    break;
                case ProjectSortType.BilledHours:
                    client.ProjectList = client.ProjectList.OrderByDescending(i => i.TotalHours).ToList();
                    break;
            }
                
            return client;
        }

        [HttpPost]
        public ActionResult Edit(Models.Client.Client clientVm, string sort)
        {
            _clientManager.UpdateClient(clientVm.ClientId, clientVm.ClientName);                
            ViewBag.SuccessMessage = "Updated Successfully";            
            return View(GetClient(clientVm.ClientId, ToSortType(sort)));
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
        
        private enum ProjectSortType
        {
            Default = 0,
            Recent = 1,
            BilledHours = 2
        }

       
    }
}