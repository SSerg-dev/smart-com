Ext.define('App.model.tpm.btl.DeletedBTL', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BTL',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'EventId', hidden: true, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualBTLTotal', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },

        { name: 'EventName', type: 'string', isDefault: true, mapping: 'Event.Name', defaultFilterConfig: { valueField: 'Name' } },
        { name: 'EventDescription', type: 'string', hidden: true, isDefault: true, mapping: 'Event.Description', defaultFilterConfig: { valueField: 'Description' } },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBTLs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
