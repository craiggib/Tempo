using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
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
    }
}