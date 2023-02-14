using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSupportDMP : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public Guid? PromoSupportId { get; set; }
        public string ExternalCode { get; set; }
        public int Quantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
