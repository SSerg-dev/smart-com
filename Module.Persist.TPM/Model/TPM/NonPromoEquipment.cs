using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class NonPromoEquipment : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        [Index("Unique_NonPromoEquipment", 3, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_NonPromoEquipment", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]       
        [Required]
        [Index("Unique_NonPromoEquipment", 1, IsUnique = true)]
        public string EquipmentType { get; set; }

        public string Description_ru { get; set; }
    }
}
