using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class EventClientTree : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_EventClientTree", 1, IsUnique = true)]
        public Guid EventId { get; set; }
        [Index("Unique_EventClientTree", 2, IsUnique = true)]
        public int ClientTreeId { get; set; }

        public virtual Event Event { get; set; }
        public virtual ClientTree ClientTree { get; set; }
    }
}
