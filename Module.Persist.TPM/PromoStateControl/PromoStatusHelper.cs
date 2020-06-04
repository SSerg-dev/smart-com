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
        public static bool IsParametersChanged(Promo curPromo, Promo prevPromo, Guid? stateIdVP, Guid? stateIdTPR)
        {
            return ((prevPromo.MarsMechanicDiscount < curPromo.MarsMechanicDiscount) ||
                    (prevPromo.MarsMechanicId == stateIdVP && curPromo.MarsMechanicId == stateIdTPR) ||
                    (prevPromo.ProductHierarchy != curPromo.ProductHierarchy) ||
                    (prevPromo.StartDate != curPromo.StartDate) ||
                    (prevPromo.EndDate != curPromo.EndDate));
        }

        public static bool IsDispatchChanged(bool isCorrectDispatchDifference, Promo curPromo, Promo prevPromo)
        {
            return !isCorrectDispatchDifference && (prevPromo.DispatchesEnd != curPromo.DispatchesEnd || prevPromo.DispatchesStart != curPromo.DispatchesStart);
        }
    }
}
