Ext.define('App.view.tpm.promoproductcorrection.HistoricalPromoProductCorrectionDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalpromoproductcorrectiondetail',
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
                name: 'PlanProductUpliftPercentCorrected',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductUpliftPercentCorrected'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'CreateDate',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('CreateDate'),
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ChangeDate',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ChangeDate'),
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }]
    }
});
