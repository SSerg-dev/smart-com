using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class ProductDataFlowModule : DataFlowModule
    {
        public List<ProductDataFlowSimpleModel> Collection { get; }
        public ProductDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<Product>().AsNoTracking()
            .Select(x => new ProductDataFlowSimpleModel
            {
            })
            .ToList();
        }
        public class ProductDataFlowSimpleModel : DataFlowSimpleModel
        {
        }
    }
}
