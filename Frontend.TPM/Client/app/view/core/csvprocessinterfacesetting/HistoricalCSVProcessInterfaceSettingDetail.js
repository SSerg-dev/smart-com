Ext.define('App.view.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSettingDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcsvprocessinterfacesettingdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalCSVProcessInterfaceSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('InterfaceName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Delimiter',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('Delimiter'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'UseQuoting',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('UseQuoting'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'QuoteChar',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('QuoteChar'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessHandler',
            fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('ProcessHandler'),
        }]
    }
});
