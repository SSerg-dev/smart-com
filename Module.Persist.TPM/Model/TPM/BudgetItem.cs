using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BudgetItem : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        [Index("Unique_BudgetItem", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_BudgetItem", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Index("Unique_BudgetItem", 1, IsUnique = true)]
        [Required]
        public string Name { get; set; }

        public string ButtonColor { get; set; }

        public string Description_ru { get; set; }

        [Index("Unique_BudgetItem", 2, IsUnique = true)]
        public Guid BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<BudgetSubItem> BudgetSubItems { get; set; }
        public ICollection<NonPromoEquipment> NonPromoEquipments { get; set; }
    }
}
