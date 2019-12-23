Ext.define('App.model.tpm.brandtech.BrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id', 
    breezeEntityType: 'BrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, default: true },
        { name: 'BrandId', hidden: true, isDefault: true },
        { name: 'TechnologyId', hidden: true, isDefault: true },
        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'TechnologyName', type: 'string', mapping: 'Technology.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Technology', hidden: false, isDefault: true },
        { name: 'BrandTech_code', type: 'string', hidden: false, default: true },
      

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
    }
});
