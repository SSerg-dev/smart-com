Ext.define('App.model.tpm.event.Event', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Event',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Year', type: 'int', hidden: false, isDefault: true },
        { name: 'Period', type: 'string', hidden: false, isDefault: true },
        { name: 'Description', type: 'string', hidden: false, isDefault: true, useNull: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Events',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
