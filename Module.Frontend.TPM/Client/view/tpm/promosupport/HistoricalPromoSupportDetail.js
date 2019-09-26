Ext.define('App.view.tpm.promosupport.HistoricalPromoSupportDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalpromosupportdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('ActualQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PlanCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('ActualCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProdCost',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PlanProdCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProdCost',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('ActualProdCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'AttachFileName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('AttachFileName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PONumber',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PONumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InvoiceNumber',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('InvoiceNumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('BudgetSubItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemBudgetItemName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('BudgetSubItemBudgetItemName'),
        }]
    }
});
