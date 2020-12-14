using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.MainNightProcessing
{
    public class MainNightProcessingHelper
    {
        public static void SetProcessingFlagUp(DatabaseContext context, string processingPrefix)
        {
            string setProcessingFlagUpScript = string.Format(Consts.Templates.setProcessingFlagUpTemplate, processingPrefix);
            context.ExecuteSqlCommand(setProcessingFlagUpScript);
        }

        public static void SetProcessingFlagDown(DatabaseContext context, string processingPrefix)
        {
            string setProcessingFlagDownScript = string.Format(Consts.Templates.setProcessingFlagDownTemplate, processingPrefix);
            context.ExecuteSqlCommand(setProcessingFlagDownScript);
        }

        public static void SetAllFlagsDown(DatabaseContext context, string[] processingPrefixes)
        {
            string setAllFlagsDownScript = string.Format(Consts.Templates.setAllFlagsDownTemplate, string.Join(",", processingPrefixes));
            context.ExecuteSqlCommand(setAllFlagsDownScript);
        }
    }
}
