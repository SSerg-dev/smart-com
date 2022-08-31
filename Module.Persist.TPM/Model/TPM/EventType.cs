using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class EventType : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("EventType_index", 1, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("EventType_index", 2, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [StringLength(255)]
        [Index("EventType_index", 3, IsUnique = true)]
        [Required]
        public string Name { get; set; }
        public bool National { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
