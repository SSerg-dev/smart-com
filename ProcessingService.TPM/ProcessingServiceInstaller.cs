﻿using Core.Settings;
using ProcessingHost;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;

namespace ProcessingService {
    [RunInstaller(true)]
    public class ProcessingServiceInstaller : Installer {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProcessingServiceInstaller() {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            string configPath = System.Reflection.Assembly.GetExecutingAssembly().Location + ".config";
            string serviceName = AppSettingsManager.GetSetting(configPath, Consts.AppSettingNames.ServiceName, "TPM Processing Host");
            
            if (String.IsNullOrEmpty(serviceName)) {
                throw new ApplicationException(String.Format("ProcessingServiceInstaller: Не задано значение настройки '{0}'", Consts.AppSettingNames.ServiceName));
            }

            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = serviceName;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
