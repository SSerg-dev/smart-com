Ext.define('App.model.tpm.brandtech.DeletedBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'BrandId', hidden: true, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, default: true },
        { name: 'TechnologyId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Brand', hidden: false, isDefault: true
        },
        {
            name: 'TechnologyName', type: 'string', mapping: 'Technology.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Technology', hidden: false, isDefault: true
        }     ,
        { name: 'BrandTech_code', type: 'string', hidden: false, default: true },   
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
