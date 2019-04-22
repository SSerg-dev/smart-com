using Core.Settings;
using NLog;
using ProcessingHost;
using System;
using System.ServiceProcess;

namespace ProcessingService {
    partial class ProcessingService : ServiceBase {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ProcessingService() {
            InitializeComponent();
            ServiceName = AppSettingsManager.GetSetting(Consts.AppSettingNames.ServiceName, "TPM Processing Host");
        }

        public static void Main() {
            ServiceBase.Run(new ProcessingService());
        }

        public static ProcessingHost.ProcessingHost host = null;

        protected override void OnStart(string[] args) {
            try {
                // Инициализация хоста
                host = new ProcessingHost.ProcessingHost();
                host.StartUp(AppDomain.CurrentDomain.BaseDirectory);
                //string configPath = System.Reflection.Assembly.GetExecutingAssembly().Location + ".config";
                string configurations = AppSettingsManager.GetSetting(Consts.AppSettingNames.ConfigurationList, "");
                string displayMessage = String.Format("ProcessingHost is ready. Configurations: {0}", configurations);
                logger.Info(displayMessage);
            } catch (Exception e) {
                logger.Fatal(e, "Произошла фатальная ошибка в ProcessingHost");
                throw;
            }
        }

        protected override void OnStop() {
            if (host != null) {
                host.Stop();
            }
        }
    }
}
