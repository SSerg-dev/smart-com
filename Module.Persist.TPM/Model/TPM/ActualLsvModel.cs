using Core.Data;
using System;

namespace Module.Persist.TPM.Model.TPM
{
    public class ActualLSV : IEntity<Guid> // интерфейс реализуем для экспорта
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public string ClientHierarchy { get; set; }
        public string Name { get; set; }
        public string BrandTech { get; set; }
        public string Event { get; set; }
        public string Mechanic { get; set; }
        public string MechanicIA { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public string MarsStartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string MarsEndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public string MarsDispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public string MarsDispatchesEnd { get; set; }
        public string Status { get; set; }
        public double? ActualInStoreDiscount { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? PlanPromoBaselineLSV { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
        public double? ActualPromoLSVByCompensation { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? ActualPromoLSVSI { get; set; }
        public double? ActualPromoLSVSO { get; set; }
        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }
        public double? ActualPromoPostPromoEffectLSVW1 { get; set; }
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }
        public double? ActualPromoPostPromoEffectLSVW2 { get; set; }
        public double? PlanPromoPostPromoEffectLSV { get; set; }
        public double? ActualPromoPostPromoEffectLSV { get; set; }
        public bool? InOut { get; set; }
        public bool? IsOnInvoice { get; set; }
    }
}
