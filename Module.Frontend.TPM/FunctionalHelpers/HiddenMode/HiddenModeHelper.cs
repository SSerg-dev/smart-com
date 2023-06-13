using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.FunctionalHelpers.HiddenMode
{
    public static class HiddenModeHelper
    {
        public static List<Promo> HidePromoes(DatabaseContext Context, List<Promo> promoes)
        {
            foreach (Promo promo in promoes)
            {
                promo.TPMmode = TPMmode.Hidden;
                foreach (PromoProductTree promoProductTree in promo.PromoProductTrees)
                {
                    promoProductTree.TPMmode = TPMmode.Hidden;
                }
                foreach (BTLPromo bTLPromo in promo.BTLPromoes)
                {
                    bTLPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (IncrementalPromo incrementalPromo in promo.IncrementalPromoes)
                {
                    incrementalPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (PromoSupportPromo supportPromo in promo.PromoSupportPromoes)
                {
                    supportPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (PromoProduct promoProduct in promo.PromoProducts)
                {
                    promoProduct.TPMmode = TPMmode.Hidden;
                    foreach (PromoProductsCorrection promoProductsCorrection in promoProduct.PromoProductsCorrections)
                    {
                        promoProductsCorrection.TPMmode = TPMmode.Hidden;
                    }
                }
            }

            Context.SaveChanges();
            return promoes;
        }
        public static List<Promo> SetCurrentPromoes(DatabaseContext Context, List<Promo> promoes)
        {
            foreach (Promo promo in promoes)
            {
                promo.TPMmode = TPMmode.Current;
                foreach (PromoProductTree promoProductTree in promo.PromoProductTrees)
                {
                    promoProductTree.TPMmode = TPMmode.Current;
                }
                foreach (BTLPromo bTLPromo in promo.BTLPromoes)
                {
                    bTLPromo.TPMmode = TPMmode.Current;
                }
                foreach (IncrementalPromo incrementalPromo in promo.IncrementalPromoes)
                {
                    incrementalPromo.TPMmode = TPMmode.Current;
                }
                foreach (PromoSupportPromo supportPromo in promo.PromoSupportPromoes)
                {
                    supportPromo.TPMmode = TPMmode.Current;
                }
                foreach (PromoProduct promoProduct in promo.PromoProducts)
                {
                    promoProduct.TPMmode = TPMmode.Current;
                    foreach (PromoProductsCorrection promoProductsCorrection in promoProduct.PromoProductsCorrections)
                    {
                        promoProductsCorrection.TPMmode = TPMmode.Current;
                    }
                }
            }

            Context.SaveChanges();
            return promoes;
        }
    }
}
