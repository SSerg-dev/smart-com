Ext.define('App.model.tpm.commercialsubnet.DeletedCommercialSubnet', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CommercialSubnet',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'CommercialNetId', hidden: true, isDefault: true },
        {
            name: 'CommercialNetName', type: 'string', isDefault: true, mapping: 'CommercialNet.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialNet'
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCommercialSubnets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
