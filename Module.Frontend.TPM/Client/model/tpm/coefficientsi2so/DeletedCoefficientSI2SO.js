Ext.define('App.model.tpm.coefficientsi2so.DeletedCoefficientSI2SO', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CoefficientSI2SO',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'CoefficientValue', type: 'float', hidden: false, isDefault: true },
        { name: 'Lock', type: 'boolean', hidden: true, isDefault: true },

        { name: 'BrandTechName', type: 'string', isDefault: true, mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech' },
        { name: 'BrandTechBrandTech_code', type: 'string', isDefault: true, mapping: 'BrandTech.BrandsegTechsub_code', defaultFilterConfig: { valueField: 'BrandsegTechsub_code' } },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCoefficientSI2SOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
