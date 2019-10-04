using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class BudgetSubItemClientTree : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int ClientTreeId { get; set; }

        public Guid BudgetSubItemId { get; set; }

        public virtual BudgetSubItem BudgetSubItem { get; set; }
        public virtual ClientTree ClientTree { get; set; }

    }
}
