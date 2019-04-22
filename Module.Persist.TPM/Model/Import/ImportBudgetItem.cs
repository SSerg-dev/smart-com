using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportBudgetItem : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Budget), LookupPropertyName = "Name")]
        [Display(Name = "Budget")]
        public String BudgetName { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Item")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "ButtonColor")]
        public string ButtonColor { get; set; }


    }
}
