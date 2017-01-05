using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Quotes;

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
            var quotes = _quoteManager.GetQuotes()
                .Select(i => Mapper.Map<Models.Quote.Quote>(i))
                .ToList();
            return View(quotes);
        }
    }
}