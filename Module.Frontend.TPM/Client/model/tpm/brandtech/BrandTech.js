Ext.define('App.model.tpm.brandtech.BrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandId', hidden: true, isDefault: true },
        { name: 'TechnologyId', hidden: true, isDefault: true },
        { name: 'BrandsegTechsub', hidden: true, isDefault: true },
        { name: 'TechSubName', hidden: true, isDefault: true },
        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'TechnologyName', type: 'string', mapping: 'Technology.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Technology', hidden: false, isDefault: true },
        { name: 'Technology_Description_ru', type: 'string', mapping: 'Technology.Description_ru', defaultFilterConfig: { valueField: 'Description_ru' }, breezeEntityType: 'Technology', hidden: false, isDefault: true },
        { name: 'SubBrandName', type: 'string', mapping: 'Technology.SubBrand', defaultFilterConfig: { valueField: 'SubBrand' }, breezeEntityType: 'Technology', hidden: false, isDefault: true },
        { name: 'BrandTech_code', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandsegTechsub_code', type: 'string', hidden: false, isDefault: true }


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
