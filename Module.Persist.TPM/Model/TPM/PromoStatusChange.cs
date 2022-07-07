using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoStatusChange : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public Guid? RoleId { get; set; }

        public DateTimeOffset? Date { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        [NotMapped]
        public string StatusColor { get; set; }

        public Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual PromoStatus PromoStatus { get; set; }

        public Guid? PromoId { get; set; }
        [ForeignKey("PromoId")]
        public virtual Promo Promo { get; set; }

        public Guid? RejectReasonId { get; set; }
        [ForeignKey("RejectReasonId")]
        public virtual RejectReason RejectReason { get; set; }
    }
}
