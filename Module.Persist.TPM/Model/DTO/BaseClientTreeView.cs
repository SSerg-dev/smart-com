using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO
{
    [Table("BaseClientTreeView")]
    public class BaseClientTreeView : IEntity<int> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BOI { get; set; }
        public string ResultNameStr { get; set; }
    }
}
