Ext.define('App.view.core.csvextractinterfacesetting.HistoricalCSVExtractInterfaceSettingDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcsvextractinterfacesettingdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalCSVExtractInterfaceSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FileNameMask',
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('FileNameMask'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ExtractHandler',
            fieldLabel: l10n.ns('core', 'HistoricalCSVExtractInterfaceSetting').value('ExtractHandler'),
        }]
    }
});
