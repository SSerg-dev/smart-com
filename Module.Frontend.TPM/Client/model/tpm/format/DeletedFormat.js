Ext.define('App.model.tpm.format.DeletedFormat', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Format',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedFormats',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
