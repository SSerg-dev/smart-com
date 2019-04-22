Ext.define('App.model.tpm.distributor.DeletedDistributor', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Distributor',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedDistributors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
