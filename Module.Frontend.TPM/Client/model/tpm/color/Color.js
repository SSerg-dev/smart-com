Ext.define('App.model.tpm.color.Color', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Color',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'SystemName', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'DisplayName', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'BrandName', type: 'string', mapping: 'BrandTech.Brand.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Brand', hidden: false, isDefault: true
        },
        {
            name: 'TechnologyName', type: 'string', mapping: 'BrandTech.Technology.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Technology', hidden: false, isDefault: true
        },
        {
            name: 'SubBrandName', type: 'string', mapping: 'BrandTech.Technology.SubBrand', defaultFilterConfig: { valueField: 'SubBrand' },
            breezeEntityType: 'Technology', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Colors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
