using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoProduct))]
    public class HistoricalPromoProduct : BaseHistoricalEntity<System.Guid>
    {
        public Guid PromoId { get; set; }
        public Guid ProductId { get; set; }
        public string ZREP { get; set; }
        public string EAN_Case { get; set; }
        public string EAN_PC { get; set; }
        public string PluCode { get; set; }
        public double? PlanProductCaseQty { get; set; }
        public int? PlanProductPCQty { get; set; }
        public double? PlanProductCaseLSV { get; set; }
        public double? PlanProductPCLSV { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
        public double? PlanProductBaselineCaseQty { get; set; }
        public double? ProductBaselinePrice { get; set; }
        public double? PlanProductPCPrice { get; set; }
        public int? ActualProductPCQty { get; set; }
        public double? ActualProductCaseQty { get; set; }
        public string ActualProductUOM { get; set; }       
        public double? ActualProductSellInPrice { get; set; }
        public double? ActualProductPCLSV { get; set; }
        public double? ActualProductUpliftPercent { get; set; }
        public double? ActualProductIncrementalPCQty { get; set; }
        public double? ActualProductIncrementalPCLSV { get; set; }
        public double? ActualProductIncrementalLSV { get; set; }
        public double? PlanProductPostPromoEffectLSVW1 { get; set; }
        public double? PlanProductPostPromoEffectLSVW2 { get; set; }
        public double? PlanProductPostPromoEffectLSV { get; set; }
        public double? ActualProductPostPromoEffectLSV { get; set; }
        public double? PlanProductIncrementalCaseQty { get; set; }
        public double? PlanProductUpliftPercent { get; set; }
        public double? ActualProductLSV { get; set; }
        public double? ActualProductPostPromoEffectQty { get; set; }
        public double? PlanProductPostPromoEffectQty { get; set; }

        public double? PlanProductPostPromoEffectQtyW1 { get; set; }
        public double? PlanProductPostPromoEffectQtyW2 { get; set; }
        public double? ActualProductPostPromoEffectQtyW1 { get; set; }
        public double? ActualProductPostPromoEffectQtyW2 { get; set; }
        public double? ActualProductLSVByCompensation { get; set; }
    }
}
