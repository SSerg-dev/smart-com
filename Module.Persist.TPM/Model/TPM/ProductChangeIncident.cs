using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class ProductChangeIncident : IEntity<Guid> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
		public Guid RecalculatedPromoId { get; set; }
		public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? NotificationProcessDate { get; set; }
        public DateTimeOffset? RecalculationProcessDate { get; set; }
		public string AddedProductIds { get; set; }
		public string ExcludedProductIds { get; set; }

		public Boolean IsCreate { get; set; }
        public Boolean IsDelete { get; set; }
		public Boolean IsChecked { get; set; }
		public Boolean IsCreateInMatrix { get; set; }
		public Boolean IsDeleteInMatrix { get; set; }
		public Boolean IsRecalculated { get; set; }

		public virtual Product Product { get; set; }
    }
}
