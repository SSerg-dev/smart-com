Ext.define('App.model.tpm.costproduction.HistoricalCostProduction', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PromoSupport',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'PlanQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'ActualQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PlanProdCostPer1Item', type: 'float', hidden: true, isDefault: false },
        { name: 'ActualProdCostPer1Item', type: 'float', hidden: true, isDefault: false },
        { name: 'PlanProdCost', type: 'float', hidden: true, isDefault: false },
        { name: 'ActualProdCost', type: 'float', hidden: true, isDefault: false },
        { name: 'AttachFileName', type: 'string', hidden: true, isDefault: false },
        { name: 'PONumber', type: 'string', hidden: false, isDefault: true },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'BudgetSubItemName', type: 'string', hidden: false, isDefault: true },
        { name: 'BudgetSubItemBudgetItemName', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoSupports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            Id: null
        }
    }
});
