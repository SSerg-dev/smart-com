using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class ClientTreeBrandTechDataFlowModule : DataFlowModule
     {
        public List<ClientTreeBrandTechDataFlowSimpleModel> Collection { get; }
        public ClientTreeBrandTechDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ClientTreeBrandTech>().AsNoTracking()
            .Select(x => new ClientTreeBrandTechDataFlowSimpleModel
            {
                Id = x.Id,
                ClientTreeId = x.ClientTreeId,
                BrandTechId = x.BrandTechId
            })
            .ToList();
        }
        public class ClientTreeBrandTechDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public int ClientTreeId { get; set; }
            public Guid BrandTechId { get; set; }
        }
    }
}
