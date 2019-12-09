using Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Module.Persist.TPM.Model.TPM {
    public class NonPromoSupport : IEntity<Guid>, IDeactivatable {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public int Number { get; set; }
        public int ClientTreeId { get; set; }
		public Guid? BrandTechId { get; set; }
		public Guid? NonPromoEquipmentId { get; set; }
		public int? PlanQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public double? PlanCostTE { get; set; }
        public double? ActualCostTE { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string UserTimestamp { get; set; }
        public string AttachFileName { get; set; }
        public string BorderColor { get; set; }
        public string InvoiceNumber { get; set; }


        public virtual ClientTree ClientTree { get; set; }
		public virtual BrandTech BrandTech { get; set; }
		public virtual NonPromoEquipment NonPromoEquipment { get; set; }
    }
}
