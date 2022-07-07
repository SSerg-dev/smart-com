using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class ProductChangeIncident : IEntity<Guid> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

		public Guid RecalculatedPromoId { get; set; }
		public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? NotificationProcessDate { get; set; }
        public DateTimeOffset? RecalculationProcessDate { get; set; }
		public string AddedProductIds { get; set; }
		public string ExcludedProductIds { get; set; }

		public bool IsCreate { get; set; }
        public bool IsDelete { get; set; }
		public bool IsChecked { get; set; }
		public bool IsCreateInMatrix { get; set; }
		public bool IsDeleteInMatrix { get; set; }
		public bool IsRecalculated { get; set; }

		public bool Disabled { get; set; }

		public Guid ProductId { get; set; }
		public virtual Product Product { get; set; }
    }
}
