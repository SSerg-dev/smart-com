Ext.define('App.view.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPointEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalassociatedaccesspointeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAssociatedAccessPoint', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Resource',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Resource'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Action',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Action'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Description'),
        }]
    }
});
