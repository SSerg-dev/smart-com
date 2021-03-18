using System;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class SimplePromoRATIShopper
    {
        public int? Number { get; set; }
        public int? Year { get; set; }
        public int? ClientTreeId { get; set; }
        public int? ClientTreeObjectId { get; set; }
        public string PromoStatusName { get; set; }
        public string ClientHierarchy { get; set; }

        public SimplePromoRATIShopper(Promo promo)
        {
            Number = promo.Number;
            Year = promo.BudgetYear;
            ClientTreeId = promo.ClientTreeKeyId;
            ClientTreeObjectId = promo.ClientTreeId;
            PromoStatusName = promo.PromoStatus != null ? promo.PromoStatus.Name : String.Empty;
            ClientHierarchy = promo.ClientHierarchy;
        }
    }
}
