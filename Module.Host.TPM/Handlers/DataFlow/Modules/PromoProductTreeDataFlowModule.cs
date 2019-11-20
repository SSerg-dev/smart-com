using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class PromoProductTreeDataFlowModule : DataFlowModule
    {
        public List<PromoProductTreeDataFlowSimpleModel> Collection { get; }
        public PromoProductTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PromoProductTree>().AsNoTracking()
            .Select(x => new PromoProductTreeDataFlowSimpleModel
            {
                PromoId = x.PromoId,
                ProductTreeObjectId = x.ProductTreeObjectId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class PromoProductTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid PromoId { get; set; }
            public int ProductTreeObjectId { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
