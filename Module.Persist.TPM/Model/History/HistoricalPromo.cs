using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History {
    [AssociatedWith(typeof(Promo))]
    public class HistoricalPromo : BaseHistoricalEntity<System.Guid> {
        public Guid? ClientId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? BrandTechId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? PromoStatusId { get; set; }
        public Guid? MarsMechanicId { get; set; }
        public Guid? MarsMechanicTypeId { get; set; }
        public Guid? InstoreMechanicId { get; set; }
        public Guid? InstoreMechanicTypeId { get; set; }
        public Guid? CreatorId { get; set; }
        public int? ClientTreeId { get; set; }
        public Guid? ColorId { get; set; }
        public int? BaseClientTreeId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public string BaseClientTreeIds { get; set; }
        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }
        public string InvoiceNumber { get; set; }

        public DateTimeOffset? LastApprovedDate { get; set; }

        //MarsDates
        public string MarsStartDate { get; set; }
        public string MarsEndDate { get; set; }
        public string MarsDispatchesStart { get; set; }
        public string MarsDispatchesEnd { get; set; }

        public string ClientHierarchy { get; set; }
        public string ProductHierarchy { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public string EventName { get; set; }
        public string Comment { get; set; }

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
        public int? PlanPostPromoEffect { get; set; }
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
        public int? FactPostPromoEffect { get; set; }
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
        public double? PlanPostPromoEffect { get; set; }
        public double? PlanPostPromoEffectW1 { get; set; }
        public double? PlanPostPromoEffectW2 { get; set; }
        //

        public int? PlanPromoROIPercent { get; set; }
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
        public int? PlanPromoNetROIPercent { get; set; }
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
        public int? FactPostPromoEffect { get; set; }
        public double? FactPostPromoEffectW1 { get; set; }
        public double? FactPostPromoEffectW2 { get; set; }

        public int? ActualPromoROIPercent { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalMAC { get; set; }

        public string BrandName { get; set; }
        public string BrandTechName { get; set; }
        public string ProductZREP { get; set; }
        public string PromoStatusName { get; set; }
        public string MarsMechanicName { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public int? MarsMechanicDiscount { get; set; }
        public string InstoreMechanicName { get; set; }
        public string InstoreMechanicTypeName { get; set; }
        public int? InstoreMechanicDiscount { get; set; }
        public int? Priority { get; set; }
        public string ColorSystemName { get; set; }

        public bool? IsAutomaticallyApproved { get; set; }
        public bool? IsCustomerMarketingApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }
        public string ProductTreeObjectIds { get; set; }

        //Поля для отчёта ROIReport
        public string Client1LevelName { get; set; }
        public string Client2LevelName { get; set; }
        public string ClientName { get; set; }
        public string ProductSubrangesList { get; set; }
    }
}
