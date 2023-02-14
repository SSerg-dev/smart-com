using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BudgetSubItem : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        [Index("Unique_BudgetSubItem", 3, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_BudgetSubItem", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Required]
        [Index("Unique_BudgetSubItem", 1, IsUnique = true)]
        public string Name { get; set; }

        public string Description_ru { get; set; }

        [Index("Unique_BudgetSubItem", 2, IsUnique = true)]
        public Guid BudgetItemId { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<BudgetSubItemClientTree> BudgetSubItemClientTrees { get; set; }
        public ICollection<PromoSupport> PromoSupports { get; set; }
    }
}
