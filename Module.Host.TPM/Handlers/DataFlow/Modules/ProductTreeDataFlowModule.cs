using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class ProductTreeDataFlowModule : DataFlowModule
    {
        public List<ProductTreeDataFlowSimpleModel> Collection { get; }
        public ProductTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ProductTree>().AsNoTracking()
            .Select(x => new ProductTreeDataFlowSimpleModel
            {
                Id = x.Id,
                EndDate = x.EndDate
            })
            .ToList();
        }
        public class ProductTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public DateTimeOffset? EndDate { get; set; }
            public int Id { get; set; }
            public int ObjectId { get; set; }
        }
    }
}
