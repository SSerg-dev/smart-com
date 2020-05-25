using Core.Extensions;
using Core.Settings;
using Interfaces.Core.Model;
using Interfaces.Implementation.Outbound.Extractor;
using Module.Host.TPM.Handlers.MainNightProcessing;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Extensions;
using Persist.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Export;

namespace Module.Host.TPM.Actions.Interface.Outcoming
{
    class OutputIncrementalProcessAction : BaseCSVExtractorAction<PromoProductDifference, PromoProductDifference>
    {
        public OutputIncrementalProcessAction(Guid interfaceId, Guid? handlerId, Guid? userId) : base(interfaceId, handlerId, userId)
        {
            InterfaceId = interfaceId;
            HandlerId = handlerId;
            UserId = userId;
        }

        protected override void Init()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                CSVExtractInterfaceSetting setting = context.CSVExtractInterfaceSettings.FirstOrDefault(x => x.InterfaceId.Equals(InterfaceId));
                if (setting == null)
                {
                    throw new ApplicationException(String.Format("Не найдены настройки интерфейса '{0}'", InterfaceId));
                }
                InterfaceName = setting.Interface.Name;
                DisplayFileName = setting.FileNameMask;
                Delimiter = setting.Delimiter;
                UseQuoting = setting.UseQuoting;
                QuoteChar = setting.QuoteChar;

                ExportDataBuilder = ExportBuilderFactory.GetCSVExportDataBuilder(typeof(PromoProductDifference), Delimiter, UseQuoting, QuoteChar);
                ExportModelBuilder = ExportCSVBuilderFactory.GetExportModelBuilder(typeof(PromoProductDifference), typeof(PromoProductDifference));
            }
        }

        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                Init();
                WriteParametersToLog();
                using (DatabaseContext context = new DatabaseContext())
                {
                    // Получить список моделей из БД
                    IEnumerable<PromoProductDifference> query = GetQueryPartinion(context);
                    int sourceCount = query.Count();
                    fileLogger.Write(true, String.Format("Received {0} source records", sourceCount) + Environment.NewLine, "Message");
                    Results["ExportSourceRecordCount"] = sourceCount;
                    // Построить DTO-модели
                    IEnumerable<PromoProductDifference> dtoList = BuildDTOList(query);
                    fileLogger.Write(true, String.Format("Received {0} records to process", dtoList.Count()) + Environment.NewLine, "Message");
                    // Обработать список DTO-моделей
                    dtoList = PostBuildDTOList(dtoList);
                    int resultCount = dtoList.Count();
                    fileLogger.Write(true, String.Format("Received {0} records after processing", resultCount) + Environment.NewLine, "Message");
                    Results["ExportResultRecordCount"] = resultCount;
                    // Получить выходные данные
                    IEnumerable<string> outputData = BuildOutputData(dtoList);
                    IEnumerable<string> outputDataWithQuoting = AddQuoting(outputData);
                    // Сохранить данные в FileBuffer
                    SaveOutputDataToFileBuffer(outputDataWithQuoting);
                    fileLogger.Write(true, "Processing completed successfully" + Environment.NewLine, "Message");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error during Extract interface '{0}'", InterfaceId);
                Errors.Add(String.Format("Error during Extract interface '{0}': {1}", InterfaceId, e.ToString()));
            }
            finally
            {
                logger.Trace("Finish");

                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                using (DatabaseContext context = new DatabaseContext())
                {
                    MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                }
            }
        }

        static IEnumerable<string> AddQuoting(IEnumerable<string> outputData)
        {
            List<string> outputDataWithQuoting = new List<string>();

            foreach (string s in outputData)
            {
                string[] dataMas = s.Split(',');
                string sWithQuoting = string.Join(",", dataMas.Select(x => string.Format("\"{0}\"", x)));
                outputDataWithQuoting.Add(sWithQuoting);
            }

            return outputDataWithQuoting;
        }

        /// <summary>
        /// Получить список исходных записей из БД
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //TODO: Перенести в ядро
        protected virtual List<PromoProductDifference> GetQueryPartinion(DatabaseContext context)
        {
            IQueryable<PromoProductDifference> query = context.Set<PromoProductDifference>().AsNoTracking();
            query = AddFilter(query);
            List<PromoProductDifference> result = new List<PromoProductDifference>();
            foreach (var partQuery in query.Partition(50000))
            {
                result.AddRange(partQuery.ToList());
            }
            return result;
        } 
    }
}
