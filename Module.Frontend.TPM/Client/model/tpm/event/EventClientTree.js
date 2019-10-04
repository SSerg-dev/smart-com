Ext.define('App.model.tpm.event.EventClientTree', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'EventClientTree',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'EventId', hidden: true },
        { name: 'ClientTreeId', hidden: true },
        { name: 'ClientTreeFullPathName', type: 'string', isDefault: true, mapping: 'ClientTree.FullPathName', defaultFilterConfig: { valueField: 'FullPathName' } },
        { name: 'ClientTreeObjectId', type: 'int', isDefault: true, mapping: 'ClientTree.ObjectId', defaultFilterConfig: { valueField: 'ObjectId' } },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'EventClientTrees',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});