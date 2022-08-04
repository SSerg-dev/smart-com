using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Module.Persist.TPM.Model.TPM
{
    public class BTL : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public int Number { get; set; }
        public double? PlanBTLTotal { get; set; }
        public double? ActualBTLTotal { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string InvoiceNumber { get; set; }

        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }

        public ICollection<BTLPromo> BTLPromoes { get; set; }
    }
}
