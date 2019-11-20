using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class AssortmentMatrixDataFlowModule : DataFlowModule
    {
        public List<AssortmentMatrixDataFlowSimpleModel> Collection { get; }
        public AssortmentMatrixDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<AssortmentMatrix>().AsNoTracking()
            .Select(x => new AssortmentMatrixDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                ClientTreeId = x.ClientTreeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();
        }
        public class AssortmentMatrixDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public bool Disabled { get; set; }
            public int ClientTreeId { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
        }
    }
}
