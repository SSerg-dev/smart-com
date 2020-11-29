Ext.define('App.model.tpm.budgetitem.BudgetItem', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BudgetItem',
    fields: [
        { name: 'Id', hidden: true },        
        { name: 'BudgetId', hidden: true, isDefault: true },
        { name: 'BudgetName', type: 'string', mapping: 'Budget.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Budget', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'ButtonColor', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BudgetItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
