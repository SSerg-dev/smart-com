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
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromoProduct', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoProduct').value('_Operation')
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ClientHierarchy'),
                name: 'ClientHierarchy',
                width: 250,
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('BrandTech'),
                name: 'BrandTech',
                width: 120,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ProductSubrangesList'),
                name: 'ProductSubrangesList',
                width: 110,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Event'),
                name: 'Event',
                width: 110,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Mechanic'),
                name: 'Mechanic',
                width: 130,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Status'),
                name: 'Status',
                width: 120,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('MarsStartDate'),
                name: 'MarsStartDate',
                width: 120,
        }, {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('MarsEndDate'),
                name: 'MarsEndDate',
                width: 120,
        }, {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductBaselineLSV'),
                name: 'PlanProductBaselineLSV'
        }, {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductIncrementalLSV'),
                name: 'PlanProductIncrementalLSV'
        }, {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductLSV'),
                name: 'PlanProductLSV'
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
