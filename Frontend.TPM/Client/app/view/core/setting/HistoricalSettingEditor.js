Ext.define('App.view.core.setting.HistoricalSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalsettingeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Type'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Value',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Value'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Description'),
        }]
    }
});
