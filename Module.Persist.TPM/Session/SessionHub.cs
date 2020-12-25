using Looper.Core;
using Looper.Parameters;
using Microsoft.AspNet.SignalR;
using Persist;
using Persist.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public void StartMoniringHandler(Guid handlerId)
        {
            DatabaseContext context = new DatabaseContext();
            lock (context)
            {
                string status = string.Empty;
                while (status != "ERROR" && status != "COMPLETE")
                {
                    Thread.Sleep(2000);
                    status = context.SqlQuery<string>($"select [Status] from [DefaultSchemaSetting].LoopHandler where id = '{handlerId}'").FirstOrDefault();
                }

                if (status.Equals("ERROR"))
                    return;

                LoopHandler handler = context.Set<LoopHandler>().FirstOrDefault(h => h.Id == handlerId);
                HandlerData data = handler.GetParameterData();
                var property = data.OutcomingParameters["File"].Value.GetType().GetProperty("Name");
                var fileName = property.GetValue(data.OutcomingParameters["File"].Value);

                Clients.Caller.downloadFile(fileName);
            }
        }
    }
}
