using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportCalendarCompetitorCompany : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(CalendarCompetitor), LookupPropertyName = "Name")]
        [Display(Name = "Competitor Name")]
        public string CompetitorName { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
    }
}
