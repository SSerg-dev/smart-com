using Core.Security;
using Core.Settings;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Session
{
    public class Client
    {
        public string UserName { get; }
        public DateTimeOffset EndSessionDate { get; private set; }
        private dynamic CurrentClient { get; set; }

        public Client(dynamic currentClient, string userName)
        {
            this.CurrentClient = currentClient;
            this.UserName = userName;
            this.UpdateEndSessionDate();
        }

        public void UpdateEndSessionDate()
        {
            /* Не коммитить
            this.EndSessionDate = DateTimeOffset.Now.AddSeconds(5);
            */

            var sessionTimeout = AppSettingsManager.GetSetting<int>(Consts.SESSION_TIME, 15);
            this.EndSessionDate = DateTimeOffset.Now.AddMinutes(sessionTimeout).AddSeconds(1);
        }

        public void ShowSessionNotification()
        {
            this.CurrentClient.showSessionNotification(); 
        }
    }
}
