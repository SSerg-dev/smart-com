using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class IncreaseBaseLineComparer : IEqualityComparer<IncreaseBaseLine>
    {
        public bool Equals(IncreaseBaseLine x, IncreaseBaseLine y)
        {
            return x.ProductId.Equals(y.ProductId) && x.DemandCode.Equals(y.DemandCode) && x.Type.Equals(y.Type);
        }

        public int GetHashCode(IncreaseBaseLine obj)
        {
            int result = 0;

            result ^= obj.ProductId.GetHashCode();
            result ^= obj.DemandCode.GetHashCode();
            result ^= obj.Type.GetHashCode();

            return result;
        }
    }
}
