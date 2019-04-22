Ext.define('App.model.tpm.postpromoeffect.HistoricalPostPromoEffect', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PostPromoEffect',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'EffectWeek1', type: 'float', hidden: false, isDefault: true },
        { name: 'EffectWeek2', type: 'float', hidden: false, isDefault: true },
        { name: 'TotalEffect', type: 'float', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPostPromoEffects',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
