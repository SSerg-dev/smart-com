using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class IncrementalPromoDataFlowModule : DataFlowModule
    {
        public List<IncrementalPromoDataFlowSimpleModel> Collection { get; }
        public IncrementalPromoDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<IncrementalPromo>().AsNoTracking()
            .Select(x => new IncrementalPromoDataFlowSimpleModel
            {
                Id = x.Id,
                PromoId = x.PromoId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class IncrementalPromoDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public Guid PromoId { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
