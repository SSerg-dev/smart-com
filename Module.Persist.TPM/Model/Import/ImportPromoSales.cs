using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoSales : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo ID")]
        public int? Number { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Promo name")]
        public String Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(
            LookupEntityType = typeof(Client), MediumEntityType = typeof(CommercialSubnet), TerminalEntityType = typeof(CommercialNet),
            NavigationPropertyName = "Client", LookupPropertyName = "Name")]
        [Display(Name = "Customer")]
        public String CustomerName { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoStatus), LookupPropertyName = "Name")]
        [Display(Name = "PromoStatus")]
        public String PromoStatusName { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [NavigationPropertyMap(LookupEntityType = typeof(Mechanic), LookupPropertyName = "MechanicName")]
        [Display(Name = "Mechanic")]
        public String MechanicName { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "StartDate")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "EndDate")]
        public DateTimeOffset? EndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "DispatchesStart")]
        public DateTimeOffset? DispatchesStart { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "DispatchesEnd")]
        public DateTimeOffset? DispatchesEnd { get; set; }

        [ImportCSVColumn(ColumnNumber = 14)]
        [NavigationPropertyMap(LookupEntityType = typeof(BudgetItem), LookupPropertyName = "Name")]
        [Display(Name = "Бюджетная статья")]
        public String BudgetItenName { get; set; }

        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "План")]
        public int? Plan { get; set; }

        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "Факт")]
        public int? Fact { get; set; }
    }
}
