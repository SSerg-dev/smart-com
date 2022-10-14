using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSupport : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public int Number { get; set; }
        public int? PlanQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public double? PlanCostTE { get; set; }
        public double? ActualCostTE { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double? PlanProdCostPer1Item { get; set; }
        public double? ActualProdCostPer1Item { get; set; }
        public double? PlanProdCost { get; set; }
        public double? ActualProdCost { get; set; }
        public string UserTimestamp { get; set; }
        public string AttachFileName { get; set; }
        public string BorderColor { get; set; }
        public string PONumber { get; set; }
        public string InvoiceNumber { get; set; }
        public bool OffAllocation { get; set; }

        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
        public Guid? BudgetSubItemId { get; set; }
        public virtual BudgetSubItem BudgetSubItem { get; set; }

        public ICollection<PromoSupportPromo> PromoSupportPromo { get; set; }
    }
}
