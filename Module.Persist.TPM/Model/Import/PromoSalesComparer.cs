using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class PromoSalesComparer : IEqualityComparer<PromoSales>
    {
        public bool Equals(PromoSales x, PromoSales y)
        {
            return x.Number.Equals(y.Number) &&
                   x.BudgetItemId.Equals(y.BudgetItemId); 
        }

        public int GetHashCode(PromoSales obj)
        {
            int result = 0;
            result ^= obj.Number.GetHashCode();
            result ^= obj.BudgetItemId.GetHashCode();

            return result;
        }
    }
}
