using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class BrandTechDataFlowModule : DataFlowModule
    {
        public IEnumerable<BrandTechDataFlowSimpleModel> Collection { get; }
        public BrandTechDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<BrandTech>().AsNoTracking()
            .Select(x => new BrandTechDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                Name = x.Name
            })
            .ToList();
        }
        public class BrandTechDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public bool Disabled { get; set; }
            public string Name { get; set; }
        }
    }
}