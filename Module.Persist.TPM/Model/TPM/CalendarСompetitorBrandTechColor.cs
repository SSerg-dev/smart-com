using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CalendarCompetitorBrandTechColor : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; } = Guid.NewGuid();
        [Index("Unique_CalendarCompetitorBrandTechColor", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_CalendarCompetitorBrandTechColor", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_CalendarCompetitorBrandTechColor", 0, IsUnique = true)]
        public Guid? CalendarCompetitorId { get; set; }
        [Index("Unique_CalendarCompetitorBrandTechColor", 1, IsUnique = true)]
        [Required]
        public Guid CalendarCompetitorCompanyId { get; set; }
        [StringLength(124)]
        [Index("Unique_CalendarCompetitorBrandTechColor", 2, IsUnique = true)]
        [Required]
        public string BrandTech { get; set; }
        [StringLength(24)]
        [Required]
        public string Color { get; set; }

        [ForeignKey("CalendarCompetitorId")]
        public virtual CalendarCompetitor CalendarCompetitor { get; set; }
        [ForeignKey("CalendarCompetitorCompanyId")]
        public virtual CalendarCompetitorCompany CalendarCompetitorCompany { get; set; }
    }
}
