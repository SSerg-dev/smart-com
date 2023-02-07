using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class NonPromoSupportDMP : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public Guid? NonPromoSupportId { get; set; }
        public string ExternalCode { get; set; }
        public int Quantity { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
