Ext.define('App.model.tpm.sale.HistoricalSale', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Sale',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'BudgetItemId', useNull: true, hidden: true, isDefault: true },
        { name: 'PromoId', useNull: true, hidden: true, isDefault: true },
        { name: 'Plan', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'Fact', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'BudgetItemBudgetName', type: 'string' },
        { name: 'BudgetItemName', type: 'string' }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalSales',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
