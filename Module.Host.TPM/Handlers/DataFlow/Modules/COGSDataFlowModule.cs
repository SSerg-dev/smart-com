using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class COGSDataFlowModule : DataFlowModule
    {
        public List<COGSDataFlowSimpleModel> Collection { get; }
        public COGSDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<COGS>().AsNoTracking()
            .Select(x => new COGSDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                ClientTreeId = x.ClientTreeId,
                ClientTreeObjectId = x.ClientTree.ObjectId,
                BrandTechId = x.BrandTechId,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();
        }

        public class COGSDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public int ClientTreeId { get; set; }
            public int ClientTreeObjectId { get; set; }
            public Guid? BrandTechId { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
            public bool Disabled { get; set; }
        }
    }
}