using Module.Persist.TPM.Model.SimpleModel;
using Persist;
using Persist.Model.Settings;
using System;
using System.Linq;

namespace Module.Frontend.TPM.FunctionalHelpers.RSPeriod
{
    public static class RSPeriodHelper
    {
        public static StartEndModel GetRSPeriod(DatabaseContext Context)
        {
            string weeks = Context.Set<Setting>().Where(g => g.Name == "RS_START_WEEKS").FirstOrDefault().Value;
            DateTimeOffset today = DateTimeOffset.Now;
            DateTimeOffset endDate = new DateTimeOffset(today.Year, 12, 31, 23, 0, 0, new TimeSpan(0, 0, 0));
            StartEndModel startEndModel = new StartEndModel
            {
                EndDate = endDate
            };

            if (Int32.TryParse(weeks, out int intweeks))
            {
                
                DateTimeOffset RsStartDate = today.AddDays(intweeks * 7);
                startEndModel.StartDate = RsStartDate;

                return startEndModel;
            }
            else
            {
                startEndModel.StartDate = DateTimeOffset.MinValue;
                return startEndModel;
            }
        }
    }
}
