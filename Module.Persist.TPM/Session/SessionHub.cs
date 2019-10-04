using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Session;
using Session = Module.Persist.TPM.Session;
using System.Collections.Concurrent;
using System.Threading;

namespace Module.Persist.TPM.Session
{
    public class SessionHub : Hub
    {
        public override Task OnConnected()
        {
            Session.AddClient(Context.ConnectionId, Context.User.Identity.Name, Clients.Caller);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Session.AddClient(Context.ConnectionId, Context.User.Identity.Name, Clients.Caller);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Session.RemoveClient(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
