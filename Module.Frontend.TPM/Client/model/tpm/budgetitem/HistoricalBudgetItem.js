Ext.define('App.model.tpm.budgetitem.HistoricalBudgetItem', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'BudgetItem',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },        
        { name: 'BudgetId', hidden: true, isDefault: true },
        { name: 'BudgetName', type: 'string' },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'ButtonColor', useNull: false, type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalBudgetItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
