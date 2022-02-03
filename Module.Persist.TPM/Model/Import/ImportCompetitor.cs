using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportCompetitor : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
