Ext.define('App.view.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfoPIEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoactivitydetailsinfopieditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ZREP',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ProductEN'),
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('PlanProductCaseQty'),
            name: 'PlanProductBaselineLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('PlanProductIncrementalLSV'),
            name: 'PlanProductIncrementalLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductIncrementalLSV'),
            name: 'ActualProductIncrementalLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('PlanProductLSV'),
            name: 'PlanProductLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('PlanProductPostPromoEffectLSV'),
            name: 'PlanProductPostPromoEffectLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductLSV'),
            name: 'ActualProductLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPostPromoEffectLSV'),
            name: 'ActualProductPostPromoEffectLSV',
        }, {
            xtype: 'numberfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductLSVByCompensation'),
            name: 'ActualProductLSVByCompensation',
        }, {
                xtype: 'numberfield',
                format: '0.00',
                fieldLabel: l10n.ns('tpm', 'PromoProduct').value('SumInvoiceProduct'),
                name: 'SumInvoiceProduct',
           }
        ] 
    },

    listeners: {
        beforerender: function (editor) {
                editor.down('#edit').hidden = true;
        },
    }
});
