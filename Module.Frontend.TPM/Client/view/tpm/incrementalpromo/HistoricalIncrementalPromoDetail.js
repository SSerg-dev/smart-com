Ext.define('App.view.tpm.incrementalpromo.HistoricalIncrementalPromoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalincrementalpromodetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalIncrementalPromo', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_Operation')
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
});
