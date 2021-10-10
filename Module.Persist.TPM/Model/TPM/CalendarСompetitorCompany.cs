using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CalendarCompetitorCompany : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_CalendarCompetitorCompany", 2, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_CalendarCompetitorCompany", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [StringLength(124)]
        [Index("Unique_CalendarCompetitorCompany", 1, IsUnique = true)]
        [Required]
        public string CompanyName { get; set; }
        [Index("Unique_CalendarCompetitorCompany", 0, IsUnique = true)]
        public Guid? CalendarCompetitorId { get; set; }

        [ForeignKey("CalendarCompetitorId")]
        public virtual CalendarCompetitor CalendarCompetitor { get; set; }
    }
}
