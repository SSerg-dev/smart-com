using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CalendarСompetitorCompany))]
    public class HistoricalCalendarCompetitorCompany : BaseHistoricalEntity<Guid>
    {
        public string CompanyName { get; set; }
        public Guid CalendarCompetitorId { get; set; }
    }
}
