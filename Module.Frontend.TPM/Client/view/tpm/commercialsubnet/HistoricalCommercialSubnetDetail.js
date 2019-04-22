Ext.define('App.view.tpm.commercialsubnet.HistoricalCommercialSubnetDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcommercialsubnetdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCommercialSubnet').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCommercialSubnet').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCommercialSubnet').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCommercialSubnet', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCommercialSubnet').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CommercialNetName',
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('CommercialNetName'),
        }]
    }
});
