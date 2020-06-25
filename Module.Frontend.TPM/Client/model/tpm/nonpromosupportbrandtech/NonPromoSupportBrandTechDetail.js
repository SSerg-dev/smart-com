Ext.define('App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'NonPromoSupportBrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'NonPromoSupportId', hidden: true },
        { name: 'BrandTechId', hidden: true },
        {
            name: 'BrandTechBrandName', type: 'string', mapping: 'BrandTech.Brand.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true
        },
        {
            name: 'BrandTechTechnologyName', type: 'string', mapping: 'BrandTech.Technology.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Technology', hidden: false, isDefault: true
        },
        {
            name: 'BrandTechSubName', type: 'string', mapping: 'BrandTech.Technology.SubBrand',
            defaultFilterConfig: { valueField: 'SubBrand' }, breezeEntityType: 'Technology', hidden: false, isDefault: true
        },
        { name: 'BrandTechBrandTech_code', type: 'string', mapping: 'BrandTech.BrandTech_code', hidden: false, isDefault: true },
        { name: 'BrandTechBrandsegTechsub_code', type: 'string', mapping: 'BrandTech.BrandsegTechsub_code', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'NonPromoSupportBrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
