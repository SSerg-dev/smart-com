Ext.define('App.view.tpm.costproduction.CostProductionEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.costproductioneditor',
    width: 800,
    minWidth: 800,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'numberfield',
            name: 'PlanProdCostPer1Item',
            fieldLabel: l10n.ns('tpm', 'CostProduction').value('PlanProdCostPer1Item'),
        }, {
            xtype: 'numberfield',
            name: 'ActualProdCostPer1Item',
            fieldLabel: l10n.ns('tpm', 'CostProduction').value('ActualProdCostPer1Item'),
        }, {
            xtype: 'numberfield',
            name: 'PlanProdCost',
            fieldLabel: l10n.ns('tpm', 'CostProduction').value('PlanProdCost'),
        }, {
            xtype: 'numberfield',
            name: 'ActualProdCost',
            fieldLabel: l10n.ns('tpm', 'CostProduction').value('ActualProdCost'),
        }]
    }
});
