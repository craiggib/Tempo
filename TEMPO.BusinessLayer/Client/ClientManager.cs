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
            return DataContext.Clients.FirstOrDefault(i => i.clientid == clientId);
        }

        public void UpdateClient(int clientId, string clientName)
        {
            Data.Client client = GetClient(clientId);
            if (client != null)
            {
                client.clientname = clientName;
                DataContext.SaveChanges();
            }
        }

        public List<Data.Client> GetClients()
        {
            return DataContext.Clients.ToList();
        }

        public Data.Client CreateClient(string clientName)
        {
            try
            {
                var newClient = new Data.Client
                {
                    clientname = clientName
                };
                DataContext.Clients.Add(newClient);
                DataContext.SaveChanges();

                return newClient;
            }
            catch (Exception e)
            {
                int i = 0;
            }
            return null;
        }
    }
}
