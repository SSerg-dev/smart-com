Ext.define('App.view.tpm.promoproductcorrection.DeletedPromoProductCorrectionDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpromoproductcorrectiondetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
                xtype: 'singlelinedisplayfield',
                name: 'ZREP',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ZREP'),
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ClientHierarchy'),
                name: 'ClientHierarchy',
                width: 250,
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('BrandTech'),
                name: 'BrandTech',
                width: 120,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ProductSubrangesList'),
                name: 'ProductSubrangesList',
                width: 110,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Event'),
                name: 'Event',
                width: 110,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Mechanic'),
                name: 'Mechanic',
                width: 130,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Status'),
                name: 'Status',
                width: 120,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('MarsStartDate'),
                name: 'MarsStartDate',
                width: 120,
            },
            {
                xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('MarsEndDate'),
                name: 'MarsEndDate',
                width: 120,
            },
            {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductBaselineLSV'),
                name: 'PlanProductBaselineLSV'
            },
            {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductIncrementalLSV'),
                name: 'PlanProductIncrementalLSV'
            },
            {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductLSV'),
                name: 'PlanProductLSV'
            },
            {
                xtype: 'numberfield',
                name: 'PlanProductUpliftPercentCorrected',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductUpliftPercentCorrected'),
            },
            {
                xtype: 'datefield',
                name: 'CreateDate',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('CreateDate'),
            },
            {
                xtype: 'datefield',
                name: 'ChangeDate',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ChangeDate'),
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'UserName',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('UserName'),
            }]
    }
})
