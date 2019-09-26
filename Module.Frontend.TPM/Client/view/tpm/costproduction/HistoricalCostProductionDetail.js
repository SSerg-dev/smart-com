Ext.define('App.view.tpm.costproduction.HistoricalCostProductionDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcostproductiondetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('ActualQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('PlanCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('ActualCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProdCost',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('PlanProdCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProdCost',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('ActualProdCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'AttachFileName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('AttachFileName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PONumber',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('PONumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InvoiceNumber',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('InvoiceNumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('BudgetSubItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemBudgetItemName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCostProduction').value('BudgetSubItemBudgetItemName'),
        }]
    }
});