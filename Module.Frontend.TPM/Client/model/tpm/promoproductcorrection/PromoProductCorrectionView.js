Ext.define('App.model.tpm.promoproductcorrection.PromoProductCorrectionView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductCorrectionView',
    fields: [
        { name: 'Number', type: 'int', hidden: false, isDefault: true, defaultFilterConfig: { valueField: 'Number' } },
        { name: 'ClientHierarchy', type: 'string', hidden: false, isDefault: true  },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTechName', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'ProductSubrangesList', type:'string', hidden: false, isDefault: true },
		{ name: 'MarsMechanicName', type: 'string', mapping: 'MarsMechanicName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'EventName', type: 'string', mapping: 'EventName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Event', hidden: false, isDefault: true  },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatusSystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },        
        { name: 'MarsStartDate', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsEndDate', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', type:'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductIncrementalLSV', type:'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductLSV', type:'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ZREP', type:'string', hidden: false, isDefault: true },
        { name: 'PlanProductUpliftPercentCorrected', type: 'float', hidden: false, isDefault: true  },
        { name: 'CreateDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ChangeDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'UserName', type:'string', hidden: false, isDefault: true },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true},
        { name: 'Id', hidden: true },
        { name: 'PromoProductId', hidden: true, isDefault: true },
        { name: 'UserId', hidden: true, isDefault: true, defaultValue: null },
        { name: 'PromoDispatchStartDate', type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone, isDefault: true },
        { name: 'PromoStatusName', type: 'string', hidden: true, isDefault: true  },
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: true, isDefault: true },
        { name: 'IsInExchange', type: 'boolean', hidden: true, isDefault: true },
    ],

    proxy: {
        type: 'breeze',
        resourceName: 'PromoProductCorrectionViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: 'Current'
        }
    },
});