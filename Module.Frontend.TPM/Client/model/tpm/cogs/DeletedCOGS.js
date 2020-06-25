Ext.define('App.model.tpm.cogs.DeletedCOGS', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'COGS',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub', useNull: true, defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
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
        resourceName: 'DeletedCOGSs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
