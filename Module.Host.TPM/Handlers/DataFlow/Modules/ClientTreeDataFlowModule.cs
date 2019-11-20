using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
     public class ClientTreeDataFlowModule : DataFlowModule
     {
        public List<ClientTreeDataFlowSimpleModel> Collection { get; }
        public ClientTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ClientTree>().AsNoTracking()
            .Select(x => new ClientTreeDataFlowSimpleModel
            {
                Id = x.Id,
                ObjectId = x.ObjectId,
                EndDate = x.EndDate,
                Type = x.Type,
                ParentId = x.parentId,
                DemandCode = x.DemandCode
            })
            .ToList();
        }
        public class ClientTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public DateTimeOffset? EndDate { get; set; }
            public int Id { get; set; }
            public int ObjectId { get; set; }
            public int ParentId { get; set; }
            public string Type { get; set; }
            public string DemandCode { get; set; }
        }
    }
}
