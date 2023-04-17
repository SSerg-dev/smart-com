Ext.define('App.model.tpm.planpostpromoeffect.PlanPostPromoEffect', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PlanPostPromoEffect',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Size', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectW1', type: 'float', hidden: false, isDefault: true, },
        { name: 'PlanPostPromoEffectW2', type: 'float', hidden: false, isDefault: true, },
        { name: 'DiscountRangeId', hidden: true, useNull: true, isDefault: true },
        {
            name: 'DiscountRangeName', type: 'string', mapping: 'DiscountRange.Name', tree: true,
        },
        { name: 'DurationRangeId', hidden: true, useNull: true, isDefault: true },
        {
            name: 'DurationRangeName', type: 'string', mapping: 'DurationRange.Name', tree: true,
        },
        { name: 'BrandTechId', hidden: true, useNull: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true
        },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true, }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PlanPostPromoEffects',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
