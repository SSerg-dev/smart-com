Ext.define('App.model.tpm.pludictionary.HistoricalPLUDictionary', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PLUDictionary',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeName', type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true },
        { name: 'PluCode', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPLUDictionaries',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
