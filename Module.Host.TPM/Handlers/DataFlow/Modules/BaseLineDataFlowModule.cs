using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class BaseLineDataFlowModule : DataFlowModule
    {
        public IEnumerable<BaseLineDataFlowSimpleModel> Collection { get; }
        public BaseLineDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<BaseLine>().AsNoTracking()
            .Select(x => new BaseLineDataFlowSimpleModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                ProductId = x.ProductId,
                DemandCode = x.DemandCode
            })
            .ToList();
        }
        public class BaseLineDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public Guid ProductId { get; set; }
            public string DemandCode { get; set; }
        }
    }
}
