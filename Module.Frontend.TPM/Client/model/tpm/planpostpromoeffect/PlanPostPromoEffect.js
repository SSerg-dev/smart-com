Ext.define('App.model.tpm.planpostpromoeffect.PlanPostPromoEffect', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PlanPostPromoEffect',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Size', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectW1', type: 'float', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectW2', type: 'float', hidden: false, isDefault: true },
        { name: 'DiscountRangeId', hidden: true, useNull: false, isDefault: false },
        {
            name: 'DiscountRangeName',
            type: 'string',
            mapping: 'DiscountRange.Name',
            isDefault: true,
            defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'DiscountRange',
            hidden: false
        },
        { name: 'DurationRangeId', hidden: true, useNull: false, isDefault: false },
        {
            name: 'DurationRangeName',
            type: 'string',
            mapping: 'DurationRange.Name',
            isDefault: true,
            defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'DurationRange',
            hidden: false
        },
        { name: 'BrandTechId', hidden: true, useNull: true, isDefault: true },
        { 
            name: 'BrandTechName', type: 'string',mapping: 'BrandTech.BrandsegTechsub',
            defaultFilterConfig: { valueField: 'BrandsegTechsub' },
            isDefault: true,
            breezeEntityType: 'BrandTech',
            hidden: false
        },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false,
            isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true
        }
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
