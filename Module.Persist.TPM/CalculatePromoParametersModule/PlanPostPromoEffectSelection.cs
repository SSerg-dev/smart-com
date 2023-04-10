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
            List<PlanPostPromoEffect> planPostPromoEffects = context.Set<PlanPostPromoEffect>()
                .Include(f => f.BrandTech)
                .Where(g => g.ClientTreeId == clientTree.Id && filterProducts.Select(a=>a.Brandsegtech).Contains(g.BrandTech.BrandTech_code) && filterProducts.Select(a => a.Size).Contains(g.Size))
                .ToList();
            foreach (var promoProduct in promoProducts)
            {
                PlanPostPromoEffect planPostPromoEffect = planPostPromoEffects.FirstOrDefault(g => g.BrandTech.BrandTech_code == promoProduct.Product.Brandsegtech && g.Size == promoProduct.Product.Size);
                if (planPostPromoEffect != null)
                {
                    promoProduct.PlanProductPostPromoEffectW1 = (double)planPostPromoEffect.PlanPostPromoEffectW1;
                    promoProduct.PlanProductPostPromoEffectW2 = (double)planPostPromoEffect.PlanPostPromoEffectW2;
                }
                
            }
            return true;
        }
    }
}
