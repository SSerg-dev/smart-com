Ext.define('App.model.tpm.planpostpromoeffect.HistoricalPlanPostPromoEffect', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PlanPostPromoEffect',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'Size', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectW1', type: 'float', hidden: false, isDefault: true, },
        { name: 'PlanPostPromoEffectW2', type: 'float', hidden: false, isDefault: true, },
        { name: 'DiscountRangeId', hidden: true, useNull: true, isDefault: true },
        { name: 'DiscountRangeName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'DurationRangeId', hidden: true, useNull: true, isDefault: true },
        { name: 'DurationRangeName', type: 'string', useNull: true, hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPlanPostPromoEffects',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            Id: null
        }
    }
});
