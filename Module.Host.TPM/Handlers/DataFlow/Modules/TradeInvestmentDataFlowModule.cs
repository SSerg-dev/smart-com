using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class TradeInvestmentDataFlowModule : DataFlowModule
    {
        public List<TradeInvestmentDataFlowSimpleModel> Collection { get; }
        public TradeInvestmentDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<TradeInvestment>().AsNoTracking()
            .Select(x => new TradeInvestmentDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                ClientTreeId = x.ClientTreeId,
                ClientTreeObjectId = x.ClientTree.ObjectId,
                BrandTechId = x.BrandTechId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TIType = x.TIType,
                TISubType = x.TISubType
            })
            .ToList();
        }
        public class TradeInvestmentDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public int ClientTreeId { get; set; }
            public int ClientTreeObjectId { get; set; }
            public Guid? BrandTechId { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
            public bool Disabled { get; set; }
            public string TIType { get; set; }
            public string TISubType { get; set; }
        }
    }
}