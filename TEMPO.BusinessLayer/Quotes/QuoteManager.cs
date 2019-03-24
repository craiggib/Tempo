using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPO.BusinessLayer.Quotes
{
    public class QuoteManager : BaseManager
    {
        public Model.Quote GetQuote(int quoteId)
        {
            return DataContext.Quotes.Where(i => i.quoteid == quoteId).FirstOrDefault();
        }

        public List<Model.Quote> GetQuotes(int clientId)
        {
            return DataContext.Quotes.Where(i => i.clientid == clientId).ToList();
        }

        public List<Model.Quote> GetQuotes(DateTime fromDate, DateTime toDate)
        {
            return DataContext.Quotes.Where(i => i.createddate >= fromDate && i.createddate <= toDate).ToList();
        }

        public void CreateQuote(int clientId, string description, float estimatedHours, decimal estimatedPrice, string tags, int createdById)
        {
            Model.Quote quote = BuildQuote(description, estimatedHours, estimatedPrice, createdById, DateTime.Now);
            quote.clientid = clientId;
            BuildTags(tags, quote);

            DataContext.Quotes.Add(quote);
            DataContext.SaveChanges();
        }

        public void CreateQuote(string clientName, string description, float estimatedHours, decimal estimatedPrice, string tags, int createdById)
        {
            Model.Quote quote = BuildQuote(description, estimatedHours, estimatedPrice, createdById, DateTime.Now);
            quote.clientname = clientName;
            BuildTags(tags, quote);

            DataContext.Quotes.Add(quote);
            DataContext.SaveChanges();
        }

        private Model.Quote BuildQuote(string description, float estimatedHours, decimal estimatedPrice, int createdById, DateTime lastUpdated)
        {
            return new Model.Quote
            {
                createdby = createdById,
                description = description,
                createddate = lastUpdated,
                hours = estimatedHours,
                lastupdateddate = lastUpdated,
                price = estimatedPrice
            };
        }

        public List<Model.QuoteTagFrequency> GetTagFrequency(int maxResults)
        {
            return DataContext.QuoteTags
                 .GroupBy(i => i.title)
                 .Select(i => new Model.QuoteTagFrequency
                 {
                     FrequencyCount = i.Count(),
                     Tag = i.Key
                 })
                 .OrderByDescending(i => i.FrequencyCount)
                 .Take(maxResults)
                 .ToList();
        }

        private void BuildTags(string tags, Model.Quote quote)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                var tagList = tags.Split(',');

                foreach (string tag in tagList)
                {
                    DataContext.QuoteTags.Add(new Model.QuoteTag
                    {
                        title = tag,
                        quote = quote
                    });
                }
            }
        }

        public void Update(int quoteId, string clientName, string description, float estimatedHours, decimal estimatedPrice, string tags)
        {
            Model.Quote quote = GetQuote(quoteId);
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

            DataContext.QuoteTags.RemoveRange(quote.quotetags);
            BuildTags(tags, quote);

            DataContext.SaveChanges();
        }
        
        public void Update(int quoteId, int clientId, string description, float estimatedHours, decimal estimatedPrice, string tags)
        {
            Model.Quote quote = GetQuote(quoteId);
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

            DataContext.QuoteTags.RemoveRange(quote.quotetags);
            BuildTags(tags, quote);

            DataContext.SaveChanges();
        }
        
        public void Delete(int quoteId)
        {
            Model.Quote quote = GetQuote(quoteId);
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
            Model.Quote quote = GetQuote(quoteId);
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
            Model.Quote quote = GetQuote(quoteId);
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

        public List<Model.Quote> FindQuotesByTag(string tag)
        {
            var matchingQuoteIds = DataContext.QuoteTags.Where(i => i.title.Contains(tag))
                .Select(i => i.quoteid);
            return DataContext.Quotes.Where(i => matchingQuoteIds.Contains(i.quoteid))
                .ToList();
        }
    }


}
