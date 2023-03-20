using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class EventClientTree : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Index("Unique_EventClientTree", 1, IsUnique = true)]
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        [Index("Unique_EventClientTree", 2, IsUnique = true)]
        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
