Ext.define('App.view.tpm.competitorpromo.DeletedCompetitorPromoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcompetitorpromodetail',
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
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Number'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorBrandTechName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
        }, {
            xtype: 'datecolumn',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
        }, {
            xtype: 'datecolumn',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Subrange',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'GrowthAcceleration',
            fieldLabel: l10n.ns('tpm', 'Promo').value('GrowthAcceleration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
        }]
    }
})