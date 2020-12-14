using Core.Data;
using Core.Settings;
using Module.Frontend.TPM.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.OData.Query;
using Utility.FileWorker;

/// <summary>
/// Экспортирует записи с id не в GUID
/// </summary>
namespace Frontend.Core.Extensions.Export {
    public class NonGuidIdExporter : BaseExporter {

        private IEnumerable<Column> columns;

        public NonGuidIdExporter(IEnumerable<Column> columns) {
            this.columns = columns.OrderBy(c => c.Order);
        }

        public string GetExportFileName(string entityName, string userName) {
            string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
            string userShortName = GetUserName(userName);
            string filename = String.Format("{0}_{1}_{2:yyyyMMddHHmmss}.xlsx", entityName, userShortName, DateTime.Now);
            if (!Directory.Exists(exportDir)) {
                Directory.CreateDirectory(exportDir);
            }
            return Path.Combine(exportDir, filename);
        }

        public void Export(System.Collections.IEnumerable records, string filename) {
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
                IWorkbook wb = new XSSFWorkbook();
                ISheet sheet = wb.CreateSheet("Sheet1");
                ICreationHelper cH = wb.GetCreationHelper();
                WriteHeaders(sheet);
                int rowCount = 1;
                foreach (var item in records) {
                    IRow row = sheet.CreateRow(rowCount);
                    rowCount++;
                    var record = CustomGetDictionary(item);
                    int cellCount = 0;
                    foreach (Column col in columns) {
                        ICell cell = row.CreateCell(cellCount);
                        cellCount++;
                        string val;
                        if (col.Function != null) {
                            val = col.EncodeValue(col.Function(record, this), false, String.Empty);
                        } else {
                            val = col.EncodeValue(GetValue(record, col.Field), false, String.Empty);
                        }
                        cell.SetCellValue(val);
                    }
                }
                wb.Write(stream);
            }
            FileDispatcher fileDispatcher = new FileDispatcher();
            string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
            fileDispatcher.UploadToBlob(Path.GetFileName(filename), Path.GetFullPath(filename), exportDir.Split('\\').Last());
        }

        protected IDictionary<string, object> CustomGetDictionary(object record) {
            IDictionary<string, object> dict;
            dict = new Dictionary<string, object>();
            Type elementType = record.GetType();
            PropertyInfo[] properties = elementType.GetProperties();
            foreach (PropertyInfo property in properties) {
                dict[property.Name] = property.GetValue(record);
            }
            return dict;
        }

        private void WriteHeaders(ISheet sheet) {
            IRow row = sheet.CreateRow(0);
            int cellCount = 0;
            foreach (Column col in columns) {
                ICell cell = row.CreateCell(cellCount);
                cellCount++;
                cell.SetCellValue(col.Header);
            }
        }
    }
}