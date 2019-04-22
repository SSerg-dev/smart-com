Ext.define('App.view.tpm.nodetype.HistoricalNodeTypeDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalnodetypedetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClient', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_Operation')
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
        }]
    }
});
