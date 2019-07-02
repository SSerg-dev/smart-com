using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO {
    [Table("ClientTreeSharesView")]
    public class ClientTreeSharesView : IEntity<int> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BOI { get; set; }
        public string ResultNameStr { get; set; }
        public Int16 LeafShare { get; set; }
        public string DemandCode { get; set; }
    }
}
