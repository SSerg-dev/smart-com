Ext.define('App.view.tpm.sale.SaleEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.saleeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    afterWindowShow: function () {
        this.down('searchfield[name=BudgetItemId]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemBudgetName'),
            name: 'BudgetItemBudgetName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            needUpdateMappings: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetitem.BudgetItem',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetItemName'
            }, {
                from: 'BudgetName',
                to: 'BudgetItemBudgetName'
            }]
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'Plan',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Sale').value('Plan'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'Fact',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Sale').value('Fact'),
        }]
    }
});
