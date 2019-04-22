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
        public string EAN { get; set; }
        public double? PlanProductQty { get; set; }
        public int? PlanProductPCQty { get; set; }
        public double? PlanProductLSV { get; set; }
        public double? PlanProductPCLSV { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
        public double? PlanProductBaselineQty { get; set; }
        public double? ProductBaselinePrice { get; set; }
        public double? ProductBaselinePCPrice { get; set; }
        public double? PlanProductUplift { get; set; }
        public int? ActualProductPCQty { get; set; }
        public double? ActualProductQty { get; set; }
        public string ActualProductUOM { get; set; }       
        public double? ActualProductSellInPrice { get; set; }
        public double? ActualProductSellInDiscount { get; set; }        
        public double? ActualProductShelfPrice { get; set; }
        public double? ActualProductShelfDiscount { get; set; }
        public double? ActualProductPCLSV { get; set; }
        public double? ActualPromoShare { get; set; }
        public double? ActualProductUplift { get; set; }
        public double? ActualProductIncrementalPCQty { get; set; }
        public double? ActualProductIncrementalPCLSV { get; set; }
        public double? ActualProductIncrementalLSV { get; set; }
        public double? PlanPostPromoEffectLSVW1 { get; set; }
        public double? PlanPostPromoEffectLSVW2 { get; set; }
        public double? PlanPostPromoEffectLSV { get; set; }
        public double? ActualPostPromoEffectLSVW1 { get; set; }
        public double? ActualPostPromoEffectLSVW2 { get; set; }
        public double? ActualPostPromoEffectLSV { get; set; }
        public double? PlanProductIncrementalQty { get; set; }
        public double? PlanProductUpliftPercent { get; set; }
        public double? ActualProductLSV { get; set; }
        public double? ActualProductPostPromoEffectQty { get; set; }
        public double? PlanProductPostPromoEffectQty { get; set; }
    }
}
