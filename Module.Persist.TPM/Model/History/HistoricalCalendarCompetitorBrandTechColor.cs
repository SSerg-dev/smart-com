using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CalendarCompetitorBrandTechColor))]
    public class HistoricalCalendarCompetitorBrandTechColor : BaseHistoricalEntity<Guid>
    {
        public string BrandTech { get; set; }
        public string Color { get; set; }
        public Guid? CalendarCompetitorId { get; set; }
        public Guid CalendarCompetitorCompanyId { get; set; }
    }
}
