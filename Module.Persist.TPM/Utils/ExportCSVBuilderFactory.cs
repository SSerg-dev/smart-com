using Module.Persist.TPM.Utils.ExportCSVModelBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Export;

namespace Module.Persist.TPM.Utils
{
    public class ExportCSVBuilderFactory
    {
        /// <summary>
        /// Получить класс для построения данных для файла экспорта
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuoting"></param>
        /// <param name="quoteChar"></param>
        /// <returns></returns>
        public static IExportDataBuilder GetCSVExportDataBuilder(Type exportType, string delimiter, bool useQuoting, string quoteChar)
        {
            string typeName = exportType.Name;
            IExportDataBuilder result;
            switch (typeName)
            {
                default:
                    result = new CSVExportDataBuilder(exportType, delimiter, useQuoting, quoteChar);
                    break;
                    //throw new Exception(String.Format("Не найдена функция для построения '{0}'", typeName));
            }
            return result;
        }

        /// <summary>
        /// Получить класс для построения модели для экспорта
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static IExportModelBuilder GetExportModelBuilder(Type sourceType, Type destinationType)
        {
            string typeName = sourceType.Name + "###" + destinationType.Name;
            IExportModelBuilder result;
            switch (typeName)
            {
                case "PromoProductDifference###PromoProductDifference":
                    result = new IncrementalToApolloModelBuilder();
                    break;
                default:
                    throw new Exception(String.Format("Builder class was not found '{0}'", typeName));
            }
            return result;
        }
    }
}
