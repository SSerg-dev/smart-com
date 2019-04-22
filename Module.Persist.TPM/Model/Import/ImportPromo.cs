using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromo : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo name")]
        public String Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(
            LookupEntityType = typeof(Client), MediumEntityType = typeof(CommercialSubnet), TerminalEntityType = typeof(CommercialNet),
            NavigationPropertyName = "Client", LookupPropertyName = "Name")]
        [Display(Name = "Customer")]
        public String CustomerName { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(LookupEntityType = typeof(Brand), LookupPropertyName = "Name")]
        [Display(Name = "Brand")]
        public String BrandName { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "Name")]
        [Display(Name = "BrandTech")]
        public String BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoStatus), LookupPropertyName = "Name")]
        [Display(Name = "PromoStatus")]
        public String PromoStatusName { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [NavigationPropertyMap(LookupEntityType = typeof(Mechanic), LookupPropertyName = "MechanicName")]
        [Display(Name = "Mechanic")]
        public String MechanicName { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Priority")]
        public String Priority { get; set; }

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

        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "EventName")]
        public String EventName { get; set; }

        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "RoiPlan")]
        public int? RoiPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "RoiFact")]
        public int? RoiFact { get; set; }

        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "PlanBaseline")]
        public int? PlanBaseline { get; set; }

        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "PlanDuration")]
        public int? PlanDuration { get; set; }

        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "PlanUplift")]
        public int? PlanUplift { get; set; }

        [ImportCSVColumn(ColumnNumber = 19)]
        [Display(Name = "PlanIncremental")]
        public int? PlanIncremental { get; set; }

        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "PlanActivity")]
        public int? PlanActivity { get; set; }

        [ImportCSVColumn(ColumnNumber = 21)]
        [Display(Name = "PlanSteal")]
        public int? PlanSteal { get; set; }

        [ImportCSVColumn(ColumnNumber = 22)]
        [Display(Name = "FactBaseline")]
        public int? FactBaseline { get; set; }

        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "FactDuration")]
        public int? FactDuration { get; set; }

        [ImportCSVColumn(ColumnNumber = 24)]
        [Display(Name = "FactUplift")]
        public int? FactUplift { get; set; }

        [ImportCSVColumn(ColumnNumber = 25)]
        [Display(Name = "FactIncremental")]
        public int? FactIncremental { get; set; }

        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "FactActivity")]
        public int? FactActivity { get; set; }

        [ImportCSVColumn(ColumnNumber = 27)]
        [Display(Name = "FactSteal")]
        public int? FactSteal { get; set; }

        [ImportCSVColumn(ColumnNumber = 28)]
        [Display(Name = "ProductFilter")]
        public string ProductFilter { get; set; }

        [ImportCSVColumn(ColumnNumber = 29)]
        [NavigationPropertyMap(LookupEntityType = typeof(Color), LookupPropertyName = "DisplayName")]
        [Display(Name = "Color name")]
        public String ColorName { get; set; }
    }
}
