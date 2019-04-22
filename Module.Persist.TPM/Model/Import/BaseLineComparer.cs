using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class BaseLineComparer : IEqualityComparer<BaseLine>
    {
        public bool Equals(BaseLine x, BaseLine y)
        {
            return x.ProductId.Equals(y.ProductId) && x.ClientTreeId.Equals(y.ClientTreeId) &&
                   x.QTY.Equals(y.QTY) && x.Price.Equals(y.Price) && x.Type.Equals(y.Type);
        }

        public int GetHashCode(BaseLine obj)
        {
            int result = 0;

            result ^= obj.ProductId.GetHashCode();
            result ^= obj.ClientTree.GetHashCode();
            result ^= obj.QTY.GetHashCode();
            result ^= obj.Price.GetHashCode();
            result ^= obj.Type.GetHashCode();

            return result;
        }
    }
}
