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
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromoSupport', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('Number'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('EndDate'),
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
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProdCostPer1Item',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('PlanProdCostPer1Item'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProdCostPer1Item',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoSupport').value('ActualProdCostPer1Item'),
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
