Ext.define('App.view.tpm.promoproduct.HistoricalPromoProductDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalactualdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoProduct').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoProduct').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoProduct').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalActual', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoProduct').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('EAN'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductPCQty',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCQty'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductQty',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductQty'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductUOM',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductUOM'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductShelfPrice',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductShelfPrice'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductPCLSV',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
        }]
    }
});
