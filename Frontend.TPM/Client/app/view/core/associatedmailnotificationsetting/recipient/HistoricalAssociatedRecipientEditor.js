Ext.define('App.view.core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipientEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalassociatedrecipienteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalRecipient', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('Type')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Value',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedRecipient').value('Value')
        }]
    }
});
