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
    public class ImportBudgetSubItem : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Budget), LookupPropertyName = "Name")]
        [Display(Name = "Budget")]
        public String BudgetName { get; set; }


        [ImportCSVColumn(ColumnNumber = 1)]
        //[NavigationPropertyMap(LookupEntityType = typeof(BudgetItem), LookupPropertyName = "Name")]
        [Display(Name = "BudgetItem")]
        public String BudgetItemName { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public Guid? BudgetItemId { get; set; }
        public Guid? BudgetId { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }

    }
}
