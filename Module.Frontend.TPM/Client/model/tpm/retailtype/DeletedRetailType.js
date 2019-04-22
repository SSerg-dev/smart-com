Ext.define('App.model.tpm.retailtype.DeletedRetailType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RetailType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedRetailTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
