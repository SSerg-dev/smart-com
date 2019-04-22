Ext.define('App.model.tpm.client.Client', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Client',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'CommercialSubnetId', hidden: true, isDefault: true },
        {
            name: 'CommercialSubnetCommercialNetName', type: 'string', mapping: 'CommercialSubnet.CommercialNet.Name',
            defaultFilterConfig: { valueField: 'CommercialNetName' }, breezeEntityType: 'CommercialNet', hidden: false, isDefault: true
        },
        {
            name: 'CommercialSubnetName', type: 'string', mapping: 'CommercialSubnet.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialSubnet', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Clients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
