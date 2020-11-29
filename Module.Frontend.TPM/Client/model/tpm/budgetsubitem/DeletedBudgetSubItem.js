Ext.define('App.model.tpm.budgetsubitem.DeletedBudgetSubItem', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BudgetSubItem',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BudgetItemId', hidden: true, isDefault: false },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'BudgetName', type: 'string', mapping: 'BudgetItem.Budget.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Budget', hidden: false, isDefault: true },        
        { name: 'BudgetItemName', type: 'string', mapping: 'BudgetItem.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BudgetItem', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBudgetSubItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
