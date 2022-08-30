Ext.define('App.model.tpm.eventtype.EventType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'EventType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'National', type: 'boolean', hidden: true, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'EventTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});
