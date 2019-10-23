using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.TPM
{
    public class ImportActualLsv : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "PromoID")]
        public int? Number { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client")]
        public string ClientHierarchy { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Promo name")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "In Out")]
        public bool? InOut { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Brandtech")]
        public string BrandTech { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Event")]
        public string Event { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Mars mechanic")]
        public string Mechanic { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "IA mechanic")]
        public string MechanicIA { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Start date")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Mars Start date")]
        public string MarsStartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "End date")]
        public DateTimeOffset? EndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "Mars End date")]
        public string MarsEndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "Dispatch start")]
        public DateTimeOffset? DispatchesStart { get; set; }

        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "Mars Dispatch start")]
        public string MarsDispatchesStart { get; set; }

        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "Dispatch end")]
        public DateTimeOffset? DispatchesEnd { get; set; }

        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "Mars Dispatch end")]
        public string MarsDispatchesEnd { get; set; }

        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "Actual InStore Discount")]
        public int? ActualInStoreDiscount { get; set; }

        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "Plan Promo Uplift %")]
        public double? PlanPromoUpliftPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 19)]
        [Display(Name = "Actual Promo Uplift %")]
        public double? ActualPromoUpliftPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "Plan Promo Baseline LSV")]
        public double? PlanPromoBaselineLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 21)]
        [Display(Name = "Actual Promo Baseline LSV")]
        public double? ActualPromoBaselineLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 22)]
        [Display(Name = "Plan Promo Incremental LSV")]
        public double? PlanPromoIncrementalLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "Actual Promo Incremental LSV")]
        public double? ActualPromoIncrementalLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 24)]
        [Display(Name = "Plan Promo LSV")]
        public double? PlanPromoLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 25)]
        [Display(Name = "Actual PromoLSV By Compensation")]
        public double? ActualPromoLSVByCompensation { get; set; }

        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "Actual Promo LSV")]
        public double? ActualPromoLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 27)]
        [Display(Name = "Plan Post Promo Effect W1, %")]
        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }

        [ImportCSVColumn(ColumnNumber = 28)]
        [Display(Name = "Actual Post Promo Effect W1")]
        public double? ActualPromoPostPromoEffectLSVW1 { get; set; }

        [ImportCSVColumn(ColumnNumber = 29)]
        [Display(Name = "Plan Post Promo Effect W2, %")]
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }

        [ImportCSVColumn(ColumnNumber = 30)]
        [Display(Name = "Actual Post Promo Effect W2, %")]
        public double? ActualPromoPostPromoEffectLSVW2 { get; set; }

        [ImportCSVColumn(ColumnNumber = 31)]
        [Display(Name = "Plan Post Promo Effect LSV total")]
        public double? PlanPromoPostPromoEffectLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 32)]
        [Display(Name = "Actual Promo Effect LSV total")]
        public double? ActualPromoPostPromoEffectLSV { get; set; }
    }
}
