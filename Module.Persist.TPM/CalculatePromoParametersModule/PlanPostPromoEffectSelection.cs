using Module.Persist.TPM.Model.TPM;
using Persist;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanPostPromoEffectSelection
    {
        public static bool SelectPPEforPromoProduct(List<PromoProduct> promoProducts, Promo promo, DatabaseContext context)
        {
            ClientTree clientTree = context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == (int)promo.ClientTreeId);
            var filterProducts = promoProducts.Select(f => f.Product).Select(d => new { d.Brandsegtech, d.Size }).ToList();
            List<DurationRange> durationRanges = context.Set<DurationRange>().ToList();
            DurationRange durationRange = durationRanges.OrderBy(g => g.MinValue).FirstOrDefault(g => g.MinValue >= promo.PromoDuration);
            List<DiscountRange> discountRanges = context.Set<DiscountRange>().ToList();
            DiscountRange discountRange = discountRanges.OrderBy(g => g.MinValue).FirstOrDefault(g => g.MinValue >= promo.MarsMechanicDiscount);
            List<PlanPostPromoEffect> planPostPromoEffects = context.Set<PlanPostPromoEffect>()
                .Include(f => f.BrandTech)
                .Where(g => g.ClientTreeId == clientTree.Id &&
                filterProducts.Select(a => a.Brandsegtech).Contains(g.BrandTech.BrandTech_code) &&
                filterProducts.Select(a => a.Size).Contains(g.Size) &&
                g.DurationRangeId == durationRange.Id &&
                g.DiscountRangeId == discountRange.Id)
                .ToList();
            foreach (var promoProduct in promoProducts)
            {
                PlanPostPromoEffect planPostPromoEffect = planPostPromoEffects.FirstOrDefault(g => g.BrandTech.BrandTech_code == promoProduct.Product.Brandsegtech && g.Size == promoProduct.Product.Size);
                if (planPostPromoEffect != null)
                {
                    promoProduct.PlanProductPostPromoEffectW1 = planPostPromoEffect.PlanPostPromoEffectW1;
                    promoProduct.PlanProductPostPromoEffectW2 = planPostPromoEffect.PlanPostPromoEffectW2;
                }
                else
                {
                    promoProduct.PlanProductPostPromoEffectW1 = 0;
                    promoProduct.PlanProductPostPromoEffectW2 = 0;
                }

            }
            context.SaveChanges();
            return true;
        }
    }
}
