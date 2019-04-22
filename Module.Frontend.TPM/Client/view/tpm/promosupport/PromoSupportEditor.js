Ext.define('App.view.tpm.promosupport.PromoSupportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promosupporteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }, {
            xtype: 'searchfield',
            name: 'BudgetSubItemId',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('BudgetSubItemName'),
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetSubItemName'
            }, {
                from: 'BudgetItemName',
                to: 'BudgetSubItemBudgetItemName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemBudgetItemName',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('BudgetSubItemBudgetItemName')
        }, {
            xtype: 'numberfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('PlanQuantity'),
            minValue: 0,
            maxValue: 100,
        }, {
            xtype: 'numberfield',
            name: 'ActualQuantity',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ActualQuantity'),
            minValue: 0,
            maxValue: 100,
        }, {
            xtype: 'numberfield',
            name: 'PlanCostTE',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('PlanCostTE'),
            minValue: 0,
            maxValue: 100,
        }, {
            xtype: 'numberfield',
            name: 'ActualCostTE',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ActualCostTE'),
            minValue: 0,
            maxValue: 100,
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('StartDate'),
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('EndDate'),
        }]
    }
});
