using Core.Data;
using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import {
    public class ImportClientsShare : BaseImportEntity, IEntity<Guid> {

        public Guid Id { get; set; }

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Client hierarchy")]
        public String ResultNameStr { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client hierarchy code")]
        public int BOI { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy Share")]
        public int LeafShare { get; set; }

    }
}
