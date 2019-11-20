using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class ChangesIncidentDataFlowModule : DataFlowModule
    {
        public List<ChangesIncidentDataFlowSimpleModel> Collection { get; }
        public ChangesIncidentDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ChangesIncident>().AsNoTracking()
            .Select(x => new ChangesIncidentDataFlowSimpleModel
            {
                Id = x.Id,
                DirectoryName = x.DirectoryName,
                ItemId = x.ItemId,
                ProcessDate = x.ProcessDate
            })
            .ToList();
        }
        public class ChangesIncidentDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public string DirectoryName { get; set; }
            public string ItemId { get; set; }
            public DateTimeOffset? ProcessDate { get; set; }
        }
    }
}
