using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ExportQuery : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public String Type { get; set; }
        public String Text { get; set; }
        public String Parameters { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

    }
}
