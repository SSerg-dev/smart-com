using Core.Data;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class SavedPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset CreateDate { get; set; } = TimeHelper.Now();

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        [ForeignKey("ClientTree")]
        public int? ClientTreeId { get; set; }
        public ClientTree ClientTree { get; set; }

        public SavedPromoType SavedPromoType { get; set; }
        public ICollection<Promo> Promoes { get; set; }
    }
}
