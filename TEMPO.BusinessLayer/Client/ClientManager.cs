using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Model;

namespace TEMPO.BusinessLayer.Client
{
    public class ClientManager : BaseManager
    {
        public List<ClientSummary> GetClientSummary(DateTime lastHoursSince)
        {
            return DataContext.ClientSummaries
                .Where(i => i.lastHoursLogged.HasValue && i.lastHoursLogged > lastHoursSince)
                .ToList();
        }

        public List<ClientSummary> GetClientSummary()
        {
            return DataContext.ClientSummaries
                .ToList();
        }

        public Model.Client GetClient(int clientId)
        {
            return DataContext.Clients.FirstOrDefault(i => i.clientid == clientId);
        }

        public void UpdateClient(int clientId, string clientName)
        {
            Model.Client client = GetClient(clientId);
            if (client != null)
            {
                client.clientname = clientName;
                DataContext.SaveChanges();
            }
        }

        public List<Model.Client> GetClients()
        {
            return DataContext.Clients.ToList();
        }

        public Model.Client CreateClient(string clientName)
        {
            var newClient = new Model.Client
            {
                clientname = clientName
            };
            DataContext.Clients.Add(newClient);
            DataContext.SaveChanges();

            return newClient;
        }

    }
}
