Ext.define('App.model.tpm.postpromoeffect.PostPromoEffect', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PostPromoEffect',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: false },
        { name: 'ProductTreeId', hidden: true, isDefault: false },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ProductTreeFullPathName', type: 'string', mapping: 'ProductTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ProductTree', hidden: false, isDefault: true
        },
        { name: 'EffectWeek1', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'EffectWeek2', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'TotalEffect', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PostPromoEffects',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
