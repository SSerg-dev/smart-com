using Module.Persist.TPM.Model.TPM;
using System.Collections.Generic;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class OneLoadModel
    {
        public List<ClientTree> ClientTrees { get; set; }
        public List<BrandTech> BrandTeches { get; set; }
        public List<ProductTree> ProductTrees { get; set; }
        public List<Product> Products { get; set; }
        public List<TradeInvestment> TradeInvestments { get; set; }
        public List<COGS> COGSs { get; set; }
        public List<PlanCOGSTn> PlanCOGSTns { get; set; }
    }
}
