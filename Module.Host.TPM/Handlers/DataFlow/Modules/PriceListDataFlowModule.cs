using System;
using System.Collections.Generic;
using System.Linq;
using Module.Persist.TPM.Model.TPM;
using Persist;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class PriceListDataFlowModule : DataFlowModule
    {
        public List<PriceListDataFlowSimpleModel> Collection { get; }
        public PriceListDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PriceList>().AsNoTracking()
            .Select(x => new PriceListDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();
        }
        public class PriceListDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
