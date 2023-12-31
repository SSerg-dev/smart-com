﻿Ext.define('App.model.tpm.coefficientsi2so.CoefficientSI2SO', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CoefficientSI2SO',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'CoefficientValue', type: 'float', hidden: false, isDefault: true },
        { name: 'Lock', type: 'bool', hidden: true, isDefault: true },

        { name: 'BrandTechName', type: 'string', isDefault: true, mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech' },
        { name: 'BrandTechBrandTech_code', type: 'string', isDefault: true, mapping: 'BrandTech.BrandsegTechsub_code', defaultFilterConfig: { valueField: 'BrandsegTechsub_code' } },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CoefficientSI2SOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
