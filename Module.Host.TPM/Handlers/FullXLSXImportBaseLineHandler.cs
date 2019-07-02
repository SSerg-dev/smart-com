using Interfaces.Core.Common;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Persist.TPM.Utils;
using ProcessingHost.Handlers.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utility;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers {
    public class FullXLSXImportBaseLineHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            IDictionary<string, IEnumerable<string>> filters = data.GetValue<IDictionary<string, IEnumerable<string>>>("Filters");
            DateTimeOffset startDate = data.GetValue<DateTimeOffset>("StartDate");
            DateTimeOffset finishDate = data.GetValue<DateTimeOffset>("FinishDate");
            bool needClearData = data.GetValue<bool>("NeedClearData");
            return new FullXLSXImportBaseLineAction(settings, startDate, finishDate, filters, needClearData);
        }

        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data) {
            bool allowPartialApply = HandlerDataHelper.GetIncomingArgument<bool?>("AllowPartialApply", handlerData, false) ?? false;
            data.SetValue<bool>("AllowPartialApply", allowPartialApply);
            bool needClearData = HandlerDataHelper.GetIncomingArgument<bool>("CrossParam.ClearTable", handlerData);
            data.SetValue<bool>("NeedClearData", needClearData);
            string clientFilter = HandlerDataHelper.GetIncomingArgument<TextListModel>("CrossParam.ClientFilter", handlerData).Value;
            string startPeriodFilter = HandlerDataHelper.GetIncomingArgument<string>("CrossParam.StartDate", handlerData);
            string finishPeriodFilter = HandlerDataHelper.GetIncomingArgument<string>("CrossParam.FinishDate", handlerData);
            DateTimeOffset startDate = DateTime.ParseExact(startPeriodFilter, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTimeOffset endDate = DateTime.ParseExact(finishPeriodFilter, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            data.SetValue("StartDate", startDate);
            data.SetValue("FinishDate", endDate);
            IDictionary<string, IEnumerable<string>> filters = new Dictionary<string, IEnumerable<string>>() {
                { ModuleFilterName.Client, FilterHelper.ParseFilter(clientFilter) }
            };
            data.SetValue("Filters", filters);
        }

        protected override void WriteParametersToLog(ILogWriter handlerLogger, ExecuteData data) {
            IDictionary<string, IEnumerable<string>> filters = data.GetValue<IDictionary<string, IEnumerable<string>>>("Filters");
            string sourceFilter = filters == null ? "NULL" : String.Join(Environment.NewLine, filters.Select(x => String.Format("{0}:\r\n\t{1}", x.Key, String.Join(Environment.NewLine + "\t", x.Value))));
            handlerLogger.Write(true, String.Format("Filter: \r\n{0}", sourceFilter), "Message");
            DateTimeOffset startDate = data.GetValue<DateTimeOffset>("StartDate");
            string startDateString = startDate == null ? "NULL" : startDate.ToString("yyyy-MM-dd");
            DateTimeOffset finishDate = data.GetValue<DateTimeOffset>("FinishDate");
            string finishDateString = finishDate == null ? "NULL" : finishDate.ToString("yyyy-MM-dd");
            handlerLogger.Write(true, String.Format("Start date: {0}, End date: {1}", startDateString, finishDateString), "Message");
            bool needClearData = data.GetValue<bool>("NeedClearData");
            handlerLogger.Write(true, String.Format("Clear table: {0}", needClearData), "Message");
        }
    }

}
