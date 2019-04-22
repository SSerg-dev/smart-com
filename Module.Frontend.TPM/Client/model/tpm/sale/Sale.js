Ext.define('App.model.tpm.sale.Sale', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Sale',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BudgetItemId', useNull: true, hidden: true, isDefault: true },
        { name: 'PromoId', useNull: true, hidden: true, isDefault: true },
        { name: 'Plan', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'Fact', useNull: true, type: 'int', hidden: false, isDefault: true },
        {
            name: 'BudgetItemBudgetName', type: 'string', mapping: 'BudgetItem.Budget.Name',
            defaultFilterConfig: { valueField: 'BudgetName' }, breezeEntityType: 'Budget', hidden: false, isDefault: true
        },
        {
            name: 'BudgetItemName', type: 'string', mapping: 'BudgetItem.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BudgetItem', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Sales',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
