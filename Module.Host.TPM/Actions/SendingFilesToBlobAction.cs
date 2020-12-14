using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    class SendingFilesToBlobAction : BaseAction
    {
        public override void Execute()
        {
            string workType = AppSettingsManager.GetSetting<string>("HANDLER_LOG_TYPE", "File");
            if (workType == "Azure")
            {
                try
                {
                    UploadFiles("FULL_LOG_DIRECTORY", "Logs");
                    UploadFiles("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                    UploadFiles("IMPORT_DIRECTORY", "ImportFiles");
                    UploadFiles("EXPORT_DIRECTORY", "ExportFiles");
                }
                catch (Exception e)
                {
                    Errors.Add(e.Message);
                }
            }
            else
            {
                Warnings.Add("This handler active only in Azure version");
            }
        }

        private void UploadFiles(string fileDirName, string defaultName)
        {
            string fileDir = GetDirectory(fileDirName, defaultName);
            var filesInDir = Directory.EnumerateFiles(fileDir);
            var filesToUpload = GetCheckedFiles(filesInDir);
            if(filesToUpload.Count > 0)
            {
                UploadToBlob(filesToUpload, fileDir);
            }
            else
            {
                Results.Add($"There are no files to upload in {defaultName}", "");
            }
        }

        private void UploadToBlob(IEnumerable<string> filesToUpload, string fileDir)
        {
            foreach (var log in filesToUpload)
            {
                var fileName = log.Substring(fileDir.Length + 1).Split('.').GetValue(0).ToString();

                var logWriter = new LogWriter(fileName, fileDir);

                logWriter.UploadToBlob();

                Results.Add($"{fileName} uploaded to blob storage", "");
            }
        }

        private string GetDirectory(string settingName, string defaulSettingName)
        {
            string logDir = AppSettingsManager.GetSetting<string>(settingName, defaulSettingName);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            return logDir;
        }

        private List<string> GetCheckedFiles(IEnumerable<string> filesInDir)
        {
            var checkedFiles = new List<string>();

            foreach (var filePath in filesInDir)
            {
                var fileWriteTime = File.GetLastWriteTime(filePath);
                if((DateTime.Now - fileWriteTime).Days > 1)
                {
                    checkedFiles.Add(filePath);
                }
            }

            return checkedFiles;
        }
    }
}
