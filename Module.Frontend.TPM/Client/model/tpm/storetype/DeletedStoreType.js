Ext.define('App.model.tpm.storetype.DeletedStoreType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'StoreType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedStoreTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
