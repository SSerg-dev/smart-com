﻿using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class BlockedPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid PromoId { get; set; }
        public Guid HandlerId { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        //!!! НИ ПРИКАКИХ ОБСТОЯТЕЛЬСТВАХ НЕ ДОБАВЛЯТЬ ВНЕШНИЕ СВЯЗИ
    }
}
