using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class SimplePromoCOGSTI
    {
        public int? Number { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? ClientTreeId { get; set; }
        public int? ClientTreeObjectId { get; set; }
        public Guid? BrandTechId { get; set; }
        public string BrandTechName { get; set; }
        public string PromoStatusName { get; set; }
        public string ClientHierarchy { get; set; }
    }

    public class SimplePromoCOGS: SimplePromoCOGSTI
    {
        public SimplePromoCOGS(Promo promo)
        {
            Number = promo.Number;
            StartDate = promo.StartDate;
            EndDate = promo.EndDate;
            DispatchesStart = promo.DispatchesStart;
            DispatchesEnd = promo.DispatchesEnd;
            ClientTreeId = promo.ClientTreeKeyId;
            ClientTreeObjectId = promo.ClientTreeId;
            BrandTechId = promo.BrandTechId;
            BrandTechName = promo.BrandTech != null ? promo.BrandTech.BrandsegTechsub : String.Empty;
            PromoStatusName = promo.PromoStatus != null ? promo.PromoStatus.Name : String.Empty;
            ClientHierarchy = promo.ClientHierarchy;
        }
    }

    public class SimplePromoTradeInvestment : SimplePromoCOGSTI
    {
        public SimplePromoTradeInvestment(Promo promo)
        {
            Number = promo.Number;
            StartDate = promo.StartDate;
            EndDate = promo.EndDate;
            DispatchesStart = promo.DispatchesStart;
            DispatchesEnd = promo.DispatchesEnd;
            ClientTreeId = promo.ClientTreeKeyId;
            ClientTreeObjectId = promo.ClientTreeId;
            BrandTechId = promo.BrandTechId;
            BrandTechName = promo.BrandTech != null ? promo.BrandTech.BrandsegTechsub : String.Empty;
            PromoStatusName = promo.PromoStatus != null ? promo.PromoStatus.Name : String.Empty;
            ClientHierarchy = promo.ClientHierarchy;
        }
    }
}