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

    }
}
