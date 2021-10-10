using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CalendarCompetitorCompany))]
    public class HistoricalCalendarCompetitorCompany : BaseHistoricalEntity<Guid>
    {
        public string CompanyName { get; set; }
        public Guid? CalendarCompetitorId { get; set; }
    }
}
