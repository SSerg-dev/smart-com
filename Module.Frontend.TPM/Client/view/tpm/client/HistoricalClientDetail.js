Ext.define('App.view.tpm.client.HistoricalClientDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalclientdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClient', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClient').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CommercialSubnetCommercialNetName',
            fieldLabel: l10n.ns('tpm', 'Client').value('CommercialSubnetCommercialNetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CommercialSubnetName',
            fieldLabel: l10n.ns('tpm', 'Client').value('CommercialSubnetName'),
        }]
    }
});