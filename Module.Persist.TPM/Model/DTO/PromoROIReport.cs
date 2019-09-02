using System;
using Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Persist;
using System.Linq;
using Newtonsoft.Json;

namespace Module.Persist.TPM.Model.DTO
{
    public class PromoROIReport : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public bool? InOut { get; set; }
        public int? ClientTreeId { get; set; }
        public int? ClientTreeKeyId { get; set; }

        public int? BaseClientTreeId { get; set; }
        [StringLength(400)]
        public string BaseClientTreeIds { get; set; }
        public bool? NeedRecountUplift { get; set; }

        public DateTimeOffset? LastApprovedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Number { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string ClientHierarchy { get; set; }
        public string ProductHierarchy { get; set; }
        [StringLength(255)]
        public string MechanicComment { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public double? PlanInstoreMechanicDiscount { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public DateTimeOffset? LastChangedDateDemand { get; set; }
        public DateTimeOffset? LastChangedDateFinance { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }
        public string InvoiceNumber { get; set; }
        [StringLength(20)]
        public string Mechanic { get; set; }
        [StringLength(20)]
        public string MechanicIA { get; set; }
        [StringLength(15)]
        public string MarsStartDate { get; set; }
        [StringLength(15)]
        public string MarsEndDate { get; set; }
        [StringLength(15)]
        public string MarsDispatchesStart { get; set; }
        [StringLength(15)]
        public string MarsDispatchesEnd { get; set; }
        [StringLength(255)]
        public string OtherEventName { get; set; }
        public string EventName { get; set; }
        public string PromoStatusName { get; set; }
        public string PlanInstoreMechanicName { get; set; }
        public string PlanInstoreMechanicTypeName { get; set; }
        public int? CalendarPriority { get; set; }
        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoTIMarketing { get; set; }
        public double? PlanPromoBranding { get; set; }
        public double? PlanPromoCost { get; set; }
        public double? TIBasePercent { get; set; }
        public double? PlanPromoBTL { get; set; }
        public double? PlanPromoCostProduction { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }
        public double? PlanPromoPostPromoEffectLSV { get; set; }
        public double? PlanPromoROIPercent { get; set; }
        public double? PlanPromoIncrementalNSV { get; set; }
        public double? PlanPromoNetIncrementalNSV { get; set; }
        public double? PlanPromoIncrementalMAC { get; set; }
        public double? PlanPromoXSites { get; set; }
        public double? PlanPromoCatalogue { get; set; }
        public double? PlanPromoPOSMInClient { get; set; }
        public double? PlanPromoCostProdXSites { get; set; }
        public double? PlanPromoCostProdCatalogue { get; set; }
        public double? PlanPromoCostProdPOSMInClient { get; set; }
        public double? ActualPromoXSites { get; set; }
        public double? ActualPromoCatalogue { get; set; }
        public double? ActualPromoPOSMInClient { get; set; }
        public double? ActualPromoCostProdXSites { get; set; }
        public double? ActualPromoCostProdCatalogue { get; set; }
        public double? ActualPromoCostProdPOSMInClient { get; set; }
        public double? PlanPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalBaseTI { get; set; }
        public double? PlanPromoNetIncrementalBaseTI { get; set; }
        public double? COGSPercent { get; set; }
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoNetIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public double? PlanPromoNetROIPercent { get; set; }
        public double? PlanPromoNetUpliftPercent { get; set; }
        public string ActualInStoreMechanicName { get; set; }
        public string ActualInStoreMechanicTypeName { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public double? ActualInStoreDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public double? PlanInStoreShelfPrice { get; set; }
        public double? PCPrice { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoNetIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
        public double? ActualPromoNetIncrementalCOGS { get; set; }
        public double? ActualPromoTotalCost { get; set; }
        public double? ActualPromoNetIncrementalLSV { get; set; }
        public double? ActualPromoNetLSV { get; set; }
        public double? ActualPromoNetIncrementalMAC { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? ActualPromoNetIncrementalEarnings { get; set; }
        public double? ActualPromoNetROIPercent { get; set; }
        public double? ActualPromoNetUpliftPercent { get; set; }
        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoNetBaseTI { get; set; }
        public double? PlanPromoNSV { get; set; }
        public double? PlanPromoNetNSV { get; set; }
        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoNetBaseTI { get; set; }
        public double? ActualPromoNSV { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }
        public double? ActualPromoBranding { get; set; }
        public double? ActualPromoBTL { get; set; }
        public double? ActualPromoCostProduction { get; set; }
        public double? ActualPromoCost { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? ActualPromoLSVByCompensation { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? ActualPromoPostPromoEffectLSVW1 { get; set; }
        public double? ActualPromoPostPromoEffectLSVW2 { get; set; }
        public double? ActualPromoPostPromoEffectLSV { get; set; }
        public double? ActualPromoROIPercent { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalMAC { get; set; }
        public bool? IsAutomaticallyApproved { get; set; }
        public bool? IsCMManagerApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }
        [StringLength(255)]
        public string Client1LevelName { get; set; }
        [StringLength(255)]
        public string Client2LevelName { get; set; }
        public string ClientName { get; set; }
        public string BrandName { get; set; }
        public string TechnologyName { get; set; }

        public string ProductSubrangesList { get; set; }
        public string MarsMechanicName { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public string ProductTreeObjectIds { get; set; }
        public bool? Calculating { get; set; }
        public string BlockInformation { get; set; }
    }
}
