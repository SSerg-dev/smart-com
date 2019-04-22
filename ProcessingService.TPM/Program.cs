﻿using Core.Settings;
using NLog;
using ProcessingHost;
using System;

namespace ProcessingService {
    class Program {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static ProcessingHost.ProcessingHost host = null;

        static void Main(string[] args) {
            try {
                // Инициализация хоста
                host = new ProcessingHost.ProcessingHost();
                host.StartUp(AppDomain.CurrentDomain.BaseDirectory);
                string configurations = AppSettingsManager.GetSetting(Consts.AppSettingNames.ConfigurationList, "");
                string displayMessage = String.Format("ProcessingHost is ready. Configurations: {0}", configurations);
                logger.Info(displayMessage);
                Console.WriteLine(displayMessage);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            } catch (Exception e) {
                logger.Fatal(e, "Произошла фатальная ошибка в ProcessingHost");
                throw;
            } finally {
                if (host != null) {
                    host.Stop();
                }
            }
        }
    }
}
