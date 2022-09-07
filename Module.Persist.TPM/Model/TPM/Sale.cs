using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Sale : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public bool Disabled { get; set; }

        public DateTimeOffset? DeletedDate { get; set; }

        public Guid? BudgetId { get; set; }

        public Guid? BudgetItemId { get; set; }

        public Guid? PromoId { get; set; }

        public int? Plan { get; set; }

        public int? Fact { get; set; }
        public Promo Promo { get; set; }
        public Budget Budget { get; set; }
        public BudgetItem BudgetItem { get; set; }
    }
}
