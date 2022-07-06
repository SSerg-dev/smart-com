﻿using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Competitor : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_Competitor", 1, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_Competitor", 2, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(124)]
        [Index("Unique_Competitor", 0, IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<CompetitorBrandTech> CompetitorBrandTechs { get; set; }
        public virtual ICollection<CompetitorPromo> CompetitorPromoes { get; set; }
    }
}
