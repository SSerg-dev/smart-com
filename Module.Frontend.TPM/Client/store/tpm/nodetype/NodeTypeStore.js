Ext.define('App.store.tpm.nodetype.NodeTypeStore', {
    extend: 'Ext.data.Store',
    alias: 'store.nodetypestore',

    fields: ['id', 'text'],

    data: [
        { id: 'CLIENT', text: l10n.ns('tpm', 'NodeType', 'TypeEnum').value('CLIENT') },
        { id: 'PRODUCT', text: l10n.ns('tpm', 'NodeType', 'TypeEnum').value('PRODUCT') }
    ]
});
