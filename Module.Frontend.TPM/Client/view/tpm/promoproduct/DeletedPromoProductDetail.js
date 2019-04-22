﻿Ext.define('App.view.tpm.promoproduct.DeletedPromoProductDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpromoproductdetail',
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
})
