Ext.define('App.model.tpm.actualcogs.ActualCOGS', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'ActualCOGS',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'Year', type: 'int', hidden: false, isDefault: true, },
        { name: 'BrandTechId', hidden: true, useNull: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        { name: 'LSVpercent', type: 'float', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'ActualCOGSs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});