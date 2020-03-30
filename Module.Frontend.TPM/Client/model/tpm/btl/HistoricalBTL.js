Ext.define('App.model.tpm.btl.HistoricalBTL', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'BTL',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'EventId', hidden: true, isDefault: true },
        { name: 'EventName', type: 'string', isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalBTLs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
