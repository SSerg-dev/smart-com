Ext.define('App.model.tpm.baseline.HistoricalBaseLine', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'BaseLine',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ProductZREP', type: 'string', isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true }, 
        { name: 'InputBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'SellInBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'SellOutBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'Type', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalBaseLines',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
