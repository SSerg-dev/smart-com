Ext.define('App.model.tpm.budgetitem.DeletedBudgetItem', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BudgetItem',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },        
        { name: 'BudgetId', hidden: true, isDefault: true },
        { name: 'BudgetName', type: 'string', mapping: 'Budget.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Budget', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'ButtonColor', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', useNull: true, type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBudgetItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
