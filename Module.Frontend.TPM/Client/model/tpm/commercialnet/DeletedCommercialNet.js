Ext.define('App.model.tpm.commercialnet.DeletedCommercialNet', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CommercialNet',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCommercialNets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
