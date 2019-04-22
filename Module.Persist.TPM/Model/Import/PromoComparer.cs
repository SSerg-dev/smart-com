using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class PromoComparer : IEqualityComparer<Promo>
    {
        public bool Equals(Promo x, Promo y)
        {
            return x.Name.Equals(y.Name); 
        }

        public int GetHashCode(Promo obj)
        {
            int result = 0;
            result ^= obj.Name.GetHashCode();

            return result;
        }
    }
}
