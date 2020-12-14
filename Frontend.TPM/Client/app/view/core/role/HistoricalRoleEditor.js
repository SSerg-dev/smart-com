Ext.define('App.view.core.role.HistoricalRoleEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalroleeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalRole', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('SystemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('DisplayName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsAllow',
            renderer: App.RenderHelper.getBooleanRenderer(),
            fieldLabel: l10n.ns('core', 'HistoricalRole').value('IsAllow'),
        }]
    }
});
