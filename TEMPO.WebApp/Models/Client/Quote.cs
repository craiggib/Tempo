using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Client
{
    public class Quote
    {
        public int QuoteId { get; set; }
        public int ClientId { get; set; }
        public float Hours { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        
    }
}