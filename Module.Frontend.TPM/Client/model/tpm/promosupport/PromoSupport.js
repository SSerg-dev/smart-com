Ext.define('App.model.tpm.promosupport.PromoSupport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupport',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'BudgetSubItemId', hidden: true, isDefault: true },
        { name: 'PromoId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'ActualQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PlanProdCost', type: 'float', hidden: true, isDefault: false },
        { name: 'ActualProdCost', type: 'float', hidden: true, isDefault: false },
        { name: 'UserTimestamp', type: 'string', hidden: true, isDefault: false },
        { name: 'AttachFileName', type: 'string', hidden: true, isDefault: false },
        { name: 'BorderColor', type: 'string', hidden: true, isDefault: false },
        { name: 'PlanProdCostPer1Item', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualProdCostPer1Item', type: 'float', hidden: false, isDefault: true },
        { name: 'PONumber', type: 'string', hidden: false, isDefault: true },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'BudgetSubItemName', type: 'string', mapping: 'BudgetSubItem.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'BudgetSubItem', hidden: false, isDefault: true
        },
        {
            name: 'BudgetSubItemBudgetItemName', type: 'string', mapping: 'BudgetSubItem.BudgetItem.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'BudgetItem', hidden: false, isDefault: true
        },
        {
            name: 'BudgetSubItemBudgetItemId', mapping: 'BudgetSubItem.BudgetItem.Id', defaultFilterConfig: { valueField: 'Id' },
            breezeEntityType: 'BudgetItem', hidden: true, isDefault: false
        }
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
