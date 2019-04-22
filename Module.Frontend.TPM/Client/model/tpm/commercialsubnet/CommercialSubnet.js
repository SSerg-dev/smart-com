Ext.define('App.model.tpm.commercialsubnet.CommercialSubnet', {
    extend: 'Sch.model.Resource',
    mixins: ['Ext.data.Model'],
    idProperty: 'Id',
    breezeEntityType: 'CommercialSubnet',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'CommercialNetId', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        {
            name: 'CommercialNetName', type: 'string', isDefault: true, mapping: 'CommercialNet.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialNet'
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CommercialSubnets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
