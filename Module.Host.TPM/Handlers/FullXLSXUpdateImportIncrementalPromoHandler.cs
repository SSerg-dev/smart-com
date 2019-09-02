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
    public class FullXLSXUpdateImportIncrementalPromoHandler : FullXLSXImportHandler {
        protected override IAction GetAction(FullImportSettings settings, ExecuteData data) {
            IDictionary<string, IEnumerable<string>> filters = data.GetValue<IDictionary<string, IEnumerable<string>>>("Filters");
			return new FullXLSXUpdateImportIncrementalPromoAction(settings, filters);
        }

        protected override void InitializeParameters(HandlerData handlerData, ExecuteData data) {
			bool allowPartialApply = HandlerDataHelper.GetIncomingArgument<bool?>("AllowPartialApply", handlerData, false) ?? false;
			data.SetValue<bool>("AllowPartialApply", allowPartialApply);
			string clientFilter = HandlerDataHelper.GetIncomingArgument<TextListModel>("CrossParam.ClientFilter", handlerData).Value;
			IDictionary<string, IEnumerable<string>> filters = new Dictionary<string, IEnumerable<string>>() {
				{ ModuleFilterName.Client, FilterHelper.ParseFilter(clientFilter) }
			};
			data.SetValue("Filters", filters);
		}

        protected override void WriteParametersToLog(ILogWriter handlerLogger, ExecuteData data) {
            IDictionary<string, IEnumerable<string>> filters = data.GetValue<IDictionary<string, IEnumerable<string>>>("Filters");
            string sourceFilter = filters == null ? "NULL" : String.Join(Environment.NewLine, filters.Select(x => String.Format("{0}:\r\n\t{1}", x.Key, String.Join(Environment.NewLine + "\t", x.Value))));
            handlerLogger.Write(true, String.Format("Filter: \r\n{0}", sourceFilter), "Message");
        }
    }

}
