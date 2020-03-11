Ext.define('App.view.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSettingDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalxmlprocessinterfacesettingdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalXMLProcessInterfaceSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'RootElement',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('RootElement'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessHandler',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('ProcessHandler'),
        }]
    }
});
