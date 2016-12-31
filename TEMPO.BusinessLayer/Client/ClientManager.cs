using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPO.BusinessLayer.Client
{
    public class ClientManager : BaseManager
    {
        public List<Data.ClientSummary> GetClientSummary()
        {
            return DataContext.ClientSummaries.ToList();
        }
        
        public Data.Client GetClient(int clientId)
        {
            return DataContext.Clients.FirstOrDefault(i=>i.clientid == clientId);
        }
    }
}
