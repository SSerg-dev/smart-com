using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public static class Consts
    {
        public static class Templates
        {
            public const string setProcessingFlagUpTemplate = "UPDATE [dbo].[JobFlag] SET [Value] = 1 WHERE [Prefix] = N'{0}'";
            public const string setProcessingFlagDownTemplate = "UPDATE [dbo].[JobFlag] SET [Value] = 0 WHERE [Prefix] = N'{0}'";
            public const string setAllFlagsDownTemplate = "UPDATE [dbo].[JobFlag] SET [Value] = 0 WHERE [Prefix] IN ({0})";
            public const string checkProcessingFlagTemplate = "SELECT [Value] FROM [dbo].[JobFlag] WHERE [Prefix] = N'{0}'";
            public const string startJobTemplate = "msdb.dbo.sp_start_job @job_name = N'{0}', @step_name = N'{1}'";
        }
    }
}
