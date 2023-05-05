using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoProductCorrectionPriceIncrease : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoProduct), TerminalEntityType = typeof(Promo), LookupPropertyName = nameof(Promo.Number))]
        [Display(Name = "Promo ID")]
        public int PromoNumber { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoProduct), TerminalEntityType = typeof(Product), LookupPropertyName = nameof(Product.ZREP))]
        [Display(Name = nameof(Product.ZREP))]
        public string ProductZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Plan Product Uplift Percent Corrected")]
        public double PlanProductUpliftPercentCorrected { get; set; }
    }
}
