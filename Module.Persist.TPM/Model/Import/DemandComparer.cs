using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class DemandComparer : IEqualityComparer<Demand>
    {
        public bool Equals(Demand x, Demand y)
        {
            return x.Number.Equals(y.Number);
        }

        public int GetHashCode(Demand obj)
        {
            int result = 0;
            result ^= obj.Number.GetHashCode();

            return result;
        }
    }
}
