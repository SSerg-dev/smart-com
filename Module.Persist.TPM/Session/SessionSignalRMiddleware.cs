using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Module.Persist.TPM.Session
{
    public class SessionSignalRMiddleware : OwinMiddleware
    {
        public SessionSignalRMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var clientKeys = Session.ClientCollection.Where(x => x.Value.UserName == context.Authentication.User.Identity.Name).Select(x => x.Key);
            foreach (var clientKey in clientKeys)
            {
                Client client;
                var successed = Session.ClientCollection.TryGetValue(clientKey, out client);
                if (successed)
                {
                    client.UpdateEndSessionDate();
                }
            }

            await Next.Invoke(context);
        }
    }
}
