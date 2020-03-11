Ext.define('App.view.core.filebuffer.HistoricalFileBufferDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalfilebufferdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileBuffer', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceDirection',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('InterfaceDirection'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('CreateDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'HandlerId',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('HandlerId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FileName',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('FileName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('Status'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('ProcessDate'),
        }]
    }
});
