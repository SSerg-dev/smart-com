using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO {
    public class PromoROIReport : IEntity<Guid> {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int? Number { get; set; }

        public string Client1Level { get; set; }
        public string Client2Level { get; set; }
        public string Client3Level { get; set; }

        public string Product1Level { get; set; }
        public string Product2Level { get; set; }
        public string Product3Level { get; set; }

        public string MarsMechanicName { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public int? MarsMechanicDiscount { get; set; }
        public string MechanicComment { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? PromoDuration { get; set; }

        public int? DispatchDuration { get; set; }

        public string EventName { get; set; }
        public string PromoStatusName { get; set; }

        public string PlanInstoreMechanicName { get; set; }
        public string PlanInstoreMechanicTypeName { get; set; }
        public int? PlanInstoreMechanicDiscount { get; set; }

        public double? PlanPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoTIMarketing { get; set; }
        public double? PlanPromoXSites { get; set; }
        public double? PlanPromoCatalogue { get; set; }
        public double? PlanPromoPOSMInClient { get; set; }
        public double? PlanPromoBranding { get; set; }
        public double? PlanPromoBTL { get; set; }
        public double? PlanPromoCostProduction { get; set; }
        public double? PlanPromoCostProdXSites { get; set; }
        public double? PlanPromoCostProdCatalogue { get; set; }
        public double? PlanPromoCostProdPOSMInClient { get; set; }
        public double? PlanPromoCost { get; set; }
        public double? PlanPromoIncrementalBaseTI { get; set; }

        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPostPromoEffectW1 { get; set; }
        public double? PlanPostPromoEffectW2 { get; set; }
        public double? PlanPostPromoEffect { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }

        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoNetNSV { get; set; }

        public double? PlanPromoIncrementalNSV { get; set; }
        public double? PlanPromoNetIncrementalNSV { get; set; }
        public double? PlanPromoIncrementalMAC { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public int? PlanPromoROIPercent { get; set; }
        public int? PlanPromoNetROIPercent { get; set; }
        public int? PlanPromoNetUpliftPercent { get; set; }

        public string ActualInStoreMechanicName { get; set; }
        public string ActualInStoreMechanicTypeName { get; set; }
        public int? ActualInStoreMechanicDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }

        public double? ActualPromoProdXSites { get; set; }
        public double? ActualPromoProdCatalogue { get; set; }
        public double? ActualPromoProdPOSMInClient { get; set; }

        public double? ActualPromoXSites { get; set; }
        public double? ActualPromoCatalogue { get; set; }
        public double? ActualPromoPOSMInClient { get; set; }

        public double? ActualPromoBranding { get; set; }
        public double? ActualPromoBTL { get; set; }
        public double? ActualPromoCostProduction { get; set; }

        public double? ActualPromoCostProdXSites { get; set; }
        public double? ActualPromoCostProdCatalogue { get; set; }
        public double? ActualPromoCostProdPOSMInClient { get; set; }

        public double? ActualPromoCost { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
        public double? ActualPromoTotalCost { get; set; }

        public double? FactPostPromoEffectW1 { get; set; }
        public double? FactPostPromoEffectW2 { get; set; }
        public int? FactPostPromoEffect { get; set; }
        public double? ActualPromoNetIncrementalLSV { get; set; }

        public double? ActualPromoNetLSV { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }

        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }

        public double? ActualPromoIncrementalMAC { get; set; }
        public double? ActualPromoNetIncrementalMAC { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? ActualPromoNetIncrementalEarnings { get; set; }
        public int? ActualPromoROIPercent { get; set; }
        public int? ActualPromoNetROIPercent { get; set; }
        public int? ActualPromoNetUpliftPercent { get; set; }
    }
}
