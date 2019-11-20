using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public class PromoDataFlowModule : DataFlowModule
    {
        public List<PromoDataFlowSimpleModel> Collection { get; }
        public PromoDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<Promo>().AsNoTracking()
            .Select(x => new PromoDataFlowSimpleModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ClientTreeId = x.ClientTreeId,
                ClientTreeKeyId = x.ClientTreeKeyId,
                DispatchesStart = x.DispatchesStart,
                DispatchesEnd = x.DispatchesEnd,
                InOut = x.InOut,
                PromoStatusSystemName = x.PromoStatus.SystemName,
                Number = x.Number,
                Disabled = x.Disabled,
                BrandTechId = x.BrandTechId
            })
            .ToList();
        }
        public class PromoDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
            public DateTimeOffset? DispatchesStart { get; set; }
            public DateTimeOffset? DispatchesEnd { get; set; }
            public int? ClientTreeId { get; set; }
            public int? ClientTreeKeyId { get; set; }
            public int? Number { get; set; }
            public string PromoStatusSystemName { get; set; }
            public bool? InOut { get; set; }
            public bool Disabled { get; set; }
            public Guid? BrandTechId { get; set; }
        }
    }
}
