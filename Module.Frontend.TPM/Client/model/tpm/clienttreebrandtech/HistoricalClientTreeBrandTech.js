Ext.define('App.model.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'ClientTreeBrandTech',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Share', type: 'float', hidden: false, isDefault: true },
        { name: 'ParentClientTreeDemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'ClientTreeName', type: 'string', hidden: false, isDefault: true },
        { name: 'CurrentBrandTechName', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalClientTreeBrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
