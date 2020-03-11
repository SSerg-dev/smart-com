Ext.define('App.view.core.filesendinterfacesetting.HistoricalFileSendInterfaceSettingDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalfilesendinterfacesettingdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileSendInterfaceSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TargetPath',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('TargetPath'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TargetFileMask',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('TargetFileMask'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SendHandler',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('SendHandler'),
        }]
    }
});
