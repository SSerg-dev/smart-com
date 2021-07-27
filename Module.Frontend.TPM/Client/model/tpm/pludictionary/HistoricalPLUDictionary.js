Ext.define('App.model.tpm.assortmentmatrix.HistoricalPLUDictionary', {
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
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeName', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductId', hidden: true, isDefault: true },
        { name: 'ProductEANPC', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPLUDictionarys',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
