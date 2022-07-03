Ext.define('App.view.tpm.event.HistoricalEventDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaleventdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [
            {
                xtype: 'singlelinedisplayfield',
                name: '_User',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_User')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_Role',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_Role')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_EditDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_EditDate')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalEvent', 'OperationType'),
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_Operation')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'Name',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('Name'),
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'Description',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('Description'),
            },
            {
                xtype: 'textfield',
                name: 'Type',
                fieldLabel: l10n.ns('tpm', 'Event').value('EventTypeName')
            },
            {
                xtype: 'textfield',
                name: 'MarketSegment',
                fieldLabel: l10n.ns('tpm', 'Event').value('MarketSegment')
            },
        ]
    }
});
