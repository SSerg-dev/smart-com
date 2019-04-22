using Core.Data;
using Persist.Model;
using Persist.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class PromoStatusChange : IEntity<Guid> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public Guid? PromoId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? RoleId { get; set; }

        public Guid? StatusId { get; set; }

        public DateTimeOffset? Date { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        public Guid? RejectReasonId { get; set; }


        [ForeignKey("StatusId")]
        public virtual PromoStatus PromoStatus { get; set; }

        [ForeignKey("PromoId")]
        public virtual Promo Promo { get; set; }

        [ForeignKey("RejectReasonId")]
        public virtual RejectReason RejectReason { get; set; }

        [NotMapped]
        public String UserName { get; set; }

        [NotMapped]
        public String RoleName { get; set; }

        [NotMapped]
        public String StatusName { get; set; }

        [NotMapped]
        public String StatusColor { get; set; }

    }
}
