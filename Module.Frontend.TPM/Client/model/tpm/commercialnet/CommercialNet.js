Ext.define('App.model.tpm.commercialnet.CommercialNet', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CommercialNet',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CommercialNets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
