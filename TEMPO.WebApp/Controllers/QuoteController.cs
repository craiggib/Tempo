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

        public ActionResult Index(DateTime? start, DateTime? end, string sort)
        {
            QuoteHome quoteHome = new QuoteHome
            {
                QuoteFilterFrom = start ?? DateTime.Today.AddMonths(-3),
                QuoteFilterTo = end ?? DateTime.Today.AddDays(1)
            };

            quoteHome.Quotes = _quoteManager.GetQuotes(quoteHome.QuoteFilterFrom, quoteHome.QuoteFilterTo)
                .Select(i => Mapper.Map<Models.Quote.Quote>(i))
                .OrderByDescending(i => i.QuoteId)
                .ToList();

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "client")
                {
                    quoteHome.Quotes = quoteHome.Quotes.OrderBy(i => i.ClientName).ToList();
                }
                else if (sort == "price")
                {
                    quoteHome.Quotes = quoteHome.Quotes.OrderByDescending(i => i.EstimatedPrice).ToList();
                }
                else if (sort == "awarded")
                {
                    quoteHome.Quotes = quoteHome.Quotes.OrderByDescending(i => i.Awarded)
                        .ThenBy(i => i.QuoteId)
                        .ToList();
                }
                else
                {
                    quoteHome.Quotes = quoteHome.Quotes.OrderByDescending(i => i.QuoteId).ToList();
                }
            }

            quoteHome.TagFrequency = Mapper.Map<List<QuoteTagFrequency>>(_quoteManager.GetTagFrequency(20));

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

        [HttpPost]
        public ActionResult Edit(Quote quoteVm)
        {
            _quoteManager.Update(
                quoteVm.QuoteId,
                quoteVm.Description,
                quoteVm.EstimatedHours,
                quoteVm.EstimatedPrice,
                quoteVm.Tags);

            ViewBag.SuccessMessage = "Updated Successfully";
            return View(GetQuote(quoteVm.QuoteId));
        }

        public ActionResult Edit(int id)
        {            
            return View(GetQuote(id));
        }

        private Quote GetQuote(int quoteId)
        {
            Data.Quote quote = _quoteManager.GetQuote(quoteId);
            if (quote != null)
            {
                var quoteVm = Mapper.Map<Quote>(quote);

                ClientManager clientManager = new ClientManager();
                quoteVm.Clients = Mapper.Map<List<Models.Client.Client>>(clientManager.GetClients())
                    .OrderBy(i => i.ClientName)
                    .ToList();

                return quoteVm;
            }
            return null;
        }
    }
}