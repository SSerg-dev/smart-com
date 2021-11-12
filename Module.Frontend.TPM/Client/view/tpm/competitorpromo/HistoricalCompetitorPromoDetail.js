Ext.define('App.view.tpm.competitorpromo.HistoricalCompetitorPromoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcompetitorpromodetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorPromo', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorName',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('CompetitorName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorBrandTechName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
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
            name: 'Status',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
        }]
    }
});