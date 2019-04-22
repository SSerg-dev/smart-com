Ext.define('App.model.tpm.client.DeletedClient', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Client',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'CommercialSubnetId', hidden: true, isDefault: true },
        { name: 'CommercialSubnetCommercialNetName', type: 'string', mapping: 'CommercialSubnet.CommercialNet.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialNet', hidden: false, isDefault: true },
        { name: 'CommercialSubnetName', type: 'string', mapping: 'CommercialSubnet.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialSubnet', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedClients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
