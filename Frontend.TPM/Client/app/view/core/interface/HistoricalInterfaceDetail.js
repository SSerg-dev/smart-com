Ext.define('App.view.core.interface.HistoricalInterfaceDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalinterfacedetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalInterface', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Direction',
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('Direction'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'HistoricalInterface').value('Description'),
        }]
    }
});
