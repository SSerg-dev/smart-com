Ext.define('App.model.tpm.budgetsubitem.HistoricalBudgetSubItem', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'BudgetSubItem',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },        
        //{ name: 'BudgetItemId', hidden: false, isDefault: false },
        { name: 'BudgetItemBudgetName', type: 'string', isDefault: true },
        { name: 'BudgetItemName', type: 'string', isDefault: true },        
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalBudgetSubItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
