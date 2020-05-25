using Core.Settings;
using Persist.Model.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public static class FileBufferExtention
    {
        public static string GetExportCSVFileData(this FileBuffer fileBuffer)
        {
            string dirSetting = GetBufferDirrectory();
            string fileName = fileBuffer.GetFileName();
            string filePath = Path.Combine(dirSetting, fileName);
            string res = File.ReadAllText(filePath);
            return res;
        }

        public static bool SetExportCSVFileData(this FileBuffer fileBuffer, IEnumerable<string> outputData)
        {
            string dirSetting = GetBufferDirrectory();
            string fileName = fileBuffer.GetFileName();
            string destPath = Path.Combine(dirSetting, fileName);
            File.WriteAllLines(destPath, outputData);
            bool res = File.Exists(destPath) ? true : false;

            return res;
        }

        public static string GetBufferDirrectory()
        {
            string dirSetting = AppSettingsManager.GetSetting("INTERFACE_DIRECTORY", "InterfaceFiles");
            if (!Directory.Exists(dirSetting))
            {
                Directory.CreateDirectory(dirSetting);
            }
            return dirSetting;
        }

        public static string GetFileName(this FileBuffer fileBuffer)
        {
            string timestamp = fileBuffer.CreateDate.ToString("_yyyyMMddHHmmss");
            string fileName = string.Format("{0}{1}", fileBuffer.FileName, timestamp);
            return fileName;
        }
    }
}
