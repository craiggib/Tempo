using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Quote
{
    public class QuoteHome
    {
        public List<Quote> Quotes { get; set; }

        public List<Models.Client.Client> Clients { get; set; }
    }
}