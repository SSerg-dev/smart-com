using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.PromoStateControl
{
    static class PromoStatusHelper
    {
        public static bool IsParametersChanged(Promo curPromo, Promo prevPromo, IEnumerable<Guid> stateIdVP, IEnumerable<Guid> stateIdTPR)
        {
            return ((prevPromo.MarsMechanicDiscount < curPromo.MarsMechanicDiscount) ||
                    (stateIdVP.Any(id => id == prevPromo.MarsMechanicId) && stateIdTPR.Any(id => id == curPromo.MarsMechanicId)) ||
                    (prevPromo.ProductHierarchy != curPromo.ProductHierarchy) ||
                    (prevPromo.StartDate != curPromo.StartDate) ||
                    (prevPromo.EndDate != curPromo.EndDate) ||
                    (prevPromo.IsGrowthAcceleration != curPromo.IsGrowthAcceleration));
        }

        public static bool IsDispatchChanged(bool isCorrectDispatchDifference, Promo curPromo, Promo prevPromo)
        {
            return !isCorrectDispatchDifference && (prevPromo.DispatchesEnd != curPromo.DispatchesEnd || prevPromo.DispatchesStart != curPromo.DispatchesStart);
        }
    }
}
