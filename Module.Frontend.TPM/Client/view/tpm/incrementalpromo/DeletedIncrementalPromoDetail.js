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
            name: 'PromoNumber',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoBrandTechName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoBrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoStartDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoStartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoEndDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoEndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoDispatchesStart',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoDispatchesStart'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoDispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoDispatchesEnd'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalCaseAmount',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalCaseAmount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalPrice',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalPrice'),
        }]
    }
})
