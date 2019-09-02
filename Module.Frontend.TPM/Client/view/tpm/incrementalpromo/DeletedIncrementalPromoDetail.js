Ext.define('App.view.tpm.incrementalpromo.DeletedIncrementalPromoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedincrementalpromodetail',
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
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP'),
            name: 'ProductZREP',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductName'),
            name: 'ProductName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoClient'),
            name: 'PromoClient',             
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber'),
            name: 'PromoNumber'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
            name: 'PromoName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalCases'),
            name: 'PlanPromoIncrementalCases',
            format: '0.00',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('CasePrice'),
            name: 'CasePrice',
            format: '0.00',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalLSV'),
            name: 'PlanPromoIncrementalLSV',
            format: '0.00',
        }]
    }
})
