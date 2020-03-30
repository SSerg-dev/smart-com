Ext.define('App.model.tpm.nonpromosupport.HistoricalNonPromoSupport', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
	breezeEntityType: 'HistoricalNonPromoSupport',
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
        { name: 'AttachFileName', type: 'string', hidden: true, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'NonPromoEquipmentEquipmentType', type: 'string', hidden: false, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalNonPromoSupports',
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
