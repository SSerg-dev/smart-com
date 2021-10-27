﻿Ext.define('App.model.tpm.competitorpromo.HistoricalCompetitorPromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'CompetitorPromo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'CompetitorId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompetitorName', type: 'string', mapping: 'Competitor.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Competitor', hidden: false, isDefault: true
        },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: true, defaultValue: null },

        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'CompetitorBrandTechId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'BrandTech', type: 'string', mapping: 'CompetitorBrandTech.BrandTech', defaultFilterConfig: { valueField: 'BrandTech' },
            breezeEntityType: 'CompetitorBrandTech', hidden: false, isDefault: true
        },
        { name: 'Name', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'Number', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'Status', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'PromoStatus', hidden: false, isDefault: true
        },
        { name: 'IsGrowthAcceleration', useNull: false, type: 'boolean', hidden: false, isDefault: true },

        { name: 'StartDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DateStart', useNull: true, type: 'date', hidden: false, isDefault: false, mapping: 'StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'Discount', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'Price', useNull: false, type: 'float', hidden: false, isDefault: true },
        { name: 'Subrange', useNull: false, type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCompetitorPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
