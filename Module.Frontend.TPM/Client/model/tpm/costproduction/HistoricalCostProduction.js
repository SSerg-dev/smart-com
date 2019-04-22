﻿Ext.define('App.model.tpm.region.HistoricalRegion', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Region',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'PlanQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'ActualQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'BudgetSubItemName', type: 'string', hidden: false, isDefault: true },
        { name: 'BudgetSubItemBudgetItemName', type: 'string', hidden: false, isDefault: true },
        { name: 'AttachFileName', type: 'string', hidden: false, isDefault: true },
        { name: 'BorderColor', type: 'string', hidden: true, isDefault: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalRegions',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
