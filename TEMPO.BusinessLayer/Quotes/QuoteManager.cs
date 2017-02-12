using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPO.BusinessLayer.Quotes
{
    public class QuoteManager : BaseManager
    {
        public Data.Quote GetQuote(int quoteId)
        {
            return DataContext.Quotes.Where(i => i.quoteid == quoteId).FirstOrDefault();
        }

        public List<Data.Quote> GetQuotes(int clientId)
        {
            return DataContext.Quotes.Where(i => i.clientid == clientId).ToList();
        }

        public List<Data.Quote> GetQuotes(DateTime fromDate, DateTime toDate)
        {
            return DataContext.Quotes.Where(i => i.createddate >= fromDate && i.createddate <= toDate).ToList();
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

        private Data.Quote BuildQuote(string description, float estimatedHours, decimal estimatedPrice, int createdById, DateTime lastUpdated)
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

        public List<Data.QuoteTagFrequency> GetTagFrequency(int maxResults)
        {
            return DataContext.quotetags
                 .GroupBy(i => i.title)
                 .Select(i => new Data.QuoteTagFrequency
                 {
                     FrequencyCount = i.Count(),
                     Tag = i.Key
                 })
                 .OrderByDescending(i => i.FrequencyCount)
                 .Take(maxResults)
                 .ToList();
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

        public void Update(int quoteId, string clientName, string description, float estimatedHours, decimal estimatedPrice, string tags)
        {
            Data.Quote quote = GetQuote(quoteId);
            if (quote == null)
            {
                return;
            }

            quote.clientid = null;
            quote.clientname = clientName;
            quote.description = description;
            quote.hours = estimatedHours;
            quote.price = estimatedPrice;
            quote.lastupdateddate = DateTime.Now;

            DataContext.quotetags.RemoveRange(quote.quotetags);
            BuildTags(tags, quote);

            DataContext.SaveChanges();
        }
        
        public void Update(int quoteId, int clientId, string description, float estimatedHours, decimal estimatedPrice, string tags)
        {
            Data.Quote quote = GetQuote(quoteId);
            if (quote == null)
            {
                return;
            }

            quote.clientname = null;
            quote.clientid = clientId;
            quote.description = description;
            quote.hours = estimatedHours;
            quote.price = estimatedPrice;
            quote.lastupdateddate = DateTime.Now;

            DataContext.quotetags.RemoveRange(quote.quotetags);
            BuildTags(tags, quote);

            DataContext.SaveChanges();
        }
        
        public void Delete(int quoteId)
        {
            Data.Quote quote = GetQuote(quoteId);
            if (quote == null)
            {
                return;
            }

            var associatedProject = DataContext.Projects.Where(i => i.quoteid == quote.quoteid).FirstOrDefault();
            if (associatedProject != null)
            {
                associatedProject.quoteid = null;
            }

            DataContext.Quotes.Remove(quote);
            DataContext.SaveChanges();
        }

        public void AwardQuote(int quoteId)
        {
            Data.Quote quote = GetQuote(quoteId);
            if (quote == null)
            {
                return;
            }
            quote.awarded = true;
            quote.awardedDate = DateTime.Now;
            DataContext.SaveChanges();            
        }

        public void UnAwardQuote(int quoteId)
        {
            Data.Quote quote = GetQuote(quoteId);
            if (quote == null)
            {
                return;
            }
            quote.awarded = false;
            quote.awardedDate = null;

            var associatedProject = DataContext.Projects.Where(i => i.quoteid == quote.quoteid).FirstOrDefault();
            if(associatedProject != null)
            {
                associatedProject.quoteid = null;
            }

            DataContext.SaveChanges();
        }

        public List<Data.Quote> FindQuotesByTag(string tag)
        {
            var matchingQuoteIds = DataContext.quotetags.Where(i => i.title.Contains(tag))
                .Select(i => i.quoteid);
            return DataContext.Quotes.Where(i => matchingQuoteIds.Contains(i.quoteid))
                .ToList();
        }
    }


}
