Ext.define('App.view.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSettingDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalfilecollectinterfacesettingdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileCollectInterfaceSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SourcePath',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('SourcePath'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SourceFileMask',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('SourceFileMask'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CollectHandler',
            fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('CollectHandler'),
        }]
    }
});
