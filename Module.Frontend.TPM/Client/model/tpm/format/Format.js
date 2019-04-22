Ext.define('App.model.tpm.format.Format', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Format',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Formats',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
