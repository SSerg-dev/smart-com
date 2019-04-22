using Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO {
    public class ProductTreeHierarchyView : IEntity<int> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Hierarchy { get; set; }
    }
}
