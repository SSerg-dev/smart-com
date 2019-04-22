Ext.define('App.view.tpm.nodetype.DeletedNodeTypeDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletednodetypedetail',
    width: 500,
    minWidth: 500,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Type'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Priority',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Priority'),
        }
        ]
    }
})