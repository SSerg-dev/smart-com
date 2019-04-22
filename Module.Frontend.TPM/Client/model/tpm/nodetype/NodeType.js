Ext.define('App.model.tpm.nodetype.NodeType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'NodeType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Priority', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'NodeTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
