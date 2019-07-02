Ext.define('App.view.tpm.assortmentmatrix.HistoricalAssortmentMatrixDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbrandtechdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalAssortmentMatrix').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalAssortmentMatrix').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalAssortmentMatrix').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalAssortmentMatrix', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalAssortmentMatrix').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
            name: 'ClientTreeName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC'),
        },
        ]
    }
});
