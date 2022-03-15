Ext.define('App.model.tpm.competitorpromo.DeletedCompetitorPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CompetitorPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'CompetitorId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompetitorName', type: 'string', mapping: 'Competitor.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Competitor', hidden: false, isDefault: true
        },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true
        },
        { name: 'CompetitorBrandTechId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompetitorBrandTechName', type: 'string', mapping: 'CompetitorBrandTech.BrandTech', defaultFilterConfig: { valueField: 'BrandTech' },
            breezeEntityType: 'CompetitorBrandTech', hidden: false, isDefault: true
        },
        { name: 'Name', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'Number', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DateStart', useNull: true, type: 'date', hidden: false, isDefault: false, mapping: 'StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MechanicType', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'Discount', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'Price', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'Subrange', useNull: false, type: 'string', hidden: false, isDefault: true },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCompetitorPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
