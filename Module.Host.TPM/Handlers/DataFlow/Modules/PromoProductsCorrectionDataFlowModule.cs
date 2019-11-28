using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class PromoProductsCorrectionDataFlowModule : DataFlowModule
    {
        public List<PromoProductsCorrectionDataFlowSimpleModel> Collection { get; }
        public PromoProductsCorrectionDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PromoProductsCorrection>().AsNoTracking()
            .Select(x => new PromoProductsCorrectionDataFlowSimpleModel
            {
                Id = x.Id,
                PromoProductId = x.PromoProductId,
                Disabled = x.Disabled,
            })
            .ToList();
        }
        public class PromoProductsCorrectionDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public Guid PromoProductId { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
