using System;
using Core.Data;

namespace Module.Persist.TPM.Model.DTO {
    public class PromoGridView : IEntity<Guid> {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool Disabled { get; set; }
        public string Mechanic { get; set; }
        public Guid? CreatorId { get; set; }
        public string MechanicIA { get; set; }
        public int? ClientTreeId { get; set; }
        public string ClientHierarchy { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsCMManagerApproved { get; set; }
        public double? PlanInstoreMechanicDiscount { get; set; }

        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? DispatchesEnd { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }

        public string MarsEndDate { get; set; }
        public string MarsStartDate { get; set; }

        public string MarsDispatchesEnd { get; set; }
        public string MarsDispatchesStart { get; set; }

        public string BrandName { get; set; }
        public string BrandTechName { get; set; }
        public string PromoEventName { get; set; }
        public string PromoStatusName { get; set; }
        public string PromoStatusColor { get; set; }
        public string MarsMechanicName { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public string PlanInstoreMechanicName { get; set; }
        public string PromoStatusSystemName { get; set; }
        public string PlanInstoreMechanicTypeName { get; set; }
        public string ProductHierarchy { get; set; }

        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoTIMarketing { get; set; }
        public double? PlanPromoXSites { get; set; }
        public double? PlanPromoCatalogue { get; set; }
        public double? PlanPromoPOSMInClient { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }
        public double? ActualPromoXSites { get; set; }
        public double? ActualPromoCatalogue { get; set; }
        public double? ActualPromoPOSMInClient { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoROIPercent { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoROIPercent { get; set; }
        public double? PlanPromoNetIncrementalNSV { get; set; }
        public double? PlanPromoIncrementalNSV { get; set; }
    }
}
