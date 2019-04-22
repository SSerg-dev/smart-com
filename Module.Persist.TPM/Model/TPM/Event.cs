using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Event : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("Event_index", 1, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Event_index", 2, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [StringLength(255)]
        [Index("Event_index", 3, IsUnique = true)]
        [Required]
        public string Name { get; set; }

        public int Year { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
    }
}
