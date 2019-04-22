Ext.define('App.model.tpm.event.DeletedEvent', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Event',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Year', type: 'int', hidden: false, isDefault: true },
        { name: 'Period', type: 'string', hidden: false, isDefault: true },
        { name: 'Description', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedEvents',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
