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
