Ext.define('App.model.tpm.storetype.StoreType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'StoreType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'StoreTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
