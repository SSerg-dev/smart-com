using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class PromoProductDataFlowModule : DataFlowModule
    {
        public List<PromoProductDataFlowSimpleModel> Collection { get; }
        public PromoProductDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PromoProduct>().AsNoTracking()
            .Select(x => new PromoProductDataFlowSimpleModel
            {
                Id = x.Id,
                PromoId = x.PromoId,
                ProductId = x.ProductId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class PromoProductDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public Guid? PromoId { get; set; }
            public Guid? ProductId { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
