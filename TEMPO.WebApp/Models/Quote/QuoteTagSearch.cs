using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Quote
{
    public class QuoteTagSearch
    {
        public List<Quote> SearchResults { get; set; }
        public string SearchTerm { get; set; }
    }
}