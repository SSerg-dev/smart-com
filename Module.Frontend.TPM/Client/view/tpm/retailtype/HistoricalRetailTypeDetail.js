Ext.define('App.view.tpm.retailtype.HistoricalRetailTypeDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalretailtypedetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalRetailType').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalRetailType').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalRetailType').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalRetailType', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalRetailType').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',            name: 'Name',            fieldLabel: l10n.ns('tpm', 'RetailType').value('Name')
        }]
    }
});