using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class AssortmentMatrix : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool Disabled { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Number { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }


        public int ClientTreeId { get; set; }
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ClientTree ClientTree { get; set; }

        public virtual AssortmentMatrix2Plu Plu { get; set; }


    }
}
