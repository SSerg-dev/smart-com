using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CalendarСompetitorCompany : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_CalendarСompetitorCompany", 2, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_CalendarСompetitorCompany", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [StringLength(124)]
        [Index("Unique_CalendarСompetitorCompany", 1, IsUnique = true)]
        [Required]
        public string CompanyName { get; set; }
        [Index("Unique_CalendarСompetitorCompany", 0, IsUnique = true)]
        [Required]
        public Guid CalendarCompetitorId { get; set; }

        public virtual CalendarСompetitor CalendarСompetitor { get; set; }
    }
}
