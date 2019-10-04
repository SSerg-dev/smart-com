using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Session
{
    public static class Session
    {
        public static ConcurrentDictionary<string, Client> ClientCollection { get; }
        private static int TimerInterval { get; }
        private static Timer Timer { get; }
        static Session()
        {
            ClientCollection = new ConcurrentDictionary<string, Client>();
            TimerInterval = 1000;
            Timer = new Timer(new TimerCallback(Tick), null, 0, TimerInterval); 
        }

        private static void Tick(object state)
        {
            if (ClientCollection.Any())
            {
                var sessionEndedClients = ClientCollection.Where(x => x.Value.EndSessionDate < DateTimeOffset.Now);
                foreach (var sessionEndedClient in sessionEndedClients)
                {
                    Client client;
                    var successed = ClientCollection.TryRemove(sessionEndedClient.Key, out client);
                    if (successed)
                    {
                        client.ShowSessionNotification();
                    }
                }
            }
            else
            {
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public static void AddClient(string connectionId, string userName, dynamic caller)
        {
            var newClient = new Client(caller, userName);
            ClientCollection.TryAdd(connectionId, newClient);
            Timer.Change(0, TimerInterval);
        }

        public static void RemoveClient(string connectionId)
        {
            Client client;
            ClientCollection.TryRemove(connectionId, out client);
        }
    }
}
