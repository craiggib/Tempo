using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
using TEMPO.BusinessLayer.Quotes;
using TEMPO.WebApp.Models.Quote;

namespace TEMPO.WebApp.Controllers
{
    public class QuoteController : BaseController
    {
        private QuoteManager _quoteManager;

        public QuoteController()
        {
            _quoteManager = new QuoteManager();
        }

        public ActionResult Index()
        {
            QuoteHome quoteHome = new QuoteHome();
            quoteHome.Quotes = _quoteManager.GetQuotes()
                .Select(i => Mapper.Map<Models.Quote.Quote>(i))
                .ToList();

            ClientManager clientManager = new ClientManager();
            quoteHome.Clients = Mapper.Map<List<Models.Client.Client>>(clientManager.GetClients())
                .OrderBy(i => i.ClientName)
                .ToList();

            return View(quoteHome);
        }

        [HttpPost]
        public ActionResult Create(Quote quoteVm)
        {
            if (string.IsNullOrEmpty(quoteVm.ClientName))
            {
                _quoteManager.CreateQuote(
                    quoteVm.ClientId.Value,
                    quoteVm.Description,
                    quoteVm.EstimatedHours,
                    quoteVm.EstimatedPrice,
                    quoteVm.Tags,  
                    GetUserID());
            }
            else
            {
                _quoteManager.CreateQuote(
                    quoteVm.ClientName,
                    quoteVm.Description,
                    quoteVm.EstimatedHours,
                    quoteVm.EstimatedPrice,
                    quoteVm.Tags,
                    GetUserID());
            }
            
            return RedirectToAction("Index");
        }
    }
}