Ext.define('App.model.tpm.pludictionary.PLUDictionary', {
    extend: 'Ext.data.Model',
    breezeEntityType: 'PLUDictionary',
    fields: [
        {
            name: 'ClientTreeName', type: 'string', mapping: 'ClientTreeName',
            defaultFilterConfig: { valueField: 'Name' },  tree: true, hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true,
        }, 
        { name: 'PluCode', type: 'string', hidden: false, isDefault: true, mapping: 'PluCode' },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true, mapping: 'EAN_PC' },
        { name: 'id', type: 'string', hidden: true},
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PLUDictionaries',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
