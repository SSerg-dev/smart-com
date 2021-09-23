using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CalendarСompetitor : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_Name", 1, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_Name", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [StringLength(124)]
        [Index("Unique_Name", 2, IsUnique = true)]
        public string Name { get; set; }
    }
}
