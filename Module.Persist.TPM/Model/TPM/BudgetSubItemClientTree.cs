using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BudgetSubItemClientTree : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
        public Guid BudgetSubItemId { get; set; }
        public virtual BudgetSubItem BudgetSubItem { get; set; }
    }
}
