Ext.define('App.model.tpm.distributor.Distributor', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Distributor',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Distributors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
