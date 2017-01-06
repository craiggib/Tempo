using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPO.BusinessLayer.Quotes
{
    public class QuoteManager : BaseManager
    {

        public List<Data.Quote> GetQuotes(int clientId)
        {
            return DataContext.Quotes.Where(i => i.clientid == clientId).ToList();
        }

        public List<Data.Quote> GetQuotes()
        {
            return DataContext.Quotes.ToList();
        }

        public void CreateQuote(int clientId, string description, float estimatedHours, decimal estimatedPrice, string tags, int createdById)
        {
            Data.Quote quote = BuildQuote(description, estimatedHours, estimatedPrice, createdById, DateTime.Now);
            quote.clientid = clientId;
            BuildTags(tags, quote);

            DataContext.Quotes.Add(quote);
            DataContext.SaveChanges();
        }

        public void CreateQuote(string clientName, string description, float estimatedHours, decimal estimatedPrice, string tags, int createdById)
        {
            Data.Quote quote = BuildQuote(description, estimatedHours, estimatedPrice, createdById, DateTime.Now);
            quote.clientname = clientName;
            BuildTags(tags, quote);

            DataContext.Quotes.Add(quote);
            DataContext.SaveChanges();
        }

        private static Data.Quote BuildQuote(string description, float estimatedHours, decimal estimatedPrice, int createdById, DateTime lastUpdated)
        {
            return new Data.Quote
            {
                createdby = createdById,
                description = description,
                createddate = lastUpdated,
                hours = estimatedHours,
                lastupdateddate = lastUpdated,
                price = estimatedPrice
            };
        }

        private void BuildTags(string tags, Data.Quote quote)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                var tagList = tags.Split(',');

                foreach (string tag in tagList)
                {
                    DataContext.quotetags.Add(new Data.quotetag
                    {
                        title = tag,
                        quote = quote
                    });
                }
            }
        }
    }

        
}
