Ext.define('App.model.tpm.promosupport.PromoSupportChoose', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupport',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'BudgetSubItemId', hidden: true, isDefault: true },        
        {
            name: 'BudgetSubItemName', type: 'string', mapping: 'BudgetSubItem.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'BudgetSubItem', hidden: false, isDefault: true
        },
        {
            name: 'BudgetSubItemBudgetItemName', type: 'string', mapping: 'BudgetSubItem.BudgetItem.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'BudgetItem', hidden: false, isDefault: true
        },
        { name: 'PlanQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
    ],

    proxy: {
        type: 'breeze',
        resourceName: 'PromoSupports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
