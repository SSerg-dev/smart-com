Ext.define('App.model.tpm.btl.BTL', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BTL',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'EventId', hidden: true, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true ,isKey: true },
        { name: 'PlanBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },

        {
            name: 'EventName', type: 'string', mapping: 'Event.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Event', hidden: false, isDefault: true
        },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BTLs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
