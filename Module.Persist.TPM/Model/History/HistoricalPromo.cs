using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History {

    [AssociatedWith(typeof(Promo))]
    public class HistoricalPromo : BaseHistoricalEntity<System.Guid>
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public DateTimeOffset? LastChangedDateDemand { get; set; }
        public DateTimeOffset? LastChangedDateFinance { get; set; }
        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }
        public string InvoiceNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string ClientHierarchy { get; set; }
        public string ProductHierarchy { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public string OtherEventName { get; set; }
        public string EventName { get; set; }
        public string Comment { get; set; }
        public string CreatorLogin { get; set; }

        //event
        public string PromoEventName { get; set; }
        public string PromoEventDescription { get; set; }

        // Calculation
        /*public int? ShopperTi { get; set; }
        public int? MarketingTi { get; set; }
        public int? Branding { get; set; }
        public int? TotalCost { get; set; }
        public int? BTL { get; set; }
        public double? CostProduction { get; set; }
        public double? PlanUplift { get; set; }
        public int? PlanIncrementalLsv { get; set; }
        public int? PlanTotalPromoLsv { get; set; }
        public int? PlanPromoPostPromoEffectLSV { get; set; }
        public int? PlanRoi { get; set; }
        public int? PlanIncrementalNsv { get; set; }
        public int? PlanTotalPromoNsv { get; set; }
        public int? PlanIncrementalMac { get; set; }
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
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public int? PlanPromoROIPercent { get; set; }
        public int? PlanPromoNetUpliftPercent { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public int? ActualInStoreDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
        public double? ActualPromoTotalCost { get; set; }
        public double? ActualPromoNetIncrementalLSV { get; set; }
        public double? ActualPromoNetLSV { get; set; }
        public double? ActualPromoNetIncrementalMAC { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? ActualPromoNetIncrementalEarnings { get; set; }
        public int? ActualPromoNetROIPercent { get; set; }
        public int? ActualPromoNetUpliftPercent { get; set; }
        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoNetNSV { get; set; }
        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }

        // Promo Closure
        public int? FactShopperTi { get; set; }
        public double? FactMarketingTi { get; set; }
        public int? FactBranding { get; set; }
        public int? FactBTL { get; set; }
        public double? FactCostProduction { get; set; }
        public int? FactTotalCost { get; set; }
        public double? FactUplift { get; set; }
        public int? FactIncrementalLsv { get; set; }
        public int? FactTotalPromoLsv { get; set; }
        public int? ActualPromoPostPromoEffectLSV { get; set; }
        public int? FactRoi { get; set; }
        public int? FactIncrementalNsv { get; set; }
        public int? FactTotalPromoNsv { get; set; }
        public int? FactIncrementalMac { get; set; }*/

        // Calculation
        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoTIMarketing { get; set; }
        public double? PlanPromoBranding { get; set; }
        public double? PlanPromoCost { get; set; }
        public double? PlanPromoBTL { get; set; }
        public double? PlanPromoCostProduction { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }

        //необходимость полей в таком виде под вопросом
        public double? PlanPromoPostPromoEffectLSV { get; set; }
        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }
        //

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
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public double? PlanPromoNetROIPercent { get; set; }
        public double? PlanPromoNetUpliftPercent { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public double? ActualInStoreDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public double? PlanInStoreShelfPrice { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
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
        public double? PlanPromoNetNSV { get; set; }
        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }

        // Promo Closure
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }
        public double? ActualPromoBranding { get; set; }
        public double? ActualPromoBTL { get; set; }
        public double? ActualPromoCostProduction { get; set; }
        public double? ActualPromoCost { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? ActualPromoLSV { get; set; }

        //необходимость полей в таком виде под вопросом
        public double? ActualPromoPostPromoEffectLSV { get; set; }
        public double? ActualPromoPostPromoEffectLSVW1 { get; set; }
        public double? ActualPromoPostPromoEffectLSVW2 { get; set; }

        public double? ActualPromoROIPercent { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalMAC { get; set; }

        public bool IsOnInvoice { get; set; }
        public string BrandName { get; set; }
        public string BrandTechName { get; set; }
        public string ProductZREP { get; set; }
        public string PromoStatusName { get; set; }
        public string PromoStatusColor { get; set; }
        public string RejectReasonName { get; set; }
        public string MarsMechanicName { get; set; }
        public string PlanInstoreMechanicName { get; set; }
        public string ActualInStoreMechanicName { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public string PlanInstoreMechanicTypeName { get; set; }
        public string ActualInStoreMechanicTypeName { get; set; }
        public string MechanicComment { get; set; }
        public int? CalendarPriority { get; set; }
        public string Mechanic { get; set; }
        public string MechanicIA { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public double? PlanInstoreMechanicDiscount { get; set; }
        public string InstoreMechanicName { get; set; }
        public string InstoreMechanicTypeName { get; set; }
        public double? InstoreMechanicDiscount { get; set; }
        public int? Priority { get; set; }
        public string ColorDisplayName { get; set; }
        public string ColorSystemName { get; set; }
        public double DeviationCoefficient { get; set; }

        public bool? IsAutomaticallyApproved { get; set; }
        public bool? IsCMManagerApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }

        public bool? NeedRecountUplift { get; set; }

        public string ProductSubrangesList { get; set; }

        //
        public double? PlanPromoNetIncrementalBaseTI { get; set; }
        public double? PlanPromoNetIncrementalCOGS { get; set; }
        public double? PlanPromoNetBaseTI { get; set; }
        public double? PlanPromoNSV { get; set; }
        public double? ActualPromoLSVByCompensation { get; set; }
        public double? ActualPromoNetIncrementalBaseTI { get; set; }
        public double? ActualPromoNetIncrementalCOGS { get; set; }
        public double? ActualPromoNetBaseTI { get; set; }
        public double? ActualPromoNSV { get; set; }
        public bool? InOut { get; set; }
        // Id операции (по сути транзакции) для предотвращения дублирования
        public Guid? OperationId { get; set; }
        public bool? IsGrowthAcceleration { get; set; }
        public bool? IsApolloExport { get; set; }
        public double? InvoiceTotal { get; set; }
    }
}
