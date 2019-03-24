using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Client;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.Quotes;
using TEMPO.WebApp.Models.Quote;

namespace TEMPO.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if (string.IsNullOrEmpty(quoteVm.ClientName))
            {

                _quoteManager.Update(
                    quoteVm.QuoteId,
                    quoteVm.ClientId.Value,
                    quoteVm.Description,
                    quoteVm.EstimatedHours,
                    quoteVm.EstimatedPrice,
                    quoteVm.Tags);
            }
            else
            {
                _quoteManager.Update(
                    quoteVm.QuoteId,
                    quoteVm.ClientName,
                    quoteVm.Description,
                    quoteVm.EstimatedHours,
                    quoteVm.EstimatedPrice,
                    quoteVm.Tags);
                }

            ViewBag.SuccessMessage = "Updated Successfully";
            return View(GetQuote(quoteVm.QuoteId));
        }

        [HttpPost]
        public ActionResult Awarded(int id)
        {
            _quoteManager.AwardQuote(id);
            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public ActionResult UnAward(int id)
        {
            _quoteManager.UnAwardQuote(id);
            return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult Edit(int id)
        {
            Quote quote = GetQuote(id);
            return View(quote);
        }


        private Quote GetQuote(int quoteId)
        {
            Model.Quote quote = _quoteManager.GetQuote(quoteId);
            if (quote != null)
            {
                var quoteVm = Mapper.Map<Quote>(quote);

                ClientManager clientManager = new ClientManager();
                quoteVm.Clients = Mapper.Map<List<Models.Client.Client>>(clientManager.GetClients())
                    .OrderBy(i => i.ClientName)
                    .ToList();

                if (quoteVm.Awarded)
                {
                    var pManager = new ProjectManager();
                    Model.Project associatedProject = pManager.FindAwardedProject(quoteId);
                    if (associatedProject != null)
                    {
                        quoteVm.AssociatedProject = Mapper.Map<Models.Project.Project>(associatedProject);
                    }
                    else
                    {
                        quoteVm.PossibleProjects = Mapper.Map<List<Models.Project.Project>>(pManager.GetProjects(active: true, hasQuote: false))
                            .OrderByDescending(i => i.ProjectName)
                            .ToList();
                    }
                }

                return quoteVm;
            }
            return null;
        }

        [HttpPost]
        public void Delete(int id)
        {
            _quoteManager.Delete(id);
        }

        [HttpPost]
        public ActionResult AssociateProject(Quote quoteVm)
        {
            var pManager = new ProjectManager();
            pManager.AssociateQuote(quoteVm.AssociateToProject.Value, quoteVm.QuoteId);
            return RedirectToAction("Edit", new { id = quoteVm.QuoteId });
        }

        [HttpPost]
        public ActionResult RemoveProjectAssociation(Quote quoteVm)
        {
            var pManager = new ProjectManager();
            pManager.RemoveQuoteAssociation(quoteVm.AssociatedProject.ProjectId);
            return RedirectToAction("Edit", new { id = quoteVm.QuoteId });
        }

        public ActionResult TagSearch(string searchTerm)
        {
            QuoteTagSearch tagSearch = new QuoteTagSearch();
            tagSearch.SearchResults = Mapper.Map<List<Models.Quote.Quote>>(_quoteManager.FindQuotesByTag(searchTerm))
                .OrderByDescending(i => i.LastUpdatedDate)
                .ToList();
            tagSearch.SearchTerm = searchTerm;

            return View(tagSearch);
        }

    }
}