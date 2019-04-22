Ext.define('App.model.tpm.nodeType.HistoricalNodeType', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'NodeType',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Type', type: 'string', isDefault: true },
        { name: 'Name', type: 'string', isDefault: true },
        { name: 'Priority', type: 'int', isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalNodeTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
