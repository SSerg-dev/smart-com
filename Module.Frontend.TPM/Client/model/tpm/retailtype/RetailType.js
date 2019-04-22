Ext.define('App.model.tpm.retailtype.RetailType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RetailType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RetailTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
